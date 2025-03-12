using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistories_Campaigns_campaign_id",
                table: "WalletHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistories_PaymentTransactions_PaymentTransactionId",
                table: "WalletHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistories_Subscriptions_subscription_id",
                table: "WalletHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistories_Wallets_wallet_id",
                table: "WalletHistories");

            migrationBuilder.DropTable(
                name: "SubPurchases");

            migrationBuilder.DropIndex(
                name: "IX_WalletHistories_campaign_id",
                table: "WalletHistories");

            migrationBuilder.DropIndex(
                name: "IX_WalletHistories_PaymentTransactionId",
                table: "WalletHistories");

            migrationBuilder.DropIndex(
                name: "IX_WalletHistories_subscription_id",
                table: "WalletHistories");

            migrationBuilder.DropIndex(
                name: "IX_WalletHistories_wallet_id",
                table: "WalletHistories");

            migrationBuilder.DropColumn(
                name: "PaymentTransactionId",
                table: "WalletHistories");

            migrationBuilder.DropColumn(
                name: "amount",
                table: "WalletHistories");

            migrationBuilder.DropColumn(
                name: "campaign_id",
                table: "WalletHistories");

            migrationBuilder.DropColumn(
                name: "changed_time",
                table: "WalletHistories");

            migrationBuilder.DropColumn(
                name: "subscription_id",
                table: "WalletHistories");

            migrationBuilder.DropColumn(
                name: "type",
                table: "WalletHistories");

            migrationBuilder.DropColumn(
                name: "wallet_balance",
                table: "WalletHistories");

            migrationBuilder.DropColumn(
                name: "wallet_id",
                table: "WalletHistories");

            migrationBuilder.RenameColumn(
                name: "transaction_id",
                table: "WalletHistories",
                newName: "TransactionId");

            migrationBuilder.AddColumn<bool>(
                name: "BalanceType",
                table: "WalletHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "CurrentBalance",
                table: "WalletHistories",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CampaignId",
                table: "PaymentTransactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SubscriptionId",
                table: "PaymentTransactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistories_TransactionId",
                table: "WalletHistories",
                column: "TransactionId",
                unique: true,
                filter: "[TransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_CampaignId",
                table: "PaymentTransactions",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_SubscriptionId",
                table: "PaymentTransactions",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_Campaigns_CampaignId",
                table: "PaymentTransactions",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "camp_id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_Subscriptions_SubscriptionId",
                table: "PaymentTransactions",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "sub_id");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistories_PaymentTransactions_TransactionId",
                table: "WalletHistories",
                column: "TransactionId",
                principalTable: "PaymentTransactions",
                principalColumn: "trans_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_Campaigns_CampaignId",
                table: "PaymentTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_Subscriptions_SubscriptionId",
                table: "PaymentTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistories_PaymentTransactions_TransactionId",
                table: "WalletHistories");

            migrationBuilder.DropIndex(
                name: "IX_WalletHistories_TransactionId",
                table: "WalletHistories");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTransactions_CampaignId",
                table: "PaymentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTransactions_SubscriptionId",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "BalanceType",
                table: "WalletHistories");

            migrationBuilder.DropColumn(
                name: "CurrentBalance",
                table: "WalletHistories");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "PaymentTransactions");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "WalletHistories",
                newName: "transaction_id");

            migrationBuilder.AddColumn<long>(
                name: "PaymentTransactionId",
                table: "WalletHistories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "amount",
                table: "WalletHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "campaign_id",
                table: "WalletHistories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "changed_time",
                table: "WalletHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "subscription_id",
                table: "WalletHistories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "WalletHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "wallet_balance",
                table: "WalletHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "wallet_id",
                table: "WalletHistories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "SubPurchases",
                columns: table => new
                {
                    subp_no = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    advertiser_code = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    sub_id = table.Column<long>(type: "bigint", nullable: true),
                    current_price = table.Column<double>(type: "float", nullable: false),
                    expired_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    purchased_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubPurchases", x => x.subp_no);
                    table.ForeignKey(
                        name: "FK_SubPurchases_Subscriptions_sub_id",
                        column: x => x.sub_id,
                        principalTable: "Subscriptions",
                        principalColumn: "sub_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SubPurchases_Users_advertiser_code",
                        column: x => x.advertiser_code,
                        principalTable: "Users",
                        principalColumn: "user_code",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistories_campaign_id",
                table: "WalletHistories",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistories_PaymentTransactionId",
                table: "WalletHistories",
                column: "PaymentTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistories_subscription_id",
                table: "WalletHistories",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistories_wallet_id",
                table: "WalletHistories",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_SubPurchases_advertiser_code",
                table: "SubPurchases",
                column: "advertiser_code");

            migrationBuilder.CreateIndex(
                name: "IX_SubPurchases_sub_id",
                table: "SubPurchases",
                column: "sub_id");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistories_Campaigns_campaign_id",
                table: "WalletHistories",
                column: "campaign_id",
                principalTable: "Campaigns",
                principalColumn: "camp_id");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistories_PaymentTransactions_PaymentTransactionId",
                table: "WalletHistories",
                column: "PaymentTransactionId",
                principalTable: "PaymentTransactions",
                principalColumn: "trans_id");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistories_Subscriptions_subscription_id",
                table: "WalletHistories",
                column: "subscription_id",
                principalTable: "Subscriptions",
                principalColumn: "sub_id");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistories_Wallets_wallet_id",
                table: "WalletHistories",
                column: "wallet_id",
                principalTable: "Wallets",
                principalColumn: "id");
        }
    }
}
