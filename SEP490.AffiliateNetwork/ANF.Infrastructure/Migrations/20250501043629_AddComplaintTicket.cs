using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddComplaintTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComplaintType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComplaintTicket",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublisherCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdvertiserCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintTicket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplaintTicket_ComplaintType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ComplaintType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "ComplaintType",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1L, "Affiliate links are disguised or not properly disclosed to users.", "Misleading Affiliate Links" },
                    { 2L, "Affiliate sales or clicks are not being properly tracked or credited.", "Commission Tracking Error" },
                    { 3L, "Affiliate promotions contain inaccurate or exaggerated product claims.", "False Product Claims" },
                    { 4L, "Delayed or missing commission payments for confirmed sales.", "Late Commission Payment" },
                    { 5L, "Affiliate links being used without proper authorization or agreement.", "Unauthorized Link Usage" },
                    { 6L, "Referral data showing inconsistencies or errors in tracking system.", "Invalid Referral Data" },
                    { 7L, "Affiliate marketing practices violating program terms or regulations.", "Compliance Violation" },
                    { 8L, "Disagreement over applied commission rates or tier calculations.", "Commission Rate Dispute" },
                    { 9L, "Problems with affiliate cookie tracking duration or attribution.", "Cookie Duration Issue" },
                    { 10L, "Lack of or unclear communication about program changes or updates.", "Program Communication" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintTicket_TypeId",
                table: "ComplaintTicket",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComplaintTicket");

            migrationBuilder.DropTable(
                name: "ComplaintType");
        }
    }
}
