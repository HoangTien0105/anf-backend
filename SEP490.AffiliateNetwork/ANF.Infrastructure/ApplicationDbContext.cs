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
        
        public DbSet<PublisherSource> PublisherSources { get; set; } = null!;
        
        public DbSet<Subscription> Subscriptions { get; set; } = null!;
        
        public DbSet<SubPurchase> SubPurchases { get; set; } = null!;
        
        public DbSet<Category> Categories { get; set; } = null!;
        
        public DbSet<Campaign> Campaigns { get; set; } = null!;
        
        public DbSet<Offer> Offers { get; set; } = null!;
        
        public DbSet<CampaignImage> CampaignImages { get; set; } = null!;

        public DbSet<PublisherOffer> PublisherOffers { get; set; } = null!;

        public DbSet<PostbackData> PostbackData { get; set; } = null!;

        public DbSet<PaymentTransaction> PaymentTransactions { get; set; } = null!;

        public DbSet<Wallet> Wallets { get; set; } = null!;
        
        public DbSet<WalletHistory> WalletHistories { get; set; } = null!;

        /// <summary>
        /// Get connection string from appsettings.json
        /// </summary>
        /// <returns>The database connection string</returns>
        // NOTE: Can be removed the method and not call it in OnConfiguring(),
        // because it has already configured in Program.cs
        private string GetConnectionString()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
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
            new PublisherSourceTypeConfig().Configure(builder.Entity<PublisherSource>());
            new CampaignImageTypeConfig().Configure(builder.Entity<CampaignImage>());
            new SubPurchaseTypeConfig().Configure(builder.Entity<SubPurchase>());

            new WalletHistoryTypeConfig().Configure(builder.Entity<WalletHistory>());
            new PaymentTransactionTypeConfig().Configure(builder.Entity<PaymentTransaction>());
            new WalletTypeConfig().Configure(builder.Entity<Wallet>());
            new PublisherOfferTypeConfig().Configure(builder.Entity<PublisherOffer>());
            new PostbackDataTypeConfig().Configure(builder.Entity<PostbackData>());
            
            new UserTypeConfig().Configure(builder.Entity<User>());
            new UserBankTypeConfig().Configure(builder.Entity<UserBank>());
            
            // Other configurations
            builder.Entity<Subscription>()
                .Property(s => s.Id).ValueGeneratedNever();
            builder.Entity<Campaign>()
                .Property(c => c.Id).ValueGeneratedNever();
            builder.Entity<Category>()
                .Property(c => c.Id).ValueGeneratedNever();
            
            #region Data seeding
            builder.SeedDataForUsers();
            #endregion
        }
    }
}
