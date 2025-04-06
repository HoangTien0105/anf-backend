using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "advertiser_stats");

            migrationBuilder.CreateTable(
                name: "advertiser_offer_stats",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    offer_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    click_count = table.Column<int>(type: "int", nullable: false),
                    conversion_count = table.Column<int>(type: "int", nullable: false),
                    conversion_rate = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    publisher_count = table.Column<int>(type: "int", nullable: false),
                    revenue = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_advertiser_offer_stats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "publisher_offer_stats",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    offer_id = table.Column<long>(type: "bigint", nullable: false),
                    publisher_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    click_count = table.Column<int>(type: "int", nullable: false),
                    conversion_count = table.Column<int>(type: "int", nullable: false),
                    conversion_rate = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publisher_offer_stats", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "advertiser_offer_stats");

            migrationBuilder.DropTable(
                name: "publisher_offer_stats");

            migrationBuilder.CreateTable(
                name: "advertiser_stats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    campaign_id = table.Column<long>(type: "bigint", nullable: false),
                    click_count = table.Column<int>(type: "int", nullable: false),
                    conversion_count = table.Column<int>(type: "int", nullable: false),
                    conversion_rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    offer_id = table.Column<long>(type: "bigint", nullable: false),
                    postback_failed = table.Column<int>(type: "int", nullable: false),
                    postback_success = table.Column<int>(type: "int", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    validation_failed = table.Column<int>(type: "int", nullable: false),
                    validation_success = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_advertiser_stats", x => x.Id);
                });
        }
    }
}
