using ANF.Core.Models.Entities;
using ANF.Infrastructure.Configs;
using ANF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ANF.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<PublisherProfile> PublisherProfiles { get; set; } = null!;

        public DbSet<AdvertiserProfile> AdvertiserProfiles { get; set; } = null!;

        public DbSet<TrafficSource> TrafficSources { get; set; } = null!;

        public DbSet<Subscription> Subscriptions { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Campaign> Campaigns { get; set; } = null!;

        public DbSet<Offer> Offers { get; set; } = null!;

        public DbSet<CampaignImage> CampaignImages { get; set; } = null!;

        public DbSet<PublisherOffer> PublisherOffers { get; set; } = null!;

        public DbSet<PostbackData> PostbackData { get; set; } = null!;

        public DbSet<Transaction> PaymentTransactions { get; set; } = null!;

        public DbSet<Wallet> Wallets { get; set; } = null!;

        public DbSet<WalletHistory> WalletHistories { get; set; } = null!;

        public DbSet<TrackingParam> TrackingParams { get; set; } = null!;

        public DbSet<TrackingEvent> TrackingEvents { get; set; } = null!;

        public DbSet<TrackingValidation> TrackingValidations { get; set; } = null!;

        public DbSet<FraudDetection> FraudDetections { get; set; } = null!;

        public DbSet<Policy> Policies { get; set; } = null!;

        public DbSet<BatchPayment> BatchPayments { get; set; } = null!;

        public DbSet<AdvertiserOfferStats> AdvertiserOfferStats { get; set; } = null!;

        public DbSet<PublisherOfferStats> PublisherOfferStats { get; set; } = null!;

        public DbSet<PurchaseLog> PurchaseLogs { get; set; } = null!;

        /// <summary>
        /// Get connection string from appsettings.json
        /// </summary>
        /// <returns>The database connection string</returns>
        private string GetConnectionString()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            return configuration.GetConnectionString("Default") ?? string.Empty;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(GetConnectionString());
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            new AdvertiserProfileTypeConfig().Configure(builder.Entity<AdvertiserProfile>());
            new PublisherProfileTypeConfig().Configure(builder.Entity<PublisherProfile>());
            new PublisherSourceTypeConfig().Configure(builder.Entity<TrafficSource>());
            new CampaignImageTypeConfig().Configure(builder.Entity<CampaignImage>());

            new WalletHistoryTypeConfig().Configure(builder.Entity<WalletHistory>());
            new TransactionTypeConfig().Configure(builder.Entity<Transaction>());
            new WalletTypeConfig().Configure(builder.Entity<Wallet>());
            new PublisherOfferTypeConfig().Configure(builder.Entity<PublisherOffer>());
            new PostbackDataTypeConfig().Configure(builder.Entity<PostbackData>());

            new UserTypeConfig().Configure(builder.Entity<User>());
            new UserBankTypeConfig().Configure(builder.Entity<UserBank>());
            new CampaignTypeConfig().Configure(builder.Entity<Campaign>());

            new TrackingEventTypeConfig().Configure(builder.Entity<TrackingEvent>());
            new TrackingValidationTypeConfig().Configure(builder.Entity<TrackingValidation>());
            new FraudDetectionTypeConfig().Configure(builder.Entity<FraudDetection>());

            #region Other type configurations
            builder.Entity<Subscription>()
                .Property(s => s.Id).ValueGeneratedNever();
            builder.Entity<Campaign>()
                .Property(c => c.Id).ValueGeneratedNever();
            builder.Entity<Category>()
                .Property(c => c.Id).ValueGeneratedNever();
            #endregion

            builder.SeedTrackingParams();
        }
    }
}
