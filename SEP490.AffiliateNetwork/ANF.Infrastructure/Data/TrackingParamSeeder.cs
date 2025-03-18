using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ANF.Infrastructure.Data
{
    public static class TrackingParamSeeder
    {
        public static void SeedTrackingParams(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrackingParam>().HasData(
                new TrackingParam
                {
                    Id = 1,
                    Name = "source",
                    Description = "Traffic source identifier"
                },
                new TrackingParam
                {
                    Id = 2,
                    Name = "publisher_id",
                    Description = "Unique identifier for the affiliate"
                },
                new TrackingParam
                {
                    Id = 3,
                    Name = "campaign_id",
                    Description = "Specific campaign identifier"
                },
                new TrackingParam
                {
                    Id = 4,
                    Name = "sub_id",
                    Description = "Sub-affiliate identifier"
                },
                new TrackingParam
                {
                    Id = 5,
                    Name = "channel",
                    Description = "Marketing channel (email, social, search, etc.)"
                },
                new TrackingParam
                {
                    Id = 6,
                    Name = "keyword",
                    Description = "Keyword that triggered the ad"
                },
                new TrackingParam
                {
                    Id = 7,
                    Name = "device",
                    Description = "User device type (desktop, mobile, tablet)"
                },
                new TrackingParam
                {
                    Id = 8,
                    Name = "country",
                    Description = "User country code"
                },
                new TrackingParam
                {
                    Id = 9,
                    Name = "referrer",
                    Description = "URL of the referring website"
                },
                new TrackingParam
                {
                    Id = 10,
                    Name = "landing_page",
                    Description = "Specific landing page URL or identifier"
                },
                new TrackingParam
                {
                    Id = 11,
                    Name = "click_id",
                    Description = "Unique identifier for each click"
                },
                new TrackingParam
                {
                    Id = 12,
                    Name = "utm_source",
                    Description = "UTM parameter for traffic source"
                },
                new TrackingParam
                {
                    Id = 13,
                    Name = "utm_campaign",
                    Description = "UTM parameter for campaign name"
                },
                new TrackingParam
                {
                    Id = 14,
                    Name = "utm_content",
                    Description = "UTM parameter for content identifier"
                },
                new TrackingParam
                {
                    Id = 15,
                    Name = "utm_term",
                    Description = "UTM parameter for search terms"
                },
                new TrackingParam
                {
                    Id = 16,
                    Name = "payout",
                    Description = "Commission amount for this affiliate"
                }
            );
        }
    }
}
