using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEntitiesAndRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropColumn(
                name: "budget",
                table: "Campaigns");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                table: "Offers",
                newName: "concurrency_stamp");

            migrationBuilder.RenameColumn(
                name: "offer_note",
                table: "Offers",
                newName: "order_return_time");

            migrationBuilder.AddColumn<string>(
                name: "reject_reason",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "bid",
                table: "Offers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "budget",
                table: "Offers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "commission_rate",
                table: "Offers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "img_url",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "offer_description",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "step_info",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Campaigns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "balance",
                table: "Campaigns",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "reject_reason",
                table: "Campaigns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CampaignImages",
                columns: table => new
                {
                    img_no = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    camp_id = table.Column<long>(type: "bigint", nullable: true),
                    img_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    added_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignImages", x => x.img_no);
                    table.ForeignKey(
                        name: "FK_CampaignImages_Campaigns_camp_id",
                        column: x => x.camp_id,
                        principalTable: "Campaigns",
                        principalColumn: "camp_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PostbackData",
                columns: table => new
                {
                    pbd_no = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    click_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    offer_id = table.Column<long>(type: "bigint", nullable: false),
                    publisher_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<double>(type: "float", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostbackData", x => x.pbd_no);
                    table.ForeignKey(
                        name: "FK_PostbackData_Offers_offer_id",
                        column: x => x.offer_id,
                        principalTable: "Offers",
                        principalColumn: "offer_id");
                });

            migrationBuilder.CreateTable(
                name: "PublisherOffers",
                columns: table => new
                {
                    po_no = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    offer_id = table.Column<long>(type: "bigint", nullable: false),
                    publisher_id = table.Column<long>(type: "bigint", nullable: false),
                    joining_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reject_reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublisherOffers", x => x.po_no);
                    table.ForeignKey(
                        name: "FK_PublisherOffers_Offers_offer_id",
                        column: x => x.offer_id,
                        principalTable: "Offers",
                        principalColumn: "offer_id");
                    table.ForeignKey(
                        name: "FK_PublisherOffers_Users_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    balance = table.Column<double>(type: "float", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.id);
                    table.ForeignKey(
                        name: "FK_Wallets_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    trans_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    wallet_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    amount = table.Column<double>(type: "float", nullable: false),
                    payment_status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.trans_id);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Wallets_wallet_id",
                        column: x => x.wallet_id,
                        principalTable: "Wallets",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "WalletHistories",
                columns: table => new
                {
                    wh_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    amount = table.Column<double>(type: "float", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transaction_id = table.Column<long>(type: "bigint", nullable: true),
                    offer_id = table.Column<long>(type: "bigint", nullable: true),
                    subscription_id = table.Column<long>(type: "bigint", nullable: true),
                    wallet_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletHistories", x => x.wh_id);
                    table.ForeignKey(
                        name: "FK_WalletHistories_Wallets_wallet_id",
                        column: x => x.wallet_id,
                        principalTable: "Wallets",
                        principalColumn: "id");
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 100L,
                column: "reject_reason",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 101L,
                column: "reject_reason",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 103L,
                column: "reject_reason",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_CampaignImages_camp_id",
                table: "CampaignImages",
                column: "camp_id");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_user_id",
                table: "PaymentTransactions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_wallet_id",
                table: "PaymentTransactions",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_PostbackData_offer_id",
                table: "PostbackData",
                column: "offer_id");

            migrationBuilder.CreateIndex(
                name: "IX_PublisherOffers_offer_id",
                table: "PublisherOffers",
                column: "offer_id");

            migrationBuilder.CreateIndex(
                name: "IX_PublisherOffers_publisher_id",
                table: "PublisherOffers",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistories_wallet_id",
                table: "WalletHistories",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_user_id",
                table: "Wallets",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignImages");

            migrationBuilder.DropTable(
                name: "PaymentTransactions");

            migrationBuilder.DropTable(
                name: "PostbackData");

            migrationBuilder.DropTable(
                name: "PublisherOffers");

            migrationBuilder.DropTable(
                name: "WalletHistories");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropColumn(
                name: "reject_reason",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "bid",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "budget",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "commission_rate",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "img_url",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "offer_description",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "step_info",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "reject_reason",
                table: "Campaigns");

            migrationBuilder.RenameColumn(
                name: "concurrency_stamp",
                table: "Offers",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "order_return_time",
                table: "Offers",
                newName: "offer_note");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Campaigns",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "balance",
                table: "Campaigns",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "budget",
                table: "Campaigns",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    img_no = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    campaign_id = table.Column<long>(type: "bigint", nullable: true),
                    offer_id = table.Column<long>(type: "bigint", nullable: true),
                    img_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.img_no);
                    table.ForeignKey(
                        name: "FK_Images_Campaigns_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "Campaigns",
                        principalColumn: "camp_id");
                    table.ForeignKey(
                        name: "FK_Images_Offers_offer_id",
                        column: x => x.offer_id,
                        principalTable: "Offers",
                        principalColumn: "offer_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_campaign_id",
                table: "Images",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_Images_offer_id",
                table: "Images",
                column: "offer_id");
        }
    }
}
