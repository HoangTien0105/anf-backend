using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.cate_id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "cate_id");
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    sub_id = table.Column<long>(type: "bigint", nullable: false),
                    sub_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sub_price = table.Column<double>(type: "float", nullable: false),
                    duration = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.sub_id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    citizen_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    user_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    user_password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email_confirmed = table.Column<bool>(type: "bit", nullable: true),
                    user_status = table.Column<int>(type: "int", nullable: false),
                    user_role = table.Column<int>(type: "int", nullable: false),
                    concurrency_stamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
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
                    banking_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    banking_provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    advertiser_id = table.Column<long>(type: "bigint", nullable: false),
                    camp_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    budget = table.Column<double>(type: "float", nullable: false),
                    balance = table.Column<double>(type: "float", nullable: false),
                    product_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tracking_params = table.Column<string>(type: "text", nullable: true),
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
                        name: "FK_Campaigns_Users_advertiser_id",
                        column: x => x.advertiser_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PublisherProfiles",
                columns: table => new
                {
                    pub_no = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    banking_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    banking_provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "PublisherSources",
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
                    table.PrimaryKey("PK_PublisherSources", x => x.pubs_no);
                    table.ForeignKey(
                        name: "FK_PublisherSources_Users_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "SubPurchases",
                columns: table => new
                {
                    subp_no = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    advertiser_id = table.Column<long>(type: "bigint", nullable: false),
                    sub_id = table.Column<long>(type: "bigint", nullable: false),
                    purchased_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    expired_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    current_price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubPurchases", x => x.subp_no);
                    table.ForeignKey(
                        name: "FK_SubPurchases_Subscriptions_sub_id",
                        column: x => x.sub_id,
                        principalTable: "Subscriptions",
                        principalColumn: "sub_id");
                    table.ForeignKey(
                        name: "FK_SubPurchases_Users_advertiser_id",
                        column: x => x.advertiser_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    offer_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    camp_id = table.Column<long>(type: "bigint", nullable: false),
                    pricing_model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    offer_note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
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
                name: "Images",
                columns: table => new
                {
                    img_no = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    offer_id = table.Column<long>(type: "bigint", nullable: true),
                    campaign_id = table.Column<long>(type: "bigint", nullable: true),
                    thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    img_url = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "IX_AdvertiserProfiles_advertiser_id",
                table: "AdvertiserProfiles",
                column: "advertiser_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_advertiser_id",
                table: "Campaigns",
                column: "advertiser_id");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_cate_id",
                table: "Campaigns",
                column: "cate_id");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryId",
                table: "Categories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_campaign_id",
                table: "Images",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "IX_Images_offer_id",
                table: "Images",
                column: "offer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_camp_id",
                table: "Offers",
                column: "camp_id");

            migrationBuilder.CreateIndex(
                name: "IX_PublisherProfiles_publisher_id",
                table: "PublisherProfiles",
                column: "publisher_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PublisherSources_publisher_id",
                table: "PublisherSources",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_SubPurchases_advertiser_id",
                table: "SubPurchases",
                column: "advertiser_id");

            migrationBuilder.CreateIndex(
                name: "IX_SubPurchases_sub_id",
                table: "SubPurchases",
                column: "sub_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertiserProfiles");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "PublisherProfiles");

            migrationBuilder.DropTable(
                name: "PublisherSources");

            migrationBuilder.DropTable(
                name: "SubPurchases");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
