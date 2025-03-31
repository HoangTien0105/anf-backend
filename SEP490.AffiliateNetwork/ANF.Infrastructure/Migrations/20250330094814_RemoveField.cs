using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "click_date",
                table: "TrackingValidations");

            migrationBuilder.DropColumn(
                name: "publisher_code",
                table: "FraudDetections");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "click_date",
                table: "TrackingValidations",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "publisher_code",
                table: "FraudDetections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
