using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Identity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RolesSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "50df0d64-b2ae-482a-98d1-bd187fbbbeda", "50df0d64-b2ae-482a-98d1-bd187fbbbeda", "Admin", "ADMIN" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50df0d64-b2ae-482a-98d1-bd187fbbbeda");
        }
    }
}
