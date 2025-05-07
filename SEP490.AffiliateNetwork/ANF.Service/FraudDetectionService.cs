using ANF.Core;
using ANF.Core.Commons;
using ANF.Core.Enums;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace ANF.Service
{
    public class FraudDetectionService(IUnitOfWork unitOfWork,
                                 IUserClaimsService userClaimsService) : IFraudDetectionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IUserClaimsService _userClaimsService = userClaimsService;

        public async Task<PaginationResponse<FraudDetectionResponse>> GetFraudDetections(PaginationRequest request, DateTime from, DateTime to)
        {
                
            if (from > to) throw new ArgumentException("To date must be after from date");

            var currentUserCode = _userClaimsService.GetClaim(ClaimConstants.NameId);

            var userRepository = _unitOfWork.GetRepository<User>();
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            var fraudRepository = _unitOfWork.GetRepository<FraudDetection>();
            var offerRepository = _unitOfWork.GetRepository<Offer>();
            var pubOfferRepository = _unitOfWork.GetRepository<PublisherOffer>();
            var trackingEventRepository = _unitOfWork.GetRepository<TrackingEvent>();

            var user = await userRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.UserCode == currentUserCode);

            IQueryable<FraudDetectionResponse> query;

            if(user!.Role == UserRoles.Advertiser)
            {
                query = fraudRepository.GetAll()
                    .AsNoTracking()
                    .Join(trackingEventRepository.GetAll().AsNoTracking(), //Join tracking event 
                        fd => fd.ClickId,
                        te => te.Id,
                        (fd, te) => new { fd, te })
                    .Join(offerRepository.GetAll().AsNoTracking(),//Join offer event 
                        fdt => fdt.te.OfferId,
                        o => o.Id,
                        (fdt, o) => new { fdt.fd, fdt.te, o })
                    .Join(campaignRepository.GetAll().AsNoTracking(),//Join campaign event 
                        fdto => fdto.o.CampaignId,
                        c => c.Id,
                        (fdto, c) => new { fdto.fd, fdto.te, fdto.o, c })
                    .Where(x => x.c.AdvertiserCode == currentUserCode
                        && x.fd.DetectedTime >= from
                        && x.fd.DetectedTime <= to)
                    .Select(x => new FraudDetectionResponse
                    {
                        Id = x.fd.Id,
                        ClickId = x.fd.ClickId,
                        Reason = x.fd.Reason,
                        DetectedTime = x.fd.DetectedTime,
                        OfferId = x.o.Id
                    });
            } 
            else if(user.Role == UserRoles.Publisher)
            {
                var offerIds = await pubOfferRepository.GetAll()
                    .AsNoTracking()
                    .Where(e => e.PublisherCode == currentUserCode)
                    .Select(e => e.OfferId)
                    .ToListAsync();

                query = fraudRepository.GetAll()
                    .AsNoTracking()
                    .Join(trackingEventRepository.GetAll().AsNoTracking(), //Join tracking event 
                        fd => fd.ClickId,
                        te => te.Id,
                        (fd, te) => new { fd, te })
                    .Join(offerRepository.GetAll().AsNoTracking(),//Join offer event 
                        fdt => fdt.te.OfferId,
                        o => o.Id,
                        (fdt, o) => new { fdt.fd, fdt.te, o })
                    .Where(x => offerIds.Contains(x.o.Id)
                        && x.fd.DetectedTime >= from
                        && x.fd.DetectedTime <= to)
                    .Select(x => new FraudDetectionResponse
                    {
                        Id = x.fd.Id,
                        ClickId = x.fd.ClickId,
                        Reason = x.fd.Reason,
                        DetectedTime = x.fd.DetectedTime,
                        OfferId = x.o.Id 
                    });
            }
            else
            {
                throw new UnauthorizedAccessException("User role is not authorized to access this function.");
            }

            var totalRecord = await query.CountAsync();
            var response = await query
                            .OrderByDescending(e => e.DetectedTime)
                            .Skip((request.pageNumber - 1) * request.pageSize)
                            .Take(request.pageSize)
                            .ToListAsync();

            if (!response.Any())
            {
                throw new KeyNotFoundException("No data for fraud detections!");
            }

            return new PaginationResponse<FraudDetectionResponse>(response, totalRecord, request.pageNumber, request.pageSize);

        }
    }
}
