using ANF.Core.Models.Entities;
using ANF.Infrastructure.Configs;
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
        
        public DbSet<Image> Images { get; set; } = null!;

        private string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, false)
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
            new ImageTypeConfig().Configure(builder.Entity<Image>());
            new SubPurchaseTypeConfig().Configure(builder.Entity<SubPurchase>());

            builder.Entity<Subscription>()
                .Property(s => s.Id).ValueGeneratedNever();
            builder.Entity<Campaign>()
                .Property(c => c.Id).ValueGeneratedNever();
            builder.Entity<Category>()
                .Property(c => c.Id).ValueGeneratedNever();
            builder.Entity<User>()
                .Property(u => u.Id).ValueGeneratedNever();
        }
    }
}
