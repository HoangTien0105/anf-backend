using ANF.Core;
using ANF.Core.Enums;
using ANF.Core.Models.Entities;
using ANF.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using MyCSharp.HttpUserAgentParser;
using System.Collections.Concurrent;
using System.Text.Json;

namespace ANF.Service
{
    public class TrackingService: ITrackingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ConcurrentQueue<PostbackData> _trackingQueue;
        private readonly CancellationTokenSource _cts;
        private readonly Task _processingTask;

        public TrackingService(IUnitOfWork unitOfWork, IMemoryCache cache, IServiceScopeFactory scopeFactory)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _trackingQueue = new ConcurrentQueue<PostbackData>();
            _cts = new CancellationTokenSource();
            _scopeFactory = scopeFactory;
            _processingTask = Task.Run(() => ProcessQueueAsync(_cts.Token));
        }

        public async Task<string> StoreParams(long offerId, long publisherId, HttpRequest httpRequest)
        {
            try
            {
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();
                var offerRepository = _unitOfWork.GetRepository<Offer>();

                // Tạo key duy nhất cho rate limiting dựa trên IP và User-Agent
                string deviceKey = $"{httpRequest.HttpContext.Connection.RemoteIpAddress}-{httpRequest.Headers["User-Agent"]}";
                string cacheKey = $"tracking_requests_{deviceKey}";
                int maxRequests = 5;
                TimeSpan decayMinutes = TimeSpan.FromMinutes(1);

                // Kiểm tra rate limiting
                int requestCount = _cache.Get<int>(cacheKey);
                if (requestCount >= maxRequests)
                {
                    throw new ArgumentException("Too many requests.");
                }
                _cache.Set(cacheKey, requestCount + 1, decayMinutes);

                string userAgent = httpRequest.Headers["User-Agent"];
                var uaInfor = HttpUserAgentParser.Parse(userAgent);
                if (offerId < 1 || uaInfor.IsRobot())
                {
                    throw new ArgumentException("Bot alert");
                }

                var offer = await offerRepository.GetAll()
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(e => e.Id == offerId);
                if (offer is null) throw new KeyNotFoundException("Offer does not exists");

                if (DateTime.UtcNow < offer.StartDate || DateTime.UtcNow > offer.EndDate)
                    throw new ArgumentException("Offer is not available.");

                var campaign = await campaignRepository.GetAll()
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(e => e.Id == offer.CampaignId);
                if (campaign is null) throw new KeyNotFoundException("Offer does not exists");
                if(campaign.Status != CampaignStatus.Started)
                    throw new ArgumentException("Campaign is not available.");

                Guid clickId = Guid.NewGuid();

                var postBackData = new PostbackData
                {
                    PublisherId = publisherId,
                    ClickId = clickId,
                    OfferId = offerId,
                    Status = 0
                };

                _trackingQueue.Enqueue(postBackData);

                var trackingData = new Dictionary<string, string>
                {
                    { "publisher_id", publisherId.ToString() },
                    { "click_id", clickId.ToString() },
                    { "offer_id", offerId.ToString() },
                }.Where(kv => kv.Value is not null).ToDictionary(kv => kv.Key, kv => kv.Value);

                string redirectUrl = await BuildRedirectUrl(campaign.ProductUrl, campaign.TrackingParams, trackingData);

                return redirectUrl;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task StoreTrackingData(PostbackData postbackData)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var postBackRepository = unitOfWork.GetRepository<PostbackData>();
                try
                {
                    postBackRepository.Add(postbackData);
                    await unitOfWork.SaveAsync();
                }
                catch (Exception)
                {
                    await unitOfWork.RollbackAsync();
                    throw;
                }
            }
        }

        private async Task ProcessQueueAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (_trackingQueue.TryDequeue(out var postbackData))
                    {
                        await StoreTrackingData(postbackData);
                    }
                    else
                    {
                        await Task.Delay(100, cancellationToken);
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
        }

        private async Task<string> BuildRedirectUrl(string productUrl,
                                    string trackingParamsJson,
                                    Dictionary<string, string> trackingData)
        {
            try
            {
                if (string.IsNullOrEmpty(trackingParamsJson))
                {
                    return productUrl;
                }

                var trackingParams = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(trackingParamsJson);
                if (trackingParams is null)
                {
                    return productUrl;
                }

                var paramsDict = new Dictionary<string, string>();
                foreach (var param in trackingParams)
                {
                    string paramName = param.ContainsKey("param_name") ? param["param_name"] : null;
                    string paramValueKey = param.ContainsKey("param_value") ? param["param_value"] : null;
                    if (!string.IsNullOrEmpty(paramName) && trackingData.ContainsKey(paramValueKey))
                    {
                        paramsDict[paramName] = trackingData[paramValueKey];
                    }
                }

                var queryString = string.Join("&", paramsDict.Select(
                                      p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
                return productUrl + (productUrl.Contains("?") ? "&" : "?") + queryString;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}