using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixColumnNameInAdvertiserAndPublisherStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PublisherOfferStats",
                table: "PublisherOfferStats");

            migrationBuilder.RenameTable(
                name: "PublisherOfferStats",
                newName: "PublisherCampaignStats");

            migrationBuilder.RenameColumn(
                name: "total_phone",
                table: "AdvertiserOfferStats",
                newName: "total_computer");

            migrationBuilder.RenameColumn(
                name: "total_phone",
                table: "PublisherCampaignStats",
                newName: "total_computer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublisherCampaignStats",
                table: "PublisherCampaignStats",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PublisherCampaignStats",
                table: "PublisherCampaignStats");

            migrationBuilder.RenameTable(
                name: "PublisherCampaignStats",
                newName: "PublisherOfferStats");

            migrationBuilder.RenameColumn(
                name: "total_computer",
                table: "AdvertiserOfferStats",
                newName: "total_phone");

            migrationBuilder.RenameColumn(
                name: "total_computer",
                table: "PublisherOfferStats",
                newName: "total_phone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublisherOfferStats",
                table: "PublisherOfferStats",
                column: "id");
        }
    }
}
