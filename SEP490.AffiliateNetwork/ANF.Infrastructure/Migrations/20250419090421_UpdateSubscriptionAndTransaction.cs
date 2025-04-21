using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubscriptionAndTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sub_price",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "duration",
                table: "Subscriptions",
                newName: "pricing_benefit");

            migrationBuilder.AddColumn<int>(
                name: "billing_type",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "valid_from",
                table: "Transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "valid_to",
                table: "Transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "max_created_campaign",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "price_per_month",
                table: "Subscriptions",
                type: "decimal(12,0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "price_per_year",
                table: "Subscriptions",
                type: "decimal(12,0)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "priority_level",
                table: "Subscriptions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "billing_type",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "valid_from",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "valid_to",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "max_created_campaign",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "price_per_month",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "price_per_year",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "priority_level",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "pricing_benefit",
                table: "Subscriptions",
                newName: "duration");

            migrationBuilder.AddColumn<decimal>(
                name: "sub_price",
                table: "Subscriptions",
                type: "decimal(12,0)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
