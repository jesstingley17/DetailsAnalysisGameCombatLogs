using CombatAnalysis.DAL.Helpers;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CombatAnalysis.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Faction = table.Column<int>(type: "int", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Agility = table.Column<int>(type: "int", nullable: false),
                    Intelligence = table.Column<int>(type: "int", nullable: false),
                    Stamina = table.Column<int>(type: "int", nullable: false),
                    Spirit = table.Column<int>(type: "int", nullable: false),
                    Dodge = table.Column<int>(type: "int", nullable: false),
                    Parry = table.Column<int>(type: "int", nullable: false),
                    Crit = table.Column<int>(type: "int", nullable: false),
                    Haste = table.Column<int>(type: "int", nullable: false),
                    Hit = table.Column<int>(type: "int", nullable: false),
                    Expertise = table.Column<int>(type: "int", nullable: false),
                    Armor = table.Column<int>(type: "int", nullable: false),
                    Talents = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerStats", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Boss",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1390, "Фэн Проклятый" },
                    { 1395, "Каменные стражи" },
                    { 1407, "Воля императора" },
                    { 1409, "Вечные защитники" },
                    { 1431, "Ша Страха" },
                    { 1434, "Душелов Гара'джал" },
                    { 1436, "Призрачные короли" },
                    { 1463, "Гаралон" },
                    { 1498, "Повелитель ветров Мел'джарак" },
                    { 1499, "Ваятель янтаря Ун'сок" },
                    { 1500, "Элегон" },
                    { 1501, "Великая императрица Шек'зир" },
                    { 1504, "Повелитель клинков Та'як" },
                    { 1505, "Цулон" },
                    { 1506, "Лэй Ши" },
                    { 1507, "Императорский визирь Зор'лок" }
                });

            MigrationHelper.CreateProcedures(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Boss");

            migrationBuilder.DropTable(
                name: "PlayerStats");

            MigrationHelper.DropProcedures(migrationBuilder);
        }
    }
}
