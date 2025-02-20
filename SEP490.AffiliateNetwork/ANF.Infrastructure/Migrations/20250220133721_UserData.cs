using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ANF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "user_id", "address", "citizen_id", "date_of_birth", "user_email", "email_confirmed", "first_name", "last_name", "user_password", "phone_number", "user_role", "user_status" },
                values: new object[,]
                {
                    { 100L, null, "JS123456789", null, "john.smith@email.com", true, "John", "Smith", "hashed_password_1", "555-0123", 2, 1 },
                    { 101L, null, "SJ987654321", null, "sarah.j@email.com", true, "Sarah", "Johnson", "hashed_password_2", "555-0124", 1, 1 },
                    { 103L, null, null, null, "saffiliatenetwork@gmail.com", true, null, null, "superstrongpassword123@", null, 0, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 100L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 101L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "user_id",
                keyValue: 103L);
        }
    }
}
