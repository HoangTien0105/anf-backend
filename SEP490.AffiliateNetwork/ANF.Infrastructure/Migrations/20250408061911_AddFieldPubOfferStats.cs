using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldPubOfferStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_publisher_offer_stats",
                table: "publisher_offer_stats");

            migrationBuilder.RenameTable(
                name: "publisher_offer_stats",
                newName: "PublisherOfferStats");

            migrationBuilder.AddColumn<decimal>(
                name: "revenue",
                table: "PublisherOfferStats",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublisherOfferStats",
                table: "PublisherOfferStats",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PublisherOfferStats",
                table: "PublisherOfferStats");

            migrationBuilder.DropColumn(
                name: "revenue",
                table: "PublisherOfferStats");

            migrationBuilder.RenameTable(
                name: "PublisherOfferStats",
                newName: "publisher_offer_stats");

            migrationBuilder.AddPrimaryKey(
                name: "PK_publisher_offer_stats",
                table: "publisher_offer_stats",
                column: "id");
        }
    }
}
