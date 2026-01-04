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
                name: "BestSpecializationScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DamageDone = table.Column<int>(type: "int", nullable: false),
                    HealDone = table.Column<int>(type: "int", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SpecializationId = table.Column<int>(type: "int", nullable: false),
                    BossId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BestSpecializationScore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Boss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
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
                    DungeonName = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    DamageDone = table.Column<int>(type: "int", nullable: false),
                    HealDone = table.Column<int>(type: "int", nullable: false),
                    DamageTaken = table.Column<int>(type: "int", nullable: false),
                    ResourcesRecovery = table.Column<int>(type: "int", nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    Target = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
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
                    AverageItemLevel = table.Column<double>(type: "float", nullable: false),
                    ResourcesRecovery = table.Column<int>(type: "int", nullable: false),
                    DamageDone = table.Column<int>(type: "int", nullable: false),
                    HealDone = table.Column<int>(type: "int", nullable: false),
                    DamageTaken = table.Column<int>(type: "int", nullable: false),
                    CombatId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    Target = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
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
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
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
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    Target = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
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
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
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
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
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
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
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
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GameId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    Faction = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerDeath",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    LastHitSpell = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    LastHitValue = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerDeath", x => x.Id);
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
                    Talents = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
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
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
                    Target = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
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
                    GameSpellId = table.Column<int>(type: "int", nullable: false),
                    Spell = table.Column<string>(type: "nvarchar(126)", maxLength: 126, nullable: false),
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
                name: "Specialization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecializationSpellsId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecializationScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DamageScore = table.Column<double>(type: "float", nullable: false),
                    DamageDone = table.Column<int>(type: "int", nullable: false),
                    HealScore = table.Column<double>(type: "float", nullable: false),
                    HealDone = table.Column<int>(type: "int", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SpecializationId = table.Column<int>(type: "int", nullable: false),
                    CombatPlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecializationScore", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BestSpecializationScore",
                columns: new[] { "Id", "BossId", "DamageDone", "HealDone", "SpecializationId", "Updated" },
                values: new object[,]
                {
                    { 1, 1, 0, 0, 1, null },
                    { 2, 1, 0, 0, 2, null },
                    { 3, 1, 0, 0, 3, null },
                    { 4, 1, 0, 0, 4, null },
                    { 5, 1, 0, 0, 5, null },
                    { 6, 1, 0, 0, 6, null },
                    { 7, 1, 0, 0, 7, null },
                    { 8, 1, 0, 0, 8, null },
                    { 9, 1, 0, 0, 9, null },
                    { 10, 1, 0, 0, 10, null },
                    { 11, 2, 0, 0, 1, null },
                    { 12, 2, 0, 0, 2, null },
                    { 13, 2, 0, 0, 3, null },
                    { 14, 2, 0, 0, 4, null },
                    { 15, 2, 0, 0, 5, null },
                    { 16, 2, 0, 0, 6, null },
                    { 17, 2, 0, 0, 7, null },
                    { 18, 2, 0, 0, 8, null },
                    { 19, 2, 0, 0, 9, null },
                    { 20, 2, 0, 0, 10, null },
                    { 21, 3, 0, 0, 1, null },
                    { 22, 3, 0, 0, 2, null },
                    { 23, 3, 0, 0, 3, null },
                    { 24, 3, 0, 0, 4, null },
                    { 25, 3, 0, 0, 5, null },
                    { 26, 3, 0, 0, 6, null },
                    { 27, 3, 0, 0, 7, null },
                    { 28, 3, 0, 0, 8, null },
                    { 29, 3, 0, 0, 9, null },
                    { 30, 3, 0, 0, 10, null },
                    { 31, 4, 0, 0, 1, null },
                    { 32, 4, 0, 0, 2, null },
                    { 33, 4, 0, 0, 3, null },
                    { 34, 4, 0, 0, 4, null },
                    { 35, 4, 0, 0, 5, null },
                    { 36, 4, 0, 0, 6, null },
                    { 37, 4, 0, 0, 7, null },
                    { 38, 4, 0, 0, 8, null },
                    { 39, 4, 0, 0, 9, null },
                    { 40, 4, 0, 0, 10, null },
                    { 41, 5, 0, 0, 1, null },
                    { 42, 5, 0, 0, 2, null },
                    { 43, 5, 0, 0, 3, null },
                    { 44, 5, 0, 0, 4, null },
                    { 45, 5, 0, 0, 5, null },
                    { 46, 5, 0, 0, 6, null },
                    { 47, 5, 0, 0, 7, null },
                    { 48, 5, 0, 0, 8, null },
                    { 49, 5, 0, 0, 9, null },
                    { 50, 5, 0, 0, 10, null },
                    { 51, 6, 0, 0, 1, null },
                    { 52, 6, 0, 0, 2, null },
                    { 53, 6, 0, 0, 3, null },
                    { 54, 6, 0, 0, 4, null },
                    { 55, 6, 0, 0, 5, null },
                    { 56, 6, 0, 0, 6, null },
                    { 57, 6, 0, 0, 7, null },
                    { 58, 6, 0, 0, 8, null },
                    { 59, 6, 0, 0, 9, null },
                    { 60, 6, 0, 0, 10, null },
                    { 61, 7, 0, 0, 1, null },
                    { 62, 7, 0, 0, 2, null },
                    { 63, 7, 0, 0, 3, null },
                    { 64, 7, 0, 0, 4, null },
                    { 65, 7, 0, 0, 5, null },
                    { 66, 7, 0, 0, 6, null },
                    { 67, 7, 0, 0, 7, null },
                    { 68, 7, 0, 0, 8, null },
                    { 69, 7, 0, 0, 9, null },
                    { 70, 7, 0, 0, 10, null },
                    { 71, 8, 0, 0, 1, null },
                    { 72, 8, 0, 0, 2, null },
                    { 73, 8, 0, 0, 3, null },
                    { 74, 8, 0, 0, 4, null },
                    { 75, 8, 0, 0, 5, null },
                    { 76, 8, 0, 0, 6, null },
                    { 77, 8, 0, 0, 7, null },
                    { 78, 8, 0, 0, 8, null },
                    { 79, 8, 0, 0, 9, null },
                    { 80, 8, 0, 0, 10, null },
                    { 81, 9, 0, 0, 1, null },
                    { 82, 9, 0, 0, 2, null },
                    { 83, 9, 0, 0, 3, null },
                    { 84, 9, 0, 0, 4, null },
                    { 85, 9, 0, 0, 5, null },
                    { 86, 9, 0, 0, 6, null },
                    { 87, 9, 0, 0, 7, null },
                    { 88, 9, 0, 0, 8, null },
                    { 89, 9, 0, 0, 9, null },
                    { 90, 9, 0, 0, 10, null },
                    { 91, 10, 0, 0, 1, null },
                    { 92, 10, 0, 0, 2, null },
                    { 93, 10, 0, 0, 3, null },
                    { 94, 10, 0, 0, 4, null },
                    { 95, 10, 0, 0, 5, null },
                    { 96, 10, 0, 0, 6, null },
                    { 97, 10, 0, 0, 7, null },
                    { 98, 10, 0, 0, 8, null },
                    { 99, 10, 0, 0, 9, null },
                    { 100, 10, 0, 0, 10, null },
                    { 101, 11, 0, 0, 1, null },
                    { 102, 11, 0, 0, 2, null },
                    { 103, 11, 0, 0, 3, null },
                    { 104, 11, 0, 0, 4, null },
                    { 105, 11, 0, 0, 5, null },
                    { 106, 11, 0, 0, 6, null },
                    { 107, 11, 0, 0, 7, null },
                    { 108, 11, 0, 0, 8, null },
                    { 109, 11, 0, 0, 9, null },
                    { 110, 11, 0, 0, 10, null },
                    { 111, 12, 0, 0, 1, null },
                    { 112, 12, 0, 0, 2, null },
                    { 113, 12, 0, 0, 3, null },
                    { 114, 12, 0, 0, 4, null },
                    { 115, 12, 0, 0, 5, null },
                    { 116, 12, 0, 0, 6, null },
                    { 117, 12, 0, 0, 7, null },
                    { 118, 12, 0, 0, 8, null },
                    { 119, 12, 0, 0, 9, null },
                    { 120, 12, 0, 0, 10, null },
                    { 121, 13, 0, 0, 1, null },
                    { 122, 13, 0, 0, 2, null },
                    { 123, 13, 0, 0, 3, null },
                    { 124, 13, 0, 0, 4, null },
                    { 125, 13, 0, 0, 5, null },
                    { 126, 13, 0, 0, 6, null },
                    { 127, 13, 0, 0, 7, null },
                    { 128, 13, 0, 0, 8, null },
                    { 129, 13, 0, 0, 9, null },
                    { 130, 13, 0, 0, 10, null },
                    { 131, 14, 0, 0, 1, null },
                    { 132, 14, 0, 0, 2, null },
                    { 133, 14, 0, 0, 3, null },
                    { 134, 14, 0, 0, 4, null },
                    { 135, 14, 0, 0, 5, null },
                    { 136, 14, 0, 0, 6, null },
                    { 137, 14, 0, 0, 7, null },
                    { 138, 14, 0, 0, 8, null },
                    { 139, 14, 0, 0, 9, null },
                    { 140, 14, 0, 0, 10, null },
                    { 141, 15, 0, 0, 1, null },
                    { 142, 15, 0, 0, 2, null },
                    { 143, 15, 0, 0, 3, null },
                    { 144, 15, 0, 0, 4, null },
                    { 145, 15, 0, 0, 5, null },
                    { 146, 15, 0, 0, 6, null },
                    { 147, 15, 0, 0, 7, null },
                    { 148, 15, 0, 0, 8, null },
                    { 149, 15, 0, 0, 9, null },
                    { 150, 15, 0, 0, 10, null },
                    { 151, 16, 0, 0, 1, null },
                    { 152, 16, 0, 0, 2, null },
                    { 153, 16, 0, 0, 3, null },
                    { 154, 16, 0, 0, 4, null },
                    { 155, 16, 0, 0, 5, null },
                    { 156, 16, 0, 0, 6, null },
                    { 157, 16, 0, 0, 7, null },
                    { 158, 16, 0, 0, 8, null },
                    { 159, 16, 0, 0, 9, null },
                    { 160, 16, 0, 0, 10, null },
                    { 161, 17, 0, 0, 1, null },
                    { 162, 17, 0, 0, 2, null },
                    { 163, 17, 0, 0, 3, null },
                    { 164, 17, 0, 0, 4, null },
                    { 165, 17, 0, 0, 5, null },
                    { 166, 17, 0, 0, 6, null },
                    { 167, 17, 0, 0, 7, null },
                    { 168, 17, 0, 0, 8, null },
                    { 169, 17, 0, 0, 9, null },
                    { 170, 17, 0, 0, 10, null },
                    { 171, 18, 0, 0, 1, null },
                    { 172, 18, 0, 0, 2, null },
                    { 173, 18, 0, 0, 3, null },
                    { 174, 18, 0, 0, 4, null },
                    { 175, 18, 0, 0, 5, null },
                    { 176, 18, 0, 0, 6, null },
                    { 177, 18, 0, 0, 7, null },
                    { 178, 18, 0, 0, 8, null },
                    { 179, 18, 0, 0, 9, null },
                    { 180, 18, 0, 0, 10, null },
                    { 181, 19, 0, 0, 1, null },
                    { 182, 19, 0, 0, 2, null },
                    { 183, 19, 0, 0, 3, null },
                    { 184, 19, 0, 0, 4, null },
                    { 185, 19, 0, 0, 5, null },
                    { 186, 19, 0, 0, 6, null },
                    { 187, 19, 0, 0, 7, null },
                    { 188, 19, 0, 0, 8, null },
                    { 189, 19, 0, 0, 9, null },
                    { 190, 19, 0, 0, 10, null },
                    { 191, 20, 0, 0, 1, null },
                    { 192, 20, 0, 0, 2, null },
                    { 193, 20, 0, 0, 3, null },
                    { 194, 20, 0, 0, 4, null },
                    { 195, 20, 0, 0, 5, null },
                    { 196, 20, 0, 0, 6, null },
                    { 197, 20, 0, 0, 7, null },
                    { 198, 20, 0, 0, 8, null },
                    { 199, 20, 0, 0, 9, null },
                    { 200, 20, 0, 0, 10, null },
                    { 201, 21, 0, 0, 1, null },
                    { 202, 21, 0, 0, 2, null },
                    { 203, 21, 0, 0, 3, null },
                    { 204, 21, 0, 0, 4, null },
                    { 205, 21, 0, 0, 5, null },
                    { 206, 21, 0, 0, 6, null },
                    { 207, 21, 0, 0, 7, null },
                    { 208, 21, 0, 0, 8, null },
                    { 209, 21, 0, 0, 9, null },
                    { 210, 21, 0, 0, 10, null },
                    { 211, 22, 0, 0, 1, null },
                    { 212, 22, 0, 0, 2, null },
                    { 213, 22, 0, 0, 3, null },
                    { 214, 22, 0, 0, 4, null },
                    { 215, 22, 0, 0, 5, null },
                    { 216, 22, 0, 0, 6, null },
                    { 217, 22, 0, 0, 7, null },
                    { 218, 22, 0, 0, 8, null },
                    { 219, 22, 0, 0, 9, null },
                    { 220, 22, 0, 0, 10, null },
                    { 221, 23, 0, 0, 1, null },
                    { 222, 23, 0, 0, 2, null },
                    { 223, 23, 0, 0, 3, null },
                    { 224, 23, 0, 0, 4, null },
                    { 225, 23, 0, 0, 5, null },
                    { 226, 23, 0, 0, 6, null },
                    { 227, 23, 0, 0, 7, null },
                    { 228, 23, 0, 0, 8, null },
                    { 229, 23, 0, 0, 9, null },
                    { 230, 23, 0, 0, 10, null },
                    { 231, 24, 0, 0, 1, null },
                    { 232, 24, 0, 0, 2, null },
                    { 233, 24, 0, 0, 3, null },
                    { 234, 24, 0, 0, 4, null },
                    { 235, 24, 0, 0, 5, null },
                    { 236, 24, 0, 0, 6, null },
                    { 237, 24, 0, 0, 7, null },
                    { 238, 24, 0, 0, 8, null },
                    { 239, 24, 0, 0, 9, null },
                    { 240, 24, 0, 0, 10, null },
                    { 241, 25, 0, 0, 1, null },
                    { 242, 25, 0, 0, 2, null },
                    { 243, 25, 0, 0, 3, null },
                    { 244, 25, 0, 0, 4, null },
                    { 245, 25, 0, 0, 5, null },
                    { 246, 25, 0, 0, 6, null },
                    { 247, 25, 0, 0, 7, null },
                    { 248, 25, 0, 0, 8, null },
                    { 249, 25, 0, 0, 9, null },
                    { 250, 25, 0, 0, 10, null },
                    { 251, 26, 0, 0, 1, null },
                    { 252, 26, 0, 0, 2, null },
                    { 253, 26, 0, 0, 3, null },
                    { 254, 26, 0, 0, 4, null },
                    { 255, 26, 0, 0, 5, null },
                    { 256, 26, 0, 0, 6, null },
                    { 257, 26, 0, 0, 7, null },
                    { 258, 26, 0, 0, 8, null },
                    { 259, 26, 0, 0, 9, null },
                    { 260, 26, 0, 0, 10, null },
                    { 261, 27, 0, 0, 1, null },
                    { 262, 27, 0, 0, 2, null },
                    { 263, 27, 0, 0, 3, null },
                    { 264, 27, 0, 0, 4, null },
                    { 265, 27, 0, 0, 5, null },
                    { 266, 27, 0, 0, 6, null },
                    { 267, 27, 0, 0, 7, null },
                    { 268, 27, 0, 0, 8, null },
                    { 269, 27, 0, 0, 9, null },
                    { 270, 27, 0, 0, 10, null },
                    { 271, 28, 0, 0, 1, null },
                    { 272, 28, 0, 0, 2, null },
                    { 273, 28, 0, 0, 3, null },
                    { 274, 28, 0, 0, 4, null },
                    { 275, 28, 0, 0, 5, null },
                    { 276, 28, 0, 0, 6, null },
                    { 277, 28, 0, 0, 7, null },
                    { 278, 28, 0, 0, 8, null },
                    { 279, 28, 0, 0, 9, null },
                    { 280, 28, 0, 0, 10, null },
                    { 281, 29, 0, 0, 1, null },
                    { 282, 29, 0, 0, 2, null },
                    { 283, 29, 0, 0, 3, null },
                    { 284, 29, 0, 0, 4, null },
                    { 285, 29, 0, 0, 5, null },
                    { 286, 29, 0, 0, 6, null },
                    { 287, 29, 0, 0, 7, null },
                    { 288, 29, 0, 0, 8, null },
                    { 289, 29, 0, 0, 9, null },
                    { 290, 29, 0, 0, 10, null },
                    { 291, 30, 0, 0, 1, null },
                    { 292, 30, 0, 0, 2, null },
                    { 293, 30, 0, 0, 3, null },
                    { 294, 30, 0, 0, 4, null },
                    { 295, 30, 0, 0, 5, null },
                    { 296, 30, 0, 0, 6, null },
                    { 297, 30, 0, 0, 7, null },
                    { 298, 30, 0, 0, 8, null },
                    { 299, 30, 0, 0, 9, null },
                    { 300, 30, 0, 0, 10, null },
                    { 301, 31, 0, 0, 1, null },
                    { 302, 31, 0, 0, 2, null },
                    { 303, 31, 0, 0, 3, null },
                    { 304, 31, 0, 0, 4, null },
                    { 305, 31, 0, 0, 5, null },
                    { 306, 31, 0, 0, 6, null },
                    { 307, 31, 0, 0, 7, null },
                    { 308, 31, 0, 0, 8, null },
                    { 309, 31, 0, 0, 9, null },
                    { 310, 31, 0, 0, 10, null },
                    { 311, 32, 0, 0, 1, null },
                    { 312, 32, 0, 0, 2, null },
                    { 313, 32, 0, 0, 3, null },
                    { 314, 32, 0, 0, 4, null },
                    { 315, 32, 0, 0, 5, null },
                    { 316, 32, 0, 0, 6, null },
                    { 317, 32, 0, 0, 7, null },
                    { 318, 32, 0, 0, 8, null },
                    { 319, 32, 0, 0, 9, null },
                    { 320, 32, 0, 0, 10, null },
                    { 321, 33, 0, 0, 1, null },
                    { 322, 33, 0, 0, 2, null },
                    { 323, 33, 0, 0, 3, null },
                    { 324, 33, 0, 0, 4, null },
                    { 325, 33, 0, 0, 5, null },
                    { 326, 33, 0, 0, 6, null },
                    { 327, 33, 0, 0, 7, null },
                    { 328, 33, 0, 0, 8, null },
                    { 329, 33, 0, 0, 9, null },
                    { 330, 33, 0, 0, 10, null },
                    { 331, 34, 0, 0, 1, null },
                    { 332, 34, 0, 0, 2, null },
                    { 333, 34, 0, 0, 3, null },
                    { 334, 34, 0, 0, 4, null },
                    { 335, 34, 0, 0, 5, null },
                    { 336, 34, 0, 0, 6, null },
                    { 337, 34, 0, 0, 7, null },
                    { 338, 34, 0, 0, 8, null },
                    { 339, 34, 0, 0, 9, null },
                    { 340, 34, 0, 0, 10, null },
                    { 341, 35, 0, 0, 1, null },
                    { 342, 35, 0, 0, 2, null },
                    { 343, 35, 0, 0, 3, null },
                    { 344, 35, 0, 0, 4, null },
                    { 345, 35, 0, 0, 5, null },
                    { 346, 35, 0, 0, 6, null },
                    { 347, 35, 0, 0, 7, null },
                    { 348, 35, 0, 0, 8, null },
                    { 349, 35, 0, 0, 9, null },
                    { 350, 35, 0, 0, 10, null },
                    { 351, 36, 0, 0, 1, null },
                    { 352, 36, 0, 0, 2, null },
                    { 353, 36, 0, 0, 3, null },
                    { 354, 36, 0, 0, 4, null },
                    { 355, 36, 0, 0, 5, null },
                    { 356, 36, 0, 0, 6, null },
                    { 357, 36, 0, 0, 7, null },
                    { 358, 36, 0, 0, 8, null },
                    { 359, 36, 0, 0, 9, null },
                    { 360, 36, 0, 0, 10, null },
                    { 361, 37, 0, 0, 1, null },
                    { 362, 37, 0, 0, 2, null },
                    { 363, 37, 0, 0, 3, null },
                    { 364, 37, 0, 0, 4, null },
                    { 365, 37, 0, 0, 5, null },
                    { 366, 37, 0, 0, 6, null },
                    { 367, 37, 0, 0, 7, null },
                    { 368, 37, 0, 0, 8, null },
                    { 369, 37, 0, 0, 9, null },
                    { 370, 37, 0, 0, 10, null },
                    { 371, 38, 0, 0, 1, null },
                    { 372, 38, 0, 0, 2, null },
                    { 373, 38, 0, 0, 3, null },
                    { 374, 38, 0, 0, 4, null },
                    { 375, 38, 0, 0, 5, null },
                    { 376, 38, 0, 0, 6, null },
                    { 377, 38, 0, 0, 7, null },
                    { 378, 38, 0, 0, 8, null },
                    { 379, 38, 0, 0, 9, null },
                    { 380, 38, 0, 0, 10, null },
                    { 381, 39, 0, 0, 1, null },
                    { 382, 39, 0, 0, 2, null },
                    { 383, 39, 0, 0, 3, null },
                    { 384, 39, 0, 0, 4, null },
                    { 385, 39, 0, 0, 5, null },
                    { 386, 39, 0, 0, 6, null },
                    { 387, 39, 0, 0, 7, null },
                    { 388, 39, 0, 0, 8, null },
                    { 389, 39, 0, 0, 9, null },
                    { 390, 39, 0, 0, 10, null },
                    { 391, 40, 0, 0, 1, null },
                    { 392, 40, 0, 0, 2, null },
                    { 393, 40, 0, 0, 3, null },
                    { 394, 40, 0, 0, 4, null },
                    { 395, 40, 0, 0, 5, null },
                    { 396, 40, 0, 0, 6, null },
                    { 397, 40, 0, 0, 7, null },
                    { 398, 40, 0, 0, 8, null },
                    { 399, 40, 0, 0, 9, null },
                    { 400, 40, 0, 0, 10, null },
                    { 401, 41, 0, 0, 1, null },
                    { 402, 41, 0, 0, 2, null },
                    { 403, 41, 0, 0, 3, null },
                    { 404, 41, 0, 0, 4, null },
                    { 405, 41, 0, 0, 5, null },
                    { 406, 41, 0, 0, 6, null },
                    { 407, 41, 0, 0, 7, null },
                    { 408, 41, 0, 0, 8, null },
                    { 409, 41, 0, 0, 9, null },
                    { 410, 41, 0, 0, 10, null },
                    { 411, 42, 0, 0, 1, null },
                    { 412, 42, 0, 0, 2, null },
                    { 413, 42, 0, 0, 3, null },
                    { 414, 42, 0, 0, 4, null },
                    { 415, 42, 0, 0, 5, null },
                    { 416, 42, 0, 0, 6, null },
                    { 417, 42, 0, 0, 7, null },
                    { 418, 42, 0, 0, 8, null },
                    { 419, 42, 0, 0, 9, null },
                    { 420, 42, 0, 0, 10, null },
                    { 421, 43, 0, 0, 1, null },
                    { 422, 43, 0, 0, 2, null },
                    { 423, 43, 0, 0, 3, null },
                    { 424, 43, 0, 0, 4, null },
                    { 425, 43, 0, 0, 5, null },
                    { 426, 43, 0, 0, 6, null },
                    { 427, 43, 0, 0, 7, null },
                    { 428, 43, 0, 0, 8, null },
                    { 429, 43, 0, 0, 9, null },
                    { 430, 43, 0, 0, 10, null },
                    { 431, 44, 0, 0, 1, null },
                    { 432, 44, 0, 0, 2, null },
                    { 433, 44, 0, 0, 3, null },
                    { 434, 44, 0, 0, 4, null },
                    { 435, 44, 0, 0, 5, null },
                    { 436, 44, 0, 0, 6, null },
                    { 437, 44, 0, 0, 7, null },
                    { 438, 44, 0, 0, 8, null },
                    { 439, 44, 0, 0, 9, null },
                    { 440, 44, 0, 0, 10, null },
                    { 441, 45, 0, 0, 1, null },
                    { 442, 45, 0, 0, 2, null },
                    { 443, 45, 0, 0, 3, null },
                    { 444, 45, 0, 0, 4, null },
                    { 445, 45, 0, 0, 5, null },
                    { 446, 45, 0, 0, 6, null },
                    { 447, 45, 0, 0, 7, null },
                    { 448, 45, 0, 0, 8, null },
                    { 449, 45, 0, 0, 9, null },
                    { 450, 45, 0, 0, 10, null },
                    { 451, 46, 0, 0, 1, null },
                    { 452, 46, 0, 0, 2, null },
                    { 453, 46, 0, 0, 3, null },
                    { 454, 46, 0, 0, 4, null },
                    { 455, 46, 0, 0, 5, null },
                    { 456, 46, 0, 0, 6, null },
                    { 457, 46, 0, 0, 7, null },
                    { 458, 46, 0, 0, 8, null },
                    { 459, 46, 0, 0, 9, null },
                    { 460, 46, 0, 0, 10, null },
                    { 461, 47, 0, 0, 1, null },
                    { 462, 47, 0, 0, 2, null },
                    { 463, 47, 0, 0, 3, null },
                    { 464, 47, 0, 0, 4, null },
                    { 465, 47, 0, 0, 5, null },
                    { 466, 47, 0, 0, 6, null },
                    { 467, 47, 0, 0, 7, null },
                    { 468, 47, 0, 0, 8, null },
                    { 469, 47, 0, 0, 9, null },
                    { 470, 47, 0, 0, 10, null },
                    { 471, 48, 0, 0, 1, null },
                    { 472, 48, 0, 0, 2, null },
                    { 473, 48, 0, 0, 3, null },
                    { 474, 48, 0, 0, 4, null },
                    { 475, 48, 0, 0, 5, null },
                    { 476, 48, 0, 0, 6, null },
                    { 477, 48, 0, 0, 7, null },
                    { 478, 48, 0, 0, 8, null },
                    { 479, 48, 0, 0, 9, null },
                    { 480, 48, 0, 0, 10, null },
                    { 481, 49, 0, 0, 1, null },
                    { 482, 49, 0, 0, 2, null },
                    { 483, 49, 0, 0, 3, null },
                    { 484, 49, 0, 0, 4, null },
                    { 485, 49, 0, 0, 5, null },
                    { 486, 49, 0, 0, 6, null },
                    { 487, 49, 0, 0, 7, null },
                    { 488, 49, 0, 0, 8, null },
                    { 489, 49, 0, 0, 9, null },
                    { 490, 49, 0, 0, 10, null },
                    { 491, 50, 0, 0, 1, null },
                    { 492, 50, 0, 0, 2, null },
                    { 493, 50, 0, 0, 3, null },
                    { 494, 50, 0, 0, 4, null },
                    { 495, 50, 0, 0, 5, null },
                    { 496, 50, 0, 0, 6, null },
                    { 497, 50, 0, 0, 7, null },
                    { 498, 50, 0, 0, 8, null },
                    { 499, 50, 0, 0, 9, null },
                    { 500, 50, 0, 0, 10, null },
                    { 501, 51, 0, 0, 1, null },
                    { 502, 51, 0, 0, 2, null },
                    { 503, 51, 0, 0, 3, null },
                    { 504, 51, 0, 0, 4, null },
                    { 505, 51, 0, 0, 5, null },
                    { 506, 51, 0, 0, 6, null },
                    { 507, 51, 0, 0, 7, null },
                    { 508, 51, 0, 0, 8, null },
                    { 509, 51, 0, 0, 9, null },
                    { 510, 51, 0, 0, 10, null },
                    { 511, 52, 0, 0, 1, null },
                    { 512, 52, 0, 0, 2, null },
                    { 513, 52, 0, 0, 3, null },
                    { 514, 52, 0, 0, 4, null },
                    { 515, 52, 0, 0, 5, null },
                    { 516, 52, 0, 0, 6, null },
                    { 517, 52, 0, 0, 7, null },
                    { 518, 52, 0, 0, 8, null },
                    { 519, 52, 0, 0, 9, null },
                    { 520, 52, 0, 0, 10, null },
                    { 521, 53, 0, 0, 1, null },
                    { 522, 53, 0, 0, 2, null },
                    { 523, 53, 0, 0, 3, null },
                    { 524, 53, 0, 0, 4, null },
                    { 525, 53, 0, 0, 5, null },
                    { 526, 53, 0, 0, 6, null },
                    { 527, 53, 0, 0, 7, null },
                    { 528, 53, 0, 0, 8, null },
                    { 529, 53, 0, 0, 9, null },
                    { 530, 53, 0, 0, 10, null },
                    { 531, 54, 0, 0, 1, null },
                    { 532, 54, 0, 0, 2, null },
                    { 533, 54, 0, 0, 3, null },
                    { 534, 54, 0, 0, 4, null },
                    { 535, 54, 0, 0, 5, null },
                    { 536, 54, 0, 0, 6, null },
                    { 537, 54, 0, 0, 7, null },
                    { 538, 54, 0, 0, 8, null },
                    { 539, 54, 0, 0, 9, null },
                    { 540, 54, 0, 0, 10, null },
                    { 541, 55, 0, 0, 1, null },
                    { 542, 55, 0, 0, 2, null },
                    { 543, 55, 0, 0, 3, null },
                    { 544, 55, 0, 0, 4, null },
                    { 545, 55, 0, 0, 5, null },
                    { 546, 55, 0, 0, 6, null },
                    { 547, 55, 0, 0, 7, null },
                    { 548, 55, 0, 0, 8, null },
                    { 549, 55, 0, 0, 9, null },
                    { 550, 55, 0, 0, 10, null },
                    { 551, 56, 0, 0, 1, null },
                    { 552, 56, 0, 0, 2, null },
                    { 553, 56, 0, 0, 3, null },
                    { 554, 56, 0, 0, 4, null },
                    { 555, 56, 0, 0, 5, null },
                    { 556, 56, 0, 0, 6, null },
                    { 557, 56, 0, 0, 7, null },
                    { 558, 56, 0, 0, 8, null },
                    { 559, 56, 0, 0, 9, null },
                    { 560, 56, 0, 0, 10, null }
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

            migrationBuilder.InsertData(
                table: "Specialization",
                columns: new[] { "Id", "SpecializationSpellsId" },
                values: new object[,]
                {
                    { 1, "48181,30108,1120" },
                    { 2, "131900,3674,53301" },
                    { 3, "55078,55090,47632" },
                    { 4, "50288,78674,8921" },
                    { 5, "129197,2944,15407" },
                    { 6, "12294,86346,7384" },
                    { 7, "6572,23922,20243" },
                    { 8, "121253,124335,100787" },
                    { 9, "47750,81751,47753" },
                    { 10, "61295,52752,51945" }
                });

            MigrationHelper.CreateTableTypes(migrationBuilder);
            MigrationHelper.CreateProcedures(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BestSpecializationScore");

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
                name: "Player");

            migrationBuilder.DropTable(
                name: "PlayerDeath");

            migrationBuilder.DropTable(
                name: "PlayerStats");

            migrationBuilder.DropTable(
                name: "ResourceRecovery");

            migrationBuilder.DropTable(
                name: "ResourceRecoveryGeneral");

            migrationBuilder.DropTable(
                name: "Specialization");

            migrationBuilder.DropTable(
                name: "SpecializationScore");

            MigrationHelper.DropTableTypes(migrationBuilder);
            MigrationHelper.DropProcedures(migrationBuilder);
        }
    }
}
