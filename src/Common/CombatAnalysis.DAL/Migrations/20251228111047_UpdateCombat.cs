using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CombatAnalysis.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCombat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Combat");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Combat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Combat",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Combat",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
