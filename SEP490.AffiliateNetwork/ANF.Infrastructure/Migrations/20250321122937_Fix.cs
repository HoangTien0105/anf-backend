using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrackingEvents_publisher_code_offer_id",
                table: "TrackingEvents");

            migrationBuilder.DropColumn(
                name: "offer_id",
                table: "TrackingValidations");

            migrationBuilder.DropColumn(
                name: "publisher_id",
                table: "PostbackData");

            migrationBuilder.DropColumn(
                name: "offer_id",
                table: "FraudDetections");

            migrationBuilder.DropColumn(
                name: "publisher_id",
                table: "FraudDetections");

            migrationBuilder.AlterColumn<decimal>(
                name: "revenue",
                table: "TrackingValidations",
                type: "decimal(10,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "conversion_status",
                table: "TrackingValidations",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "publisher_code",
                table: "TrackingEvents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "PostbackData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "publisher_code",
                table: "PostbackData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "publisher_code",
                table: "FraudDetections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BatchPayments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    transaction_id = table.Column<long>(type: "bigint", nullable: false),
                    from_account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(10,0)", nullable: false),
                    beneficiary_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    beneficiary_account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    beneficiary_bank_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    beneficiary_bank_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchPayments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Policies",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    header = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policies", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchPayments");

            migrationBuilder.DropTable(
                name: "Policies");

            migrationBuilder.DropColumn(
                name: "publisher_code",
                table: "PostbackData");

            migrationBuilder.DropColumn(
                name: "publisher_code",
                table: "FraudDetections");

            migrationBuilder.AlterColumn<double>(
                name: "revenue",
                table: "TrackingValidations",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "conversion_status",
                table: "TrackingValidations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "offer_id",
                table: "TrackingValidations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "publisher_code",
                table: "TrackingEvents",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "PostbackData",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "publisher_id",
                table: "PostbackData",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "offer_id",
                table: "FraudDetections",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "publisher_id",
                table: "FraudDetections",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingEvents_publisher_code_offer_id",
                table: "TrackingEvents",
                columns: new[] { "publisher_code", "offer_id" },
                unique: true);
        }
    }
}
