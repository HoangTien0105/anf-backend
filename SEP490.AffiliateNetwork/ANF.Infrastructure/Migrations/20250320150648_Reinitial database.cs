using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Reinitialdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    cate_id = table.Column<long>(type: "bigint", nullable: false),
                    cate_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.cate_id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    sub_id = table.Column<long>(type: "bigint", nullable: false),
                    sub_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sub_price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    duration = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.sub_id);
                });

            migrationBuilder.CreateTable(
                name: "TrackingParams",
                columns: table => new
                {
                    param_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingParams", x => x.param_id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    user_code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    citizen_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    user_email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    user_password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email_confirmed = table.Column<bool>(type: "bit", nullable: true),
                    user_status = table.Column<int>(type: "int", nullable: false),
                    user_role = table.Column<int>(type: "int", nullable: false),
                    reset_password_token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    token_expired_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    reject_reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    concurrency_stamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
                    table.UniqueConstraint("AK_Users_user_code", x => x.user_code);
                });

            migrationBuilder.CreateTable(
                name: "AdvertiserProfiles",
                columns: table => new
                {
                    adv_no = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    company_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    industry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    advertiser_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertiserProfiles", x => x.adv_no);
                    table.ForeignKey(
                        name: "FK_AdvertiserProfiles_Users_advertiser_id",
                        column: x => x.advertiser_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    camp_id = table.Column<long>(type: "bigint", nullable: false),
                    advertiser_code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    camp_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    product_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tracking_params = table.Column<string>(type: "text", nullable: true),
                    reject_reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cate_id = table.Column<long>(type: "bigint", nullable: true),
                    camp_status = table.Column<int>(type: "int", nullable: false),
                    concurrency_stamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.camp_id);
                    table.ForeignKey(
                        name: "FK_Campaigns_Categories_cate_id",
                        column: x => x.cate_id,
                        principalTable: "Categories",
                        principalColumn: "cate_id");
                    table.ForeignKey(
                        name: "FK_Campaigns_Users_advertiser_code",
                        column: x => x.advertiser_code,
                        principalTable: "Users",
                        principalColumn: "user_code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublisherProfiles",
                columns: table => new
                {
                    pub_no = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    publisher_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublisherProfiles", x => x.pub_no);
                    table.ForeignKey(
                        name: "FK_PublisherProfiles_Users_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "TrafficSources",
                columns: table => new
                {
                    pubs_no = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    publisher_id = table.Column<long>(type: "bigint", nullable: false),
                    provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    soruce_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficSources", x => x.pubs_no);
                    table.ForeignKey(
                        name: "FK_TrafficSources_Users_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "UserBank",
                columns: table => new
                {
                    ub_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_code = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    banking_no = table.Column<long>(type: "bigint", nullable: false),
                    banking_provider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    added_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBank", x => x.ub_id);
                    table.ForeignKey(
                        name: "FK_UserBank_Users_user_code",
                        column: x => x.user_code,
                        principalTable: "Users",
                        principalColumn: "user_code",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    balance = table.Column<double>(type: "float", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    user_code = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.id);
                    table.ForeignKey(
                        name: "FK_Wallets_Users_user_code",
                        column: x => x.user_code,
                        principalTable: "Users",
                        principalColumn: "user_code");
                });

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
                name: "Offers",
                columns: table => new
                {
                    offer_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    camp_id = table.Column<long>(type: "bigint", nullable: false),
                    pricing_model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    offer_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    step_info = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bid = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    budget = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    commission_rate = table.Column<double>(type: "float", nullable: true),
                    order_return_time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    img_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    concurrency_stamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.offer_id);
                    table.ForeignKey(
                        name: "FK_Offers_Campaigns_camp_id",
                        column: x => x.camp_id,
                        principalTable: "Campaigns",
                        principalColumn: "camp_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    trans_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    wallet_id = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    campaign_id = table.Column<long>(type: "bigint", nullable: true),
                    subscription_id = table.Column<long>(type: "bigint", nullable: true),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.trans_id);
                    table.ForeignKey(
                        name: "FK_Transactions_Campaigns_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "Campaigns",
                        principalColumn: "camp_id");
                    table.ForeignKey(
                        name: "FK_Transactions_Subscriptions_subscription_id",
                        column: x => x.subscription_id,
                        principalTable: "Subscriptions",
                        principalColumn: "sub_id");
                    table.ForeignKey(
                        name: "FK_Transactions_Users_user_code",
                        column: x => x.user_code,
                        principalTable: "Users",
                        principalColumn: "user_code");
                    table.ForeignKey(
                        name: "FK_Transactions_Wallets_wallet_id",
                        column: x => x.wallet_id,
                        principalTable: "Wallets",
                        principalColumn: "id");
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
                    publisher_code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    joining_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    approved_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    reject_reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_PublisherOffers_Users_publisher_code",
                        column: x => x.publisher_code,
                        principalTable: "Users",
                        principalColumn: "user_code");
                });

            migrationBuilder.CreateTable(
                name: "TrackingEvents",
                columns: table => new
                {
                    click_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    offer_id = table.Column<long>(type: "bigint", nullable: false),
                    publisher_code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ip_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_agent = table.Column<string>(type: "text", nullable: true),
                    site_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    carrier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    click_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    referer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    proxy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingEvents", x => x.click_id);
                    table.ForeignKey(
                        name: "FK_TrackingEvents_Offers_offer_id",
                        column: x => x.offer_id,
                        principalTable: "Offers",
                        principalColumn: "offer_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WalletHistories",
                columns: table => new
                {
                    wh_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    transaction_id = table.Column<long>(type: "bigint", nullable: true),
                    current_balance = table.Column<double>(type: "float", nullable: true),
                    balance_type = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletHistories", x => x.wh_id);
                    table.ForeignKey(
                        name: "FK_WalletHistories_Transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "Transactions",
                        principalColumn: "trans_id");
                });

            migrationBuilder.CreateTable(
                name: "FraudDetections",
                columns: table => new
                {
                    fraud_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    click_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    offer_id = table.Column<long>(type: "bigint", nullable: false),
                    publisher_id = table.Column<long>(type: "bigint", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    detected_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FraudDetections", x => x.fraud_id);
                    table.ForeignKey(
                        name: "FK_FraudDetections_TrackingEvents_click_id",
                        column: x => x.click_id,
                        principalTable: "TrackingEvents",
                        principalColumn: "click_id");
                });

            migrationBuilder.CreateTable(
                name: "TrackingValidations",
                columns: table => new
                {
                    validation_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    click_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    offer_id = table.Column<long>(type: "bigint", nullable: false),
                    validated_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    click_date = table.Column<DateOnly>(type: "date", nullable: false),
                    conversion_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    revenue = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingValidations", x => x.validation_id);
                    table.ForeignKey(
                        name: "FK_TrackingValidations_TrackingEvents_click_id",
                        column: x => x.click_id,
                        principalTable: "TrackingEvents",
                        principalColumn: "click_id");
                });

            migrationBuilder.InsertData(
                table: "TrackingParams",
                columns: new[] { "param_id", "description", "name" },
                values: new object[,]
                {
                    { 1L, "Traffic source identifier", "source" },
                    { 2L, "Unique identifier for the affiliate", "publisher_id" },
                    { 3L, "Specific campaign identifier", "campaign_id" },
                    { 4L, "Sub-affiliate identifier", "sub_id" },
                    { 5L, "Marketing channel (email, social, search, etc.)", "channel" },
                    { 6L, "Keyword that triggered the ad", "keyword" },
                    { 7L, "User device type (desktop, mobile, tablet)", "device" },
                    { 8L, "User country code", "country" },
                    { 9L, "URL of the referring website", "referrer" },
                    { 10L, "Specific landing page URL or identifier", "landing_page" },
                    { 11L, "Unique identifier for each click", "click_id" },
                    { 12L, "UTM parameter for traffic source", "utm_source" },
                    { 13L, "UTM parameter for campaign name", "utm_campaign" },
                    { 14L, "UTM parameter for content identifier", "utm_content" },
                    { 15L, "UTM parameter for search terms", "utm_term" },
                    { 16L, "Commission amount for this affiliate", "payout" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertiserProfiles_advertiser_id",
                table: "AdvertiserProfiles",
                column: "advertiser_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CampaignImages_camp_id",
                table: "CampaignImages",
                column: "camp_id");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_advertiser_code",
                table: "Campaigns",
                column: "advertiser_code");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_cate_id",
                table: "Campaigns",
                column: "cate_id");

            migrationBuilder.CreateIndex(
                name: "IX_FraudDetections_click_id",
                table: "FraudDetections",
                column: "click_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_camp_id",
                table: "Offers",
                column: "camp_id");

            migrationBuilder.CreateIndex(
                name: "IX_PostbackData_offer_id",
                table: "PostbackData",
                column: "offer_id");

            migrationBuilder.CreateIndex(
                name: "IX_PublisherOffers_offer_id",
                table: "PublisherOffers",
                column: "offer_id");

            migrationBuilder.CreateIndex(
                name: "IX_PublisherOffers_publisher_code",
                table: "PublisherOffers",
                column: "publisher_code");

            migrationBuilder.CreateIndex(
                name: "IX_PublisherProfiles_publisher_id",
                table: "PublisherProfiles",
                column: "publisher_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingEvents_offer_id",
                table: "TrackingEvents",
                column: "offer_id");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingEvents_publisher_code_offer_id",
                table: "TrackingEvents",
                columns: new[] { "publisher_code", "offer_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingValidations_click_id",
                table: "TrackingValidations",
                column: "click_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrafficSources_publisher_id",
                table: "TrafficSources",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_campaign_id",
                table: "Transactions",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_subscription_id",
                table: "Transactions",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_user_code",
                table: "Transactions",
                column: "user_code");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_wallet_id",
                table: "Transactions",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserBank_user_code",
                table: "UserBank",
                column: "user_code");

            migrationBuilder.CreateIndex(
                name: "IX_Users_user_code",
                table: "Users",
                column: "user_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_user_email",
                table: "Users",
                column: "user_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistories_transaction_id",
                table: "WalletHistories",
                column: "transaction_id",
                unique: true,
                filter: "[transaction_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_user_code",
                table: "Wallets",
                column: "user_code",
                unique: true,
                filter: "[user_code] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertiserProfiles");

            migrationBuilder.DropTable(
                name: "CampaignImages");

            migrationBuilder.DropTable(
                name: "FraudDetections");

            migrationBuilder.DropTable(
                name: "PostbackData");

            migrationBuilder.DropTable(
                name: "PublisherOffers");

            migrationBuilder.DropTable(
                name: "PublisherProfiles");

            migrationBuilder.DropTable(
                name: "TrackingParams");

            migrationBuilder.DropTable(
                name: "TrackingValidations");

            migrationBuilder.DropTable(
                name: "TrafficSources");

            migrationBuilder.DropTable(
                name: "UserBank");

            migrationBuilder.DropTable(
                name: "WalletHistories");

            migrationBuilder.DropTable(
                name: "TrackingEvents");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
