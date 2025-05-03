using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixStatsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "advertiser_offer_stats");

            migrationBuilder.DropColumn(
                name: "conversion_rate",
                table: "PublisherOfferStats");

            migrationBuilder.DropColumn(
                name: "publisher_code",
                table: "PublisherOfferStats");

            migrationBuilder.RenameColumn(
                name: "revenue",
                table: "PublisherOfferStats",
                newName: "total_revenue");

            migrationBuilder.RenameColumn(
                name: "offer_id",
                table: "PublisherOfferStats",
                newName: "campaign_id");

            migrationBuilder.RenameColumn(
                name: "conversion_count",
                table: "PublisherOfferStats",
                newName: "total_verified_click");

            migrationBuilder.RenameColumn(
                name: "click_count",
                table: "PublisherOfferStats",
                newName: "total_tablet");

            migrationBuilder.AddColumn<int>(
                name: "total_click",
                table: "PublisherOfferStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "total_fraud_click",
                table: "PublisherOfferStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "total_mobile",
                table: "PublisherOfferStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "total_phone",
                table: "PublisherOfferStats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AdvertiserOfferStats",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    campaign_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    total_click = table.Column<int>(type: "int", nullable: false),
                    total_verified_click = table.Column<int>(type: "int", nullable: false),
                    total_fraud_click = table.Column<int>(type: "int", nullable: false),
                    total_joined_publisher = table.Column<int>(type: "int", nullable: false),
                    total_rejected_publisher = table.Column<int>(type: "int", nullable: false),
                    total_budget_spent = table.Column<decimal>(type: "decimal(12,0)", nullable: false),
                    total_mobile = table.Column<int>(type: "int", nullable: false),
                    total_phone = table.Column<int>(type: "int", nullable: false),
                    total_tablet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertiserOfferStats", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertiserOfferStats");

            migrationBuilder.DropColumn(
                name: "total_click",
                table: "PublisherOfferStats");

            migrationBuilder.DropColumn(
                name: "total_fraud_click",
                table: "PublisherOfferStats");

            migrationBuilder.DropColumn(
                name: "total_mobile",
                table: "PublisherOfferStats");

            migrationBuilder.DropColumn(
                name: "total_phone",
                table: "PublisherOfferStats");

            migrationBuilder.RenameColumn(
                name: "total_verified_click",
                table: "PublisherOfferStats",
                newName: "conversion_count");

            migrationBuilder.RenameColumn(
                name: "total_tablet",
                table: "PublisherOfferStats",
                newName: "click_count");

            migrationBuilder.RenameColumn(
                name: "total_revenue",
                table: "PublisherOfferStats",
                newName: "revenue");

            migrationBuilder.RenameColumn(
                name: "campaign_id",
                table: "PublisherOfferStats",
                newName: "offer_id");

            migrationBuilder.AddColumn<decimal>(
                name: "conversion_rate",
                table: "PublisherOfferStats",
                type: "decimal(12,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "publisher_code",
                table: "PublisherOfferStats",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "advertiser_offer_stats",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    click_count = table.Column<int>(type: "int", nullable: false),
                    conversion_count = table.Column<int>(type: "int", nullable: false),
                    conversion_rate = table.Column<decimal>(type: "decimal(12,0)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    offer_id = table.Column<long>(type: "bigint", nullable: false),
                    publisher_count = table.Column<int>(type: "int", nullable: false),
                    revenue = table.Column<decimal>(type: "decimal(12,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_advertiser_offer_stats", x => x.id);
                });
        }
    }
}
