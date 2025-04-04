using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "click_id",
                table: "PostbackData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "advertiser_stats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    campaign_id = table.Column<long>(type: "bigint", nullable: false),
                    offer_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    click_count = table.Column<int>(type: "int", nullable: false),
                    conversion_count = table.Column<int>(type: "int", nullable: false),
                    conversion_rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    postback_success = table.Column<int>(type: "int", nullable: false),
                    postback_failed = table.Column<int>(type: "int", nullable: false),
                    validation_success = table.Column<int>(type: "int", nullable: false),
                    validation_failed = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_advertiser_stats", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "advertiser_stats");

            migrationBuilder.AlterColumn<Guid>(
                name: "click_id",
                table: "PostbackData",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
