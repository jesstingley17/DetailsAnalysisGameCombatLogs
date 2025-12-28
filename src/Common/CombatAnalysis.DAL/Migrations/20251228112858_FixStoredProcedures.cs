using CombatAnalysis.DAL.Helpers;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CombatAnalysis.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MigrationHelper.CreateProcedures(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            MigrationHelper.DropProcedures(migrationBuilder);
        }
    }
}
