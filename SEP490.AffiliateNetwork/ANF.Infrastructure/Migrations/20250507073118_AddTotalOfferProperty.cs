using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalOfferProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PublisherOfferStats",
                table: "PublisherOfferStats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvertiserOfferStats",
                table: "AdvertiserOfferStats");

            migrationBuilder.RenameTable(
                name: "PublisherOfferStats",
                newName: "PublisherCampaignStats");

            migrationBuilder.RenameTable(
                name: "AdvertiserOfferStats",
                newName: "AdvertiserCampaignStats");

            migrationBuilder.AddColumn<int>(
                name: "total_offer",
                table: "AdvertiserCampaignStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublisherCampaignStats",
                table: "PublisherCampaignStats",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdvertiserCampaignStats",
                table: "AdvertiserCampaignStats",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PublisherCampaignStats",
                table: "PublisherCampaignStats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvertiserCampaignStats",
                table: "AdvertiserCampaignStats");

            migrationBuilder.DropColumn(
                name: "total_offer",
                table: "AdvertiserCampaignStats");

            migrationBuilder.RenameTable(
                name: "PublisherCampaignStats",
                newName: "PublisherOfferStats");

            migrationBuilder.RenameTable(
                name: "AdvertiserCampaignStats",
                newName: "AdvertiserOfferStats");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublisherOfferStats",
                table: "PublisherOfferStats",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdvertiserOfferStats",
                table: "AdvertiserOfferStats",
                column: "id");
        }
    }
}
