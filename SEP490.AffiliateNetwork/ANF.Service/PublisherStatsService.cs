using ANF.Core;
using ANF.Core.Commons;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ANF.Service
{
    public class PublisherStatsService(IUnitOfWork unitOfWork,
                                 IMapper mapper,
                                 IUserClaimsService userClaimsService) : IPublisherStatsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IUserClaimsService _userClaimsService = userClaimsService;
        public async Task<bool> GeneratePublisherOfferStatsByPublisherCode(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                {
                    throw new ArgumentException("End date must be after start date");
                }
                var publisherOfferStatsRepository = _unitOfWork.GetRepository<PublisherCampaignStats>();
                var userRepository = _unitOfWork.GetRepository<User>();
                var publisherOfferRepository = _unitOfWork.GetRepository<PublisherOffer>();
                var offerRepository = _unitOfWork.GetRepository<Offer>();

                var publisherCode = _userClaimsService.GetClaim(ClaimConstants.NameId);

                var publisher = await userRepository.GetAll()
                    .Where(u => u.Role == Core.Enums.UserRoles.Publisher
                    && u.Status == Core.Enums.UserStatus.Active
                    && u.UserCode == publisherCode)
                    .FirstOrDefaultAsync();

                if (publisher is null)
                {
                    throw new KeyNotFoundException("Publisher not found");
                }

                bool hasChanges = false;

                // Lấy các offer Id
                var publisherOffers = await publisherOfferRepository.GetAll()
                   .Where(po => po.PublisherCode == publisherCode
                             && po.Status == Core.Enums.PublisherOfferStatus.Approved)
                   .ToListAsync();

                var offerIds = publisherOffers.Select(po => po.OfferId).ToList();

                // Lấy các campaign của các offer
                var campaigns = await offerRepository.GetAll()
                    .Where(o => offerIds.Contains(o.Id) &&
                               (o.Status == Core.Enums.OfferStatus.Approved || o.Status == Core.Enums.OfferStatus.Started))
                    .GroupBy(o => o.CampaignId)
                    .ToListAsync();

                if (campaigns is null || !campaigns.Any())
                {
                    throw new KeyNotFoundException("Campaigns not found");
                }

                //handle từng campaign
                foreach (var campaign in campaigns)
                {
                    var campaignOffers = campaign.ToList();
                    var newCampaignStatsList = await AnalyzePublisherOfferStats(campaignOffers, publisherCode, startDate, endDate);

                    if (newCampaignStatsList != null && newCampaignStatsList.Any())
                    {
                        foreach (var stats in newCampaignStatsList)
                        {
                            publisherOfferStatsRepository.Add(stats);
                            hasChanges = true;
                        }
                    }
                }

                if (hasChanges)
                {
                    return await _unitOfWork.SaveAsync() > 0;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        private async Task<List<PublisherCampaignStats>?> AnalyzePublisherOfferStats(List<Offer> offers, string publisherCode, DateTime startDate, DateTime endDate)
        {
            try
            {
                var trackingRepository = _unitOfWork.GetRepository<TrackingEvent>();
                var validationRepository = _unitOfWork.GetRepository<TrackingValidation>();
                var fraudRepository = _unitOfWork.GetRepository<FraudDetection>();
                var statsRepository = _unitOfWork.GetRepository<PublisherCampaignStats>();

                var offerIds = offers.Select(o => o.Id).ToList();
                var statsList = new List<PublisherCampaignStats>();

                // Handle từng ngày
                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    var startOfDay = date;
                    var endOfDay = date.AddDays(1).AddTicks(-1);

                    // Get clicks for the current day
                    var clickList = await trackingRepository.GetAll()
                        .AsNoTracking()
                        .Where(t => offerIds.Contains(t.OfferId)
                                 && t.PublisherCode == publisherCode
                                 && t.ClickTime >= startOfDay
                                 && t.ClickTime <= endOfDay)
                        .ToListAsync();

                    int totalClicks = clickList.Count();

                    if (totalClicks == 0) continue;

                    var clickIds = clickList.Select(c => c.Id)
                        .Where(id => id != null)
                        .ToList();

                    var validatedClickList = await validationRepository.GetAll()
                        .AsNoTracking()
                        .Where(v => v.ClickId != null
                                 && clickIds.Contains(v.ClickId)
                                 && v.ValidationStatus == Core.Enums.ValidationStatus.Success
                                 && v.ConversionStatus == Core.Enums.ConversionStatus.Success
                                 && v.ValidatedTime >= startOfDay
                                 && v.ValidatedTime <= endOfDay)
                        .ToListAsync();

                    int totalValidateClicks = validatedClickList.Count();

                    var totalFraudClicks = await fraudRepository.GetAll()
                        .AsNoTracking()
                        .Where(f => f.ClickId != null
                                 && clickIds.Contains(f.ClickId)
                                 && f.DetectedTime >= startOfDay
                                 && f.DetectedTime <= endOfDay)
                        .CountAsync();

                    decimal totalRevenue = 0;
                    var validateClickByOffer = validatedClickList
                        .GroupBy(v => clickList.First(c => c.Id == v.ClickId).OfferId)
                        .ToDictionary(g => g.Key, g => g.ToList());

                    foreach (var offer in offers)
                    {
                        if (validateClickByOffer.TryGetValue(offer.Id, out var offerValidatedClicks))
                        {
                            if (offer.PricingModel == "CPS")
                            {
                                decimal sumAmount = offerValidatedClicks.Sum(v => v.Amount ?? 0);
                                totalRevenue += sumAmount * (decimal)(offer.CommissionRate ?? 0);
                            }
                            else
                            {
                                totalRevenue += offerValidatedClicks.Count * offer.Bid;
                            }
                        }
                    }

                    int totalMobile = 0;
                    int totalTablet = 0;
                    int totalDesktop = 0;

                    foreach (var click in clickList)
                    {
                        if (!string.IsNullOrEmpty(click.UserAgent))
                        {
                            string lowerUa = click.UserAgent.ToLower();
                            if (lowerUa.Contains("mobile") || lowerUa.Contains("android") || lowerUa.Contains("iphone"))
                            {
                                totalMobile++;
                            }
                            else if (lowerUa.Contains("tablet") || lowerUa.Contains("ipad"))
                            {
                                totalTablet++;
                            }
                            else
                            {
                                totalDesktop++;
                            }
                        }
                    }

                    var newPublisherOfferStats = new PublisherCampaignStats()
                    {
                        Date = date,
                        CampaignId = offers.First().CampaignId,
                        PublisherCode = publisherCode,
                        TotalClick = totalClicks,
                        TotalVerifiedClick = totalValidateClicks,
                        TotalFraudClick = totalFraudClicks,
                        TotalRevenue = totalRevenue,
                        TotalMobile = totalMobile,
                        TotalTablet = totalTablet,
                        TotalComputer = totalDesktop,
                    };

                    var existingStats = await statsRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Date == newPublisherOfferStats.Date
                                       && s.CampaignId == newPublisherOfferStats.CampaignId
                                       && s.PublisherCode == newPublisherOfferStats.PublisherCode);

                    if (existingStats is not null) continue;
                    statsList.Add(newPublisherOfferStats);
                }

                return statsList.Any() ? statsList : null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to analyze publisher offer stats for publisher {publisherCode}.", ex);
            }
        }

        public async Task<List<PublisherStatsResponse>> GetRevenueStatsByCampaignId(long campaignId, DateTime from, DateTime to)
        {
            if (from > to) throw new ArgumentException("To date must be behind from date");
            var statsRepository = _unitOfWork.GetRepository<PublisherCampaignStats>();

            var publisherCode = _userClaimsService.GetClaim(ClaimConstants.NameId);

            var stats = await statsRepository.GetAll()
                    .AsNoTracking()
                    .Where(e => e.PublisherCode == publisherCode
                             && e.CampaignId == campaignId
                             && e.Date >= from
                             && e.Date <= to)
                    .OrderBy(e => e.Date)
                    .ToListAsync();

            var response = stats
                    .GroupBy(e => e.Date.Date)
                    .Select(g => new PublisherStatsResponse
                    {
                        Date = g.Key,
                        Campaigns = g
                            .GroupBy(e => e.CampaignId)
                            .Select(cg => cg.OrderByDescending(e => e.Date).First())
                            .Select(e => new CampaignStatsDto
                            {
                                CampaignId = e.CampaignId,
                                TotalRevenue = e.TotalRevenue,
                                TotalClick = e.TotalClick,
                                TotalVerifiedClick = e.TotalVerifiedClick,
                                TotalFraudClick = e.TotalFraudClick,
                                TotalComputer = e.TotalComputer,
                                TotalMobile = e.TotalMobile,
                                TotalTablet = e.TotalTablet
                            })
                            .ToList()
                    })
                    .OrderBy(g => g.Date)
                    .ToList();

            return response;
        }

        public async Task<List<PublisherStatsResponse>> GetRevenueStats(DateTime from, DateTime to)
        {
            if (from > to) throw new ArgumentException("To date must be behind from date");
            var statsRepository = _unitOfWork.GetRepository<PublisherCampaignStats>();

            var publisherCode = _userClaimsService.GetClaim(ClaimConstants.NameId);

            var stats = await statsRepository.GetAll()
                    .AsNoTracking()
                    .Where(e => e.PublisherCode == publisherCode
                             && e.Date >= from
                             && e.Date <= to)
                    .OrderBy(e => e.Date)
                    .ToListAsync();

            var response = stats
                    .GroupBy(e => e.Date.Date) 
                    .Select(g => new PublisherStatsResponse
                    {
                        Date = g.Key,
                        Campaigns = g
                            .GroupBy(e => e.CampaignId) 
                            .Select(cg => cg.OrderByDescending(e => e.Date).First())
                            .Select(e => new CampaignStatsDto
                            {
                                CampaignId = e.CampaignId,
                                TotalRevenue = e.TotalRevenue,
                                TotalClick = e.TotalClick,
                                TotalVerifiedClick = e.TotalVerifiedClick,
                                TotalFraudClick = e.TotalFraudClick,
                                TotalComputer = e.TotalComputer,
                                TotalMobile = e.TotalMobile,
                                TotalTablet = e.TotalTablet 
                            })
                            .ToList()
                    })
                    .OrderBy(g => g.Date)
                    .ToList();

            return response;
        }
    }
}
