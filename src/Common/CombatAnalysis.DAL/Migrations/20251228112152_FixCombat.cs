using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CombatAnalysis.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixCombat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BoosId",
                table: "Combat",
                newName: "BossId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BossId",
                table: "Combat",
                newName: "BoosId");
        }
    }
}
