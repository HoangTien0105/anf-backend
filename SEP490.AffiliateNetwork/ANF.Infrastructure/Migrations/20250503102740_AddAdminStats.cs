using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminStats",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    total_user = table.Column<int>(type: "int", nullable: false),
                    total_campaign = table.Column<int>(type: "int", nullable: false),
                    total_rejected_campaign = table.Column<int>(type: "int", nullable: false),
                    total_approved_campaign = table.Column<int>(type: "int", nullable: false),
                    total_ticket = table.Column<int>(type: "int", nullable: false),
                    total_resolved_ticket = table.Column<int>(type: "int", nullable: false),
                    total_pending_ticket = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminStats", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminStats");
        }
    }
}
