using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "banking_no",
                table: "PublisherProfiles");

            migrationBuilder.DropColumn(
                name: "banking_provider",
                table: "PublisherProfiles");

            migrationBuilder.DropColumn(
                name: "banking_no",
                table: "AdvertiserProfiles");

            migrationBuilder.DropColumn(
                name: "banking_provider",
                table: "AdvertiserProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "banking_no",
                table: "PublisherProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "banking_provider",
                table: "PublisherProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "banking_no",
                table: "AdvertiserProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "banking_provider",
                table: "AdvertiserProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
