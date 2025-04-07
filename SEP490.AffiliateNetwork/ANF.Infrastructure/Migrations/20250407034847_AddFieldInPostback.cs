using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldInPostback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                table: "PostbackData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "transaction_id",
                table: "PostbackData",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date",
                table: "PostbackData");

            migrationBuilder.DropColumn(
                name: "transaction_id",
                table: "PostbackData");
        }
    }
}
