using ANF.Core;
using ANF.Core.Enums;
using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ANF.Service.Backgrounds
{
    public class AdminStatsBackgroundService(IServiceScopeFactory serviceScopeFactory,
        ILogger<AdminStatsBackgroundService> logger) : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly ILogger<AdminStatsBackgroundService> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;
                    var nextRunTime = DateTime.Today.AddHours(23).AddMinutes(30);
                    if (now > nextRunTime)
                        nextRunTime = nextRunTime.AddDays(1);

                    var delay = nextRunTime - now;
                    //_logger.LogInformation($"AdminStatsBackgroundService will run at {nextRunTime} (in {delay})");
                    await Task.Delay(delay, stoppingToken);

                    using var scope = _serviceScopeFactory.CreateScope();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    await AddStatsDataForAdmin(unitOfWork);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, e.StackTrace);
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }

        private async Task AddStatsDataForAdmin(IUnitOfWork unitOfWork)
        {
            var userRepository = unitOfWork.GetRepository<User>();
            var campaignRepository = unitOfWork.GetRepository<Campaign>();
            var complaintTicketRepository = unitOfWork.GetRepository<ComplaintTicket>();
            var adminStatsRepository = unitOfWork.GetRepository<AdminStats>();

            var totalUsers = await userRepository.GetAll()
                .AsNoTracking()
                .CountAsync();

            var totalCampaigns = await campaignRepository.GetAll()
                .AsNoTracking()
                .Where(c => c.Status == CampaignStatus.Pending || 
                    c.Status == CampaignStatus.Verified || 
                    c.Status == CampaignStatus.Started || 
                    c.Status == CampaignStatus.Ended)
                .CountAsync();

            var totalApprovedCampaigns = await campaignRepository.GetAll()
                .AsNoTracking()
                .Where(c => c.Status == CampaignStatus.Verified || 
                    c.Status == CampaignStatus.Started)
                .CountAsync();

            var totalRejectedCampaigns = await campaignRepository.GetAll()
                .AsNoTracking()
                .Where(c => c.Status == CampaignStatus.Rejected)
                .CountAsync();

            var totalTickets = await complaintTicketRepository.GetAll()
                .AsNoTracking()
                .CountAsync();

            var totalResolvedTickets = await complaintTicketRepository.GetAll()
                .AsNoTracking()
                .Where(c => c.Status == TicketStatus.Resolved)
                .CountAsync();

            var totalPendingTickets = await complaintTicketRepository.GetAll()
                .AsNoTracking()
                .Where(c => c.Status == TicketStatus.Pending)
                .CountAsync();

            var adminStats = new AdminStats
            {
                TotalUser = totalUsers,
                TotalCampaign = totalCampaigns,
                TotalApprovedCampaign = totalApprovedCampaigns,
                TotalRejectedCampaign = totalRejectedCampaigns,
                TotalTicket = totalTickets,
                TotalResolvedTicket = totalResolvedTickets,
                TotalPendingTicket = totalPendingTickets,
                Date = DateTime.Now,
            };
            adminStatsRepository.Add(adminStats);
            await unitOfWork.SaveAsync();
        }
    }
}
