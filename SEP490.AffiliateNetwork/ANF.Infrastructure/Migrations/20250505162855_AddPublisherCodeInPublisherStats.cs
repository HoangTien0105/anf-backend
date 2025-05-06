using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPublisherCodeInPublisherStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PublisherCampaignStats",
                table: "PublisherCampaignStats");

            migrationBuilder.RenameTable(
                name: "PublisherCampaignStats",
                newName: "PublisherOfferStats");

            migrationBuilder.AddColumn<string>(
                name: "publisher_code",
                table: "PublisherOfferStats",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublisherOfferStats",
                table: "PublisherOfferStats",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PublisherOfferStats",
                table: "PublisherOfferStats");

            migrationBuilder.DropColumn(
                name: "publisher_code",
                table: "PublisherOfferStats");

            migrationBuilder.RenameTable(
                name: "PublisherOfferStats",
                newName: "PublisherCampaignStats");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublisherCampaignStats",
                table: "PublisherCampaignStats",
                column: "id");
        }
    }
}
