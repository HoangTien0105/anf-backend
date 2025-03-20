using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApprovedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublisherSources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrackingParam",
                table: "TrackingParam");

            migrationBuilder.RenameTable(
                name: "TrackingParam",
                newName: "TrackingParams");

            migrationBuilder.RenameColumn(
                name: "payment_status",
                table: "Transactions",
                newName: "status");

            migrationBuilder.AddColumn<string>(
                name: "reason",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "approved_date",
                table: "PublisherOffers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrackingParams",
                table: "TrackingParams",
                column: "param_id");

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

            migrationBuilder.CreateIndex(
                name: "IX_TrafficSources_publisher_id",
                table: "TrafficSources",
                column: "publisher_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrafficSources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrackingParams",
                table: "TrackingParams");

            migrationBuilder.DropColumn(
                name: "reason",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "approved_date",
                table: "PublisherOffers");

            migrationBuilder.RenameTable(
                name: "TrackingParams",
                newName: "TrackingParam");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Transactions",
                newName: "payment_status");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrackingParam",
                table: "TrackingParam",
                column: "param_id");

            migrationBuilder.CreateTable(
                name: "PublisherSources",
                columns: table => new
                {
                    pubs_no = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    publisher_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    soruce_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_PublisherSources_publisher_id",
                table: "PublisherSources",
                column: "publisher_id");
        }
    }
}
