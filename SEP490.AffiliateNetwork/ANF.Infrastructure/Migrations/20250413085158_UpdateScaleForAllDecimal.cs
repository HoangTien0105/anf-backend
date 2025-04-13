using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScaleForAllDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "balance",
                table: "Wallets",
                type: "decimal(12,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "current_balance",
                table: "WalletHistories",
                type: "decimal(12,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "TrackingValidations",
                type: "decimal(12,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_price",
                table: "Subscriptions",
                type: "decimal(12,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "PurchaseLogs",
                type: "decimal(12,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "revenue",
                table: "PublisherOfferStats",
                type: "decimal(12,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "conversion_rate",
                table: "PublisherOfferStats",
                type: "decimal(12,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "budget",
                table: "Offers",
                type: "decimal(12,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "bid",
                table: "Offers",
                type: "decimal(12,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "balance",
                table: "Campaigns",
                type: "decimal(12,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "BatchPayments",
                type: "decimal(12,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "revenue",
                table: "advertiser_offer_stats",
                type: "decimal(12,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "conversion_rate",
                table: "advertiser_offer_stats",
                type: "decimal(12,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "balance",
                table: "Wallets",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "current_balance",
                table: "WalletHistories",
                type: "decimal(10,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "TrackingValidations",
                type: "decimal(10,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "sub_price",
                table: "Subscriptions",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "PurchaseLogs",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "revenue",
                table: "PublisherOfferStats",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "conversion_rate",
                table: "PublisherOfferStats",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "budget",
                table: "Offers",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "bid",
                table: "Offers",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "balance",
                table: "Campaigns",
                type: "decimal(10,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "BatchPayments",
                type: "decimal(10,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "revenue",
                table: "advertiser_offer_stats",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "conversion_rate",
                table: "advertiser_offer_stats",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,0)");
        }
    }
}
