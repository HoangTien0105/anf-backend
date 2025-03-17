using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTrackingParams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrackingParam",
                columns: table => new
                {
                    param_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingParam", x => x.param_id);
                });

            migrationBuilder.InsertData(
                table: "TrackingParam",
                columns: new[] { "param_id", "description", "name" },
                values: new object[,]
                {
                    { 1L, "Traffic source identifier", "source" },
                    { 2L, "Unique identifier for the affiliate", "publisher_id" },
                    { 3L, "Specific campaign identifier", "campaign_id" },
                    { 4L, "Sub-affiliate identifier", "sub_id" },
                    { 5L, "Marketing channel (email, social, search, etc.)", "channel" },
                    { 6L, "Keyword that triggered the ad", "keyword" },
                    { 7L, "User device type (desktop, mobile, tablet)", "device" },
                    { 8L, "User country code", "country" },
                    { 9L, "URL of the referring website", "referrer" },
                    { 10L, "Specific landing page URL or identifier", "landing_page" },
                    { 11L, "Unique identifier for each click", "click_id" },
                    { 12L, "UTM parameter for traffic source", "utm_source" },
                    { 13L, "UTM parameter for campaign name", "utm_campaign" },
                    { 14L, "UTM parameter for content identifier", "utm_content" },
                    { 15L, "UTM parameter for search terms", "utm_term" },
                    { 16L, "Commission amount for this affiliate", "payout" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackingParam");
        }
    }
}
