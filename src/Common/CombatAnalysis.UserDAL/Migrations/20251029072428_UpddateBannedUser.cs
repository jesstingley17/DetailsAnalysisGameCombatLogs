using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CombatAnalysis.UserDAL.Migrations
{
    /// <inheritdoc />
    public partial class UpddateBannedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BannedCustomerId",
                table: "BannedUser",
                newName: "WhomBannedId");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "BannedUser",
                newName: "BannedUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WhomBannedId",
                table: "BannedUser",
                newName: "BannedCustomerId");

            migrationBuilder.RenameColumn(
                name: "BannedUserId",
                table: "BannedUser",
                newName: "AppUserId");
        }
    }
}
