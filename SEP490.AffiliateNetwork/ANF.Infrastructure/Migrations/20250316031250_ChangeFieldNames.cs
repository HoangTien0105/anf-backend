using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFieldNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_Campaigns_CampaignId",
                table: "PaymentTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_Subscriptions_SubscriptionId",
                table: "PaymentTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_Users_user_code",
                table: "PaymentTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_Wallets_wallet_id",
                table: "PaymentTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistories_PaymentTransactions_TransactionId",
                table: "WalletHistories");

            migrationBuilder.DropIndex(
                name: "IX_WalletHistories_TransactionId",
                table: "WalletHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentTransactions",
                table: "PaymentTransactions");

            migrationBuilder.RenameTable(
                name: "PaymentTransactions",
                newName: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "WalletHistories",
                newName: "transaction_id");

            migrationBuilder.RenameColumn(
                name: "CurrentBalance",
                table: "WalletHistories",
                newName: "current_balance");

            migrationBuilder.RenameColumn(
                name: "BalanceType",
                table: "WalletHistories",
                newName: "balance_type");

            migrationBuilder.RenameColumn(
                name: "expiry_date",
                table: "Users",
                newName: "token_expired_date");

            migrationBuilder.RenameColumn(
                name: "SubscriptionId",
                table: "Transactions",
                newName: "subscription_id");

            migrationBuilder.RenameColumn(
                name: "CampaignId",
                table: "Transactions",
                newName: "campaign_id");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTransactions_wallet_id",
                table: "Transactions",
                newName: "IX_Transactions_wallet_id");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTransactions_user_code",
                table: "Transactions",
                newName: "IX_Transactions_user_code");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTransactions_SubscriptionId",
                table: "Transactions",
                newName: "IX_Transactions_subscription_id");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTransactions_CampaignId",
                table: "Transactions",
                newName: "IX_Transactions_campaign_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "trans_id");

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistories_transaction_id",
                table: "WalletHistories",
                column: "transaction_id",
                unique: true,
                filter: "[transaction_id] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Campaigns_campaign_id",
                table: "Transactions",
                column: "campaign_id",
                principalTable: "Campaigns",
                principalColumn: "camp_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Subscriptions_subscription_id",
                table: "Transactions",
                column: "subscription_id",
                principalTable: "Subscriptions",
                principalColumn: "sub_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_user_code",
                table: "Transactions",
                column: "user_code",
                principalTable: "Users",
                principalColumn: "user_code");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_wallet_id",
                table: "Transactions",
                column: "wallet_id",
                principalTable: "Wallets",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistories_Transactions_transaction_id",
                table: "WalletHistories",
                column: "transaction_id",
                principalTable: "Transactions",
                principalColumn: "trans_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Campaigns_campaign_id",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Subscriptions_subscription_id",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_user_code",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_wallet_id",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistories_Transactions_transaction_id",
                table: "WalletHistories");

            migrationBuilder.DropIndex(
                name: "IX_WalletHistories_transaction_id",
                table: "WalletHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "PaymentTransactions");

            migrationBuilder.RenameColumn(
                name: "transaction_id",
                table: "WalletHistories",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "current_balance",
                table: "WalletHistories",
                newName: "CurrentBalance");

            migrationBuilder.RenameColumn(
                name: "balance_type",
                table: "WalletHistories",
                newName: "BalanceType");

            migrationBuilder.RenameColumn(
                name: "token_expired_date",
                table: "Users",
                newName: "expiry_date");

            migrationBuilder.RenameColumn(
                name: "subscription_id",
                table: "PaymentTransactions",
                newName: "SubscriptionId");

            migrationBuilder.RenameColumn(
                name: "campaign_id",
                table: "PaymentTransactions",
                newName: "CampaignId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_wallet_id",
                table: "PaymentTransactions",
                newName: "IX_PaymentTransactions_wallet_id");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_user_code",
                table: "PaymentTransactions",
                newName: "IX_PaymentTransactions_user_code");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_subscription_id",
                table: "PaymentTransactions",
                newName: "IX_PaymentTransactions_SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_campaign_id",
                table: "PaymentTransactions",
                newName: "IX_PaymentTransactions_CampaignId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentTransactions",
                table: "PaymentTransactions",
                column: "trans_id");

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistories_TransactionId",
                table: "WalletHistories",
                column: "TransactionId",
                unique: true,
                filter: "[TransactionId] IS NOT NULL");

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
                name: "FK_PaymentTransactions_Users_user_code",
                table: "PaymentTransactions",
                column: "user_code",
                principalTable: "Users",
                principalColumn: "user_code");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_Wallets_wallet_id",
                table: "PaymentTransactions",
                column: "wallet_id",
                principalTable: "Wallets",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistories_PaymentTransactions_TransactionId",
                table: "WalletHistories",
                column: "TransactionId",
                principalTable: "PaymentTransactions",
                principalColumn: "trans_id");
        }
    }
}
