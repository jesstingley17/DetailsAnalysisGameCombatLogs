using CombatAnalysis.DAL.Helpers;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CombatAnalysis.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Health = table.Column<long>(type: "bigint", nullable: false),
                    Difficult = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Combat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DungeonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DamageDone = table.Column<int>(type: "int", nullable: false),
                    HealDone = table.Column<int>(type: "int", nullable: false),
                    DamageTaken = table.Column<int>(type: "int", nullable: false),
                    EnergyRecovery = table.Column<int>(type: "int", nullable: false),
                    IsWin = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FinishDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsReady = table.Column<bool>(type: "bit", nullable: false),
                    BossId = table.Column<int>(type: "int", nullable: false),
                    CombatLogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CombatAura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuraCreatorType = table.Column<int>(type: "int", nullable: false),
                    AuraType = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    FinishTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Stacks = table.Column<int>(type: "int", nullable: false),
                    CombatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatAura", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CombatLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LogType = table.Column<int>(type: "int", nullable: false),
                    NumberReadyCombats = table.Column<int>(type: "int", nullable: false),
                    CombatsInQueue = table.Column<int>(type: "int", nullable: false),
                    IsReady = table.Column<bool>(type: "bit", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CombatPlayer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AverageItemLevel = table.Column<double>(type: "float", nullable: false),
                    ResourcesRecovery = table.Column<int>(type: "int", nullable: false),
                    DamageDone = table.Column<int>(type: "int", nullable: false),
                    HealDone = table.Column<int>(type: "int", nullable: false),
                    DamageTaken = table.Column<int>(type: "int", nullable: false),
                    CombatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatPlayer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CombatPlayerPosition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionX = table.Column<double>(type: "float", nullable: false),
                    PositionY = table.Column<double>(type: "float", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false),
                    CombatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatPlayerPosition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DamageDone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsTargetBoss = table.Column<bool>(type: "bit", nullable: false),
                    DamageType = table.Column<int>(type: "int", nullable: false),
                    IsPeriodicDamage = table.Column<bool>(type: "bit", nullable: false),
                    IsSingleTarget = table.Column<bool>(type: "bit", nullable: false),
                    IsPet = table.Column<bool>(type: "bit", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageDone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DamageDoneGeneral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    DamagePerSecond = table.Column<double>(type: "float", nullable: false),
                    CritNumber = table.Column<int>(type: "int", nullable: false),
                    MissNumber = table.Column<int>(type: "int", nullable: false),
                    CastNumber = table.Column<int>(type: "int", nullable: false),
                    MinValue = table.Column<int>(type: "int", nullable: false),
                    MaxValue = table.Column<int>(type: "int", nullable: false),
                    AverageValue = table.Column<double>(type: "float", nullable: false),
                    IsPet = table.Column<bool>(type: "bit", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageDoneGeneral", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DamageTaken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DamageTakenType = table.Column<int>(type: "int", nullable: false),
                    ActualValue = table.Column<int>(type: "int", nullable: false),
                    IsPeriodicDamage = table.Column<bool>(type: "bit", nullable: false),
                    Resisted = table.Column<int>(type: "int", nullable: false),
                    Absorbed = table.Column<int>(type: "int", nullable: false),
                    Blocked = table.Column<int>(type: "int", nullable: false),
                    RealDamage = table.Column<int>(type: "int", nullable: false),
                    Mitigated = table.Column<int>(type: "int", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageTaken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DamageTakenGeneral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    ActualValue = table.Column<int>(type: "int", nullable: false),
                    DamageTakenPerSecond = table.Column<double>(type: "float", nullable: false),
                    CritNumber = table.Column<int>(type: "int", nullable: false),
                    MissNumber = table.Column<int>(type: "int", nullable: false),
                    CastNumber = table.Column<int>(type: "int", nullable: false),
                    MinValue = table.Column<int>(type: "int", nullable: false),
                    MaxValue = table.Column<int>(type: "int", nullable: false),
                    AverageValue = table.Column<double>(type: "float", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageTakenGeneral", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HealDone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Overheal = table.Column<int>(type: "int", nullable: false),
                    IsCrit = table.Column<bool>(type: "bit", nullable: false),
                    IsAbsorbed = table.Column<bool>(type: "bit", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealDone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HealDoneGeneral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    HealPerSecond = table.Column<double>(type: "float", nullable: false),
                    CritNumber = table.Column<int>(type: "int", nullable: false),
                    CastNumber = table.Column<int>(type: "int", nullable: false),
                    MinValue = table.Column<int>(type: "int", nullable: false),
                    MaxValue = table.Column<int>(type: "int", nullable: false),
                    AverageValue = table.Column<double>(type: "float", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealDoneGeneral", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerDeath",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastHitSpell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastHitValue = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerDeath", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerParseInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    DamageEfficiency = table.Column<int>(type: "int", nullable: false),
                    HealEfficiency = table.Column<int>(type: "int", nullable: false),
                    BossId = table.Column<int>(type: "int", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerParseInfo", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "ResourceRecovery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceRecovery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceRecoveryGeneral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Spell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    ResourcePerSecond = table.Column<double>(type: "float", nullable: false),
                    CastNumber = table.Column<int>(type: "int", nullable: false),
                    MinValue = table.Column<int>(type: "int", nullable: false),
                    MaxValue = table.Column<int>(type: "int", nullable: false),
                    AverageValue = table.Column<double>(type: "float", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceRecoveryGeneral", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecializationScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecId = table.Column<int>(type: "int", nullable: false),
                    BossId = table.Column<int>(type: "int", nullable: false),
                    Damage = table.Column<int>(type: "int", nullable: false),
                    Heal = table.Column<int>(type: "int", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecializationScore", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Boss",
                columns: new[] { "Id", "Difficult", "GameId", "Health", "Name", "Size" },
                values: new object[,]
                {
                    { 1, 3, 1395, 130841100L, "Каменные стражи", 10 },
                    { 2, 5, 1395, 235513980L, "Каменные стражи", 10 },
                    { 3, 3, 1390, 152647950L, "Фэн Проклятый", 10 },
                    { 4, 5, 1390, 209345760L, "Фэн Проклятый", 10 },
                    { 5, 3, 1434, 117756990L, "Душелов Гара'джал", 10 },
                    { 6, 5, 1434, 179252307L, "Душелов Гара'джал", 10 },
                    { 7, 3, 1436, 174454800L, "Призрачные короли", 10 },
                    { 8, 5, 1436, 261682200L, "Призрачные короли", 10 },
                    { 9, 3, 1500, 294392475L, "Элегон", 10 },
                    { 10, 5, 1500, 339750723L, "Элегон", 10 },
                    { 11, 3, 1407, 314018640L, "Воля императора", 10 },
                    { 12, 5, 1407, 471027960L, "Воля императора", 10 },
                    { 13, 3, 1409, 213968815L, "Вечные защитники", 10 },
                    { 14, 5, 1409, 344082093L, "Вечные защитники", 10 },
                    { 15, 3, 1505, 174454800L, "Цулон", 10 },
                    { 16, 5, 1505, 279127680L, "Цулон", 10 },
                    { 17, 3, 1506, 138168195L, "Лэй Ши", 10 },
                    { 18, 5, 1506, 301457900L, "Лэй Ши", 10 },
                    { 19, 3, 1431, 184704020L, "Ша Страха", 10 },
                    { 20, 5, 1431, 544037304L, "Ша Страха", 10 },
                    { 21, 3, 1507, 174454800L, "Императорский визирь Зор'лок", 10 },
                    { 22, 5, 1507, 218068500L, "Императорский визирь Зор'лок", 10 },
                    { 23, 3, 1504, 150467265L, "Повелитель клинков Та'як", 10 },
                    { 24, 5, 1504, 196261650L, "Повелитель клинков Та'як", 10 },
                    { 25, 3, 1463, 218068500L, "Гаралон", 10 },
                    { 26, 5, 1463, 290759446L, "Гаралон", 10 },
                    { 27, 3, 1498, 270404940L, "Повелитель ветров Мел'джарак", 10 },
                    { 28, 5, 1498, 588784950L, "Повелитель ветров Мел'джарак", 10 },
                    { 29, 3, 1499, 218068500L, "Ваятель янтаря Ун'сок", 10 },
                    { 30, 5, 1499, 340186860L, "Ваятель янтаря Ун'сок", 10 },
                    { 31, 3, 1501, 196261650L, "Великая императрица Шек'зир", 10 },
                    { 32, 5, 1501, 307476585L, "Великая императрица Шек'зир", 10 },
                    { 33, 3, 1577, 207601212L, "Джин'рок Разрушитель", 10 },
                    { 34, 5, 1577, 317507736L, "Джин'рок Разрушитель", 10 },
                    { 35, 3, 1575, 357632340L, "Хорридон", 10 },
                    { 36, 5, 1575, 654205500L, "Хорридон", 10 },
                    { 37, 3, 1570, 299538888L, "Совет старейшин", 10 },
                    { 38, 5, 1570, 470330152L, "Совет старейшин", 10 },
                    { 39, 3, 1565, 179999841L, "Тортос", 10 },
                    { 40, 5, 1565, 319999818L, "Тортос", 10 },
                    { 41, 3, 1578, 263317712L, "Мегера", 10 },
                    { 42, 5, 1578, 342297774L, "Мегера", 10 },
                    { 43, 3, 1573, 244236720L, "Цзи-Кунь", 10 },
                    { 44, 5, 1573, 366355080L, "Цзи-Кунь", 10 },
                    { 45, 3, 1572, 261682200L, "Дуруму Позабытый", 10 },
                    { 46, 5, 1572, 392523300L, "Дуруму Позабытый", 10 },
                    { 47, 3, 1574, 218068500L, "Изначалий", 10 },
                    { 48, 5, 1574, 258193104L, "Изначалий", 10 },
                    { 49, 3, 1576, 80999797L, "Темный Анимус", 10 },
                    { 50, 5, 1576, 288000023L, "Темный Анимус", 10 },
                    { 51, 3, 1559, 119937675L, "Кон Железный", 10 },
                    { 52, 5, 1559, 155700909L, "Кон Железный", 10 },
                    { 53, 3, 1560, 219812670L, "Небесные сестры", 10 },
                    { 54, 5, 1560, 628036200L, "Небесные сестры", 10 },
                    { 55, 3, 1579, 329283435L, "Лэй Шэнь", 10 },
                    { 56, 5, 1579, 580498347L, "Лэй Шэнь", 10 }
                });

            MigrationHelper.CreateTableTypes(migrationBuilder);
            MigrationHelper.CreateProcedures(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Boss");

            migrationBuilder.DropTable(
                name: "Combat");

            migrationBuilder.DropTable(
                name: "CombatAura");

            migrationBuilder.DropTable(
                name: "CombatLog");

            migrationBuilder.DropTable(
                name: "CombatPlayer");

            migrationBuilder.DropTable(
                name: "CombatPlayerPosition");

            migrationBuilder.DropTable(
                name: "DamageDone");

            migrationBuilder.DropTable(
                name: "DamageDoneGeneral");

            migrationBuilder.DropTable(
                name: "DamageTaken");

            migrationBuilder.DropTable(
                name: "DamageTakenGeneral");

            migrationBuilder.DropTable(
                name: "HealDone");

            migrationBuilder.DropTable(
                name: "HealDoneGeneral");

            migrationBuilder.DropTable(
                name: "PlayerDeath");

            migrationBuilder.DropTable(
                name: "PlayerParseInfo");

            migrationBuilder.DropTable(
                name: "PlayerStats");

            migrationBuilder.DropTable(
                name: "ResourceRecovery");

            migrationBuilder.DropTable(
                name: "ResourceRecoveryGeneral");

            migrationBuilder.DropTable(
                name: "SpecializationScore");

            MigrationHelper.DropTableTypes(migrationBuilder);
            MigrationHelper.DropProcedures(migrationBuilder);
        }
    }
}
