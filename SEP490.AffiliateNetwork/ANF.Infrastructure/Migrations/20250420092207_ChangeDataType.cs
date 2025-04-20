using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Policies",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Policies",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "offer_description",
                table: "Offers",
                type: "ntext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Campaigns",
                type: "ntext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "Policies");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Policies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "offer_description",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ntext");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Campaigns",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ntext");
        }
    }
}
