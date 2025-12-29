using CombatAnalysis.DAL.Entities;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

namespace CombatAnalysis.DAL.Helpers;

internal static class MigrationHelper
{
    private static readonly Type[] _defaultTypes =
    [
            typeof(CombatLog),
            typeof(CombatPlayer),
            typeof(PlayerParseInfo),
            typeof(PlayerStats),
            typeof(SpecializationScore),
            typeof(Combat),
            typeof(PlayerDeath),
            typeof(DamageDoneGeneral),
            typeof(HealDoneGeneral),
            typeof(DamageTakenGeneral),
            typeof(ResourceRecoveryGeneral),
            typeof(CombatAura),
            typeof(CombatPlayerPosition),
            typeof(DamageDone),
            typeof(HealDone),
            typeof(DamageTaken),
            typeof(ResourceRecovery),
    ];

    private static readonly Type[] _insertValueTypes =
    [
            typeof(CombatLog),
            typeof(Combat),
            typeof(CombatPlayer),
            typeof(PlayerStats),
    ];

    private static readonly Type[] _tableTypes =
    [       
            typeof(PlayerParseInfo),
            typeof(SpecializationScore),
            typeof(PlayerDeath),
            typeof(DamageDoneGeneral),
            typeof(HealDoneGeneral),
            typeof(DamageTakenGeneral),
            typeof(ResourceRecoveryGeneral),
            typeof(CombatAura),
            typeof(CombatPlayerPosition),
            typeof(DamageDone),
            typeof(HealDone),
            typeof(DamageTaken),
            typeof(ResourceRecovery),
    ];

    private static readonly Type[] _paginationTypes =
    [
            typeof(DamageDone),
            typeof(HealDone),
            typeof(DamageTaken),
            typeof(ResourceRecovery),
    ];

    private static readonly Type[] _typesByCombatPlayer =
    [
            typeof(DamageDone),
            typeof(DamageDoneGeneral),
            typeof(HealDone),
            typeof(HealDoneGeneral),
            typeof(DamageTaken),
            typeof(DamageTakenGeneral),
            typeof(ResourceRecovery),
            typeof(ResourceRecoveryGeneral),
            typeof(CombatPlayerPosition),
            typeof(PlayerParseInfo),
            typeof(PlayerStats),
            typeof(PlayerDeath),
    ];

    public static void CreateTableTypes(MigrationBuilder migrationBuilder)
    {
        foreach (var item in _tableTypes)
        {
            var typeParams = CreateParams(item, false);
            migrationBuilder.Sql($"CREATE TYPE dbo.{item.Name}Type AS TABLE({typeParams.Item1});");

            migrationBuilder.Sql($"" +
                "EXEC('" +
                $"CREATE OR ALTER PROCEDURE InsertInto{item.Name}Batch (@Items dbo.{item.Name}Type READONLY)\n" +
                "AS\n" +
                "BEGIN\n" +
                "\tSET NOCOUNT ON;\n" +
                $"\tINSERT INTO {item.Name}({typeParams.Item2})\n" +
                "\tSELECT * FROM @Items;\n" +
                "END" +
                "');");
        }
    }

    public static void CreateProcedures(MigrationBuilder migrationBuilder)
    {
        foreach (var item in _defaultTypes)
        {
            migrationBuilder.Sql($"" +
                "EXEC('" +
                $"CREATE OR ALTER PROCEDURE GetAll{item.Name}\n" +
                "AS\n" +
                "BEGIN\n" +
                "\tSELECT * \n" +
                $"\tFROM {item.Name}\n" +
                "END" +
                "');");

            var property = item.GetProperty("Id");
            migrationBuilder.Sql($"" +
                "EXEC('" +
                $"CREATE OR ALTER PROCEDURE Get{item.Name}ById (@id {Converter(property.PropertyType)})\n" +
                "AS\n" +
                "BEGIN\n" +
                "\tSELECT * \n" +
                $"\tFROM {item.Name}\n" +
                "\tWHERE Id = @id\n" +
                "END" +
                "');");

            var updateParams = UpdateParamsAndValues(item);
            migrationBuilder.Sql($"" +
                "EXEC('" +
                $"CREATE OR ALTER PROCEDURE Update{item.Name} ({updateParams.Item1})\n" +
                "AS\n" +
                "BEGIN\n" +
                $"\tUPDATE {item.Name}\n" +
                $"\tSET {updateParams.Item2}\n" +
                "\tWHERE Id = @Id\n" +
                "END" +
                "');");

            migrationBuilder.Sql($"" +
                "EXEC('" +
                $"CREATE OR ALTER PROCEDURE Delete{item.Name}ById (@id {Converter(property.PropertyType)})\n" +
                "AS\n" +
                "BEGIN\n" +
                $"\tDELETE FROM {item.Name}\n" +
                "\tWHERE Id = @id\n" +
                "END" +
                "');");
        }

        foreach (var item in _insertValueTypes)
        {
            var insertIntoParams = CreateParams(item);
            var insertIntoOutputParams = CreateOutputParams(item);
            migrationBuilder.Sql($"" +
                "EXEC('" +
                $"CREATE OR ALTER PROCEDURE InsertInto{item.Name} ({insertIntoParams.Item1})\n" +
                "AS\n" +
                "BEGIN\n" +
                $"\tDECLARE @OutputTbl TABLE ({insertIntoOutputParams})\n" +
                $"\tINSERT INTO {item.Name}\n" +
                $"\tOUTPUT INSERTED.* INTO @OutputTbl\n" +
                $"\tVALUES ({insertIntoParams.Item2})\n" +
                "\tSELECT * FROM @OutputTbl\n" +
                "END" +
                "');");
        }

        CreateProceduresWithPaginations(migrationBuilder);

        GetDataByCombatPlayerId(migrationBuilder);
        GetSpecializationScore(migrationBuilder);
    }
    
    public static void DropTableTypes(MigrationBuilder migrationBuilder)
    {
        foreach (var item in _tableTypes)
        {
            migrationBuilder.Sql($"DROP TYPE IF EXISTS dbo.{item.Name}Type");
        }
    }

    public static void DropProcedures(MigrationBuilder migrationBuilder)
    {
        foreach (var item in _defaultTypes)
        {
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS GetAll{item.Name}");
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS Get{item.Name}ById");
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS Update{item.Name}");
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS Delete{item.Name}ById");
        }

        foreach (var item in _insertValueTypes)
        {
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS InsertInto{item.Name}");
        }

        foreach (var item in _paginationTypes)
        {
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS Get{item.Name}ByCombatPlayerIdPagination");
        }

        foreach (var item in _typesByCombatPlayer)
        {
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS Get{item.Name}ByCombatPlayerId");
        }

        migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS Get{nameof(SpecializationScore)}BySpecId");
    }

    public static Boss[] GenerateBossCollection()
    {
        Boss[] collection =
        {
            // Подземелье Могу'шан
            new() { GameId = 1395, Name = "Каменные стражи", Health = 130841100, Difficult = 3, Size = 10 },
            new() { GameId = 1395, Name = "Каменные стражи", Health = 235513980, Difficult = 5, Size = 10 },
            new() { GameId = 1390, Name = "Фэн Проклятый", Health = 152647950, Difficult = 3, Size = 10 },
            new() { GameId = 1390, Name = "Фэн Проклятый", Health = 209345760, Difficult = 5, Size = 10 },
            new() { GameId = 1434, Name = "Душелов Гара'джал", Health = 117756990, Difficult = 3, Size = 10 },
            new() { GameId = 1434, Name = "Душелов Гара'джал", Health = 179252307, Difficult = 5, Size = 10 },
            new() { GameId = 1436, Name = "Призрачные короли", Health = 174454800, Difficult = 3, Size = 10 },
            new() { GameId = 1436, Name = "Призрачные короли", Health = 261682200, Difficult = 5, Size = 10 },
            new() { GameId = 1500, Name = "Элегон", Health = 294392475, Difficult = 3, Size = 10 },
            new() { GameId = 1500, Name = "Элегон", Health = 339750723, Difficult = 5, Size = 10 },
            new() { GameId = 1407, Name = "Воля императора", Health = 314018640, Difficult = 3, Size = 10 },
            new() { GameId = 1407, Name = "Воля императора", Health = 471027960, Difficult = 5, Size = 10 },

            // Терраса Вечной Весны
            new() { GameId = 1409, Name = "Вечные защитники", Health = 213968815, Difficult = 3, Size = 10 },
            new() { GameId = 1409, Name = "Вечные защитники", Health = 344082093, Difficult = 5, Size = 10 },
            new() { GameId = 1505, Name = "Цулон", Health = 174454800, Difficult = 3 , Size = 10 },
            new() { GameId = 1505, Name = "Цулон", Health = 279127680, Difficult = 5 , Size = 10 },
            new() { GameId = 1506, Name = "Лэй Ши", Health = 138168195, Difficult = 3 , Size = 10 },
            new() { GameId = 1506, Name = "Лэй Ши", Health = 301457900, Difficult = 5 , Size = 10 },
            new() { GameId = 1431, Name = "Ша Страха", Health = 184704020, Difficult = 3 , Size = 10 },
            new() { GameId = 1431, Name = "Ша Страха", Health = 544037304, Difficult = 5 , Size = 10 },

            // Сердце Страха
            new() { GameId = 1507, Name = "Императорский визирь Зор'лок", Health = 174454800, Difficult = 3, Size = 10 },
            new() { GameId = 1507, Name = "Императорский визирь Зор'лок", Health = 218068500, Difficult = 5, Size = 10 },
            new() { GameId = 1504, Name = "Повелитель клинков Та'як", Health = 150467265, Difficult = 3, Size = 10 },
            new() { GameId = 1504, Name = "Повелитель клинков Та'як", Health = 196261650, Difficult = 5, Size = 10 },
            new() { GameId = 1463, Name = "Гаралон", Health = 218068500, Difficult = 3, Size = 10 },
            new() { GameId = 1463, Name = "Гаралон", Health = 290759446, Difficult = 5, Size = 10 },
            new() { GameId = 1498, Name = "Повелитель ветров Мел'джарак", Health = 270404940, Difficult = 3, Size = 10 },
            new() { GameId = 1498, Name = "Повелитель ветров Мел'джарак", Health = 588784950, Difficult = 5, Size = 10 },
            new() { GameId = 1499, Name = "Ваятель янтаря Ун'сок", Health = 218068500, Difficult = 3, Size = 10 },
            new() { GameId = 1499, Name = "Ваятель янтаря Ун'сок", Health = 340186860, Difficult = 5, Size = 10 },
            new() { GameId = 1501, Name = "Великая императрица Шек'зир", Health = 196261650, Difficult = 3, Size = 10 },
            new() { GameId = 1501, Name = "Великая императрица Шек'зир", Health = 307476585, Difficult = 5, Size = 10 },

            // Престол Гроз
            new() { GameId = 1577, Name = "Джин'рок Разрушитель", Health = 207601212, Difficult = 3, Size = 10 },
            new() { GameId = 1577, Name = "Джин'рок Разрушитель", Health = 317507736, Difficult = 5, Size = 10 },
            new() { GameId = 1575, Name = "Хорридон", Health = 357632340, Difficult = 3, Size = 10 },
            new() { GameId = 1575, Name = "Хорридон", Health = 654205500, Difficult = 5, Size = 10 },
            new() { GameId = 1570, Name = "Совет старейшин", Health = 299538888, Difficult = 3, Size = 10 },
            new() { GameId = 1570, Name = "Совет старейшин", Health = 470330152, Difficult = 5, Size = 10 },
            new() { GameId = 1565, Name = "Тортос", Health = 179999841, Difficult = 3, Size = 10 },
            new() { GameId = 1565, Name = "Тортос", Health = 319999818, Difficult = 5, Size = 10 },
            new() { GameId = 1578, Name = "Мегера", Health = 263317712, Difficult = 3, Size = 10 },
            new() { GameId = 1578, Name = "Мегера", Health = 342297774, Difficult = 5, Size = 10 },
            new() { GameId = 1573, Name = "Цзи-Кунь", Health = 244236720, Difficult = 3, Size = 10 },
            new() { GameId = 1573, Name = "Цзи-Кунь", Health = 366355080, Difficult = 5, Size = 10 },
            new() { GameId = 1572, Name = "Дуруму Позабытый", Health = 261682200, Difficult = 3, Size = 10 },
            new() { GameId = 1572, Name = "Дуруму Позабытый", Health = 392523300, Difficult = 5, Size = 10 },
            new() { GameId = 1574, Name = "Изначалий", Health = 218068500, Difficult = 3, Size = 10 },
            new() { GameId = 1574, Name = "Изначалий", Health = 258193104, Difficult = 5, Size = 10 },
            new() { GameId = 1576, Name = "Темный Анимус", Health = 80999797, Difficult = 3, Size = 10 },
            new() { GameId = 1576, Name = "Темный Анимус", Health = 288000023, Difficult = 5, Size = 10 },
            new() { GameId = 1559, Name = "Кон Железный", Health = 119937675, Difficult = 3, Size = 10 },
            new() { GameId = 1559, Name = "Кон Железный", Health = 155700909, Difficult = 5, Size = 10 },
            new() { GameId = 1560, Name = "Небесные сестры", Health = 219812670, Difficult = 3, Size = 10 },
            new() { GameId = 1560, Name = "Небесные сестры", Health = 628036200, Difficult = 5, Size = 10 },
            new() { GameId = 1579, Name = "Лэй Шэнь", Health = 329283435, Difficult = 3, Size = 10 },
            new() { GameId = 1579, Name = "Лэй Шэнь", Health = 580498347, Difficult = 5, Size = 10 }
        };

        for (int i = 0; i < collection.Length; i++)
        {
            collection[i].Id = i + 1;
        }

        return collection;
    }

    private static void CreateProceduresWithPaginations(MigrationBuilder migrationBuilder)
    {
        foreach (var item in _paginationTypes)
        {
            var property = item.GetProperty("CombatPlayerId");
            migrationBuilder.Sql($"" +
                "EXEC('" +
                $"CREATE OR ALTER PROCEDURE Get{item.Name}ByCombatPlayerIdPagination (@combatPlayerId {Converter(property.PropertyType)}, @page INT, @pageSize INT)\n" +
                "AS\n" +
                "BEGIN\n" +
                "\tSELECT * \n" +
                $"\tFROM {item.Name}\n" +
                "\tWHERE CombatPlayerId = @combatPlayerId\n" +
                $"\tORDER BY Id\n" +
                "\tOFFSET (@page - 1) * @pageSize ROWS\n" +
                "\tFETCH NEXT @pageSize ROWS ONLY\n" +
                "END" +
                "');");
        }
    }

    private static void GetDataByCombatPlayerId(MigrationBuilder migrationBuilder)
    {
        foreach (var item in _typesByCombatPlayer)
        {
            var property = item.GetProperty("CombatPlayerId");
            migrationBuilder.Sql($"" +
                 "EXEC('" +
                $"CREATE OR ALTER PROCEDURE Get{item.Name}ByCombatPlayerId (@combatPlayerId {Converter(property.PropertyType)})\n" +
                "AS\n" +
                "BEGIN\n" +
                "\tSELECT * \n" +
                $"\tFROM {item.Name}\n" +
                "\tWHERE CombatPlayerId = @combatPlayerId\n" +
                "END" +
                "');");
        }
    }

    private static void GetSpecializationScore(MigrationBuilder migrationBuilder)
    {
        var classType = typeof(SpecializationScore);

        var propertySpecId = classType.GetProperty(nameof(SpecializationScore.SpecId));
        var propertyBossId = classType.GetProperty(nameof(SpecializationScore.BossId));
        migrationBuilder.Sql($"" +
            "EXEC('" +
            $"CREATE OR ALTER PROCEDURE Get{classType.Name}BySpecId (@specId {Converter(propertySpecId.PropertyType)}, " +
                                                                  $"@bossId {Converter(propertyBossId.PropertyType)})\n" +
            "AS\n" +
            "BEGIN\n" +
            "\tSELECT * \n" +
            $"\tFROM {classType.Name}\n" +
            "\tWHERE SpecId = @specId AND BossId = @bossId\n" +
            "END" +
            "');");
    }

    private static Tuple<string, string> CreateParams(Type type, bool isStoredProcedure = true)
    {
        var properties = type.GetProperties();
        var paramNames = new StringBuilder();
        var paramNamesWithPropertyTypes = new StringBuilder();
        if (type.GetProperty("Id")?.PropertyType != typeof(int))
        {
            paramNamesWithPropertyTypes.Append(isStoredProcedure ? $"@{properties[0].Name} {Converter(properties[0].PropertyType)}," : $"{properties[0].Name} {Converter(properties[0].PropertyType)},");
            paramNames.Append(isStoredProcedure ? $"@{properties[0].Name}," : $"{properties[0].Name},");
        }

        for (int i = 1; i < properties.Length; i++)
        {
            if (properties[i].CanWrite)
            {
                paramNamesWithPropertyTypes.Append(isStoredProcedure ? $"@{properties[i].Name} {Converter(properties[i].PropertyType)}," : $"{properties[i].Name} {Converter(properties[i].PropertyType)},");
                paramNames.Append(isStoredProcedure ? $"@{properties[i].Name}," : $"{properties[i].Name},");
            }
        }

        paramNamesWithPropertyTypes.Remove(paramNamesWithPropertyTypes.Length - 1, 1);
        paramNames.Remove(paramNames.Length - 1, 1);

        return new Tuple<string, string>(paramNamesWithPropertyTypes.ToString(), paramNames.ToString());
    }

    private static string CreateOutputParams(Type type)
    {
        var properties = type.GetProperties();
        var procedureParamNamesWithPropertyTypes = new StringBuilder();
        for (int i = 0; i < properties.Length; i++)
        {
            if (properties[i].CanWrite)
            {
                procedureParamNamesWithPropertyTypes.Append($"{properties[i].Name} {Converter(properties[i].PropertyType)},");
            }
        }

        procedureParamNamesWithPropertyTypes.Remove(procedureParamNamesWithPropertyTypes.Length - 1, 1);

        return procedureParamNamesWithPropertyTypes.ToString();
    }

    private static Tuple<string, string> UpdateParamsAndValues(Type type)
    {
        var properties = type.GetProperties();
        var procedureParamNames = new StringBuilder();
        var procedureParamNamesWithPropertyTypes = new StringBuilder();
        for (int i = 0; i < properties.Length; i++)
        {
            if (properties[i].CanWrite)
            {
                procedureParamNamesWithPropertyTypes.Append($"@{properties[i].Name} {Converter(properties[i].PropertyType)},");
            }
        }

        for (int i = 1; i < properties.Length; i++)
        {
            if (properties[i].CanWrite)
            {
                procedureParamNames.Append($"{properties[i].Name} = @{properties[i].Name},");
            }
        }

        procedureParamNamesWithPropertyTypes.Remove(procedureParamNamesWithPropertyTypes.Length - 1, 1);
        procedureParamNames.Remove(procedureParamNames.Length - 1, 1);

        return new Tuple<string, string>(procedureParamNamesWithPropertyTypes.ToString(), procedureParamNames.ToString());
    }

    private static string Converter(Type type)
    {
        if (Nullable.GetUnderlyingType(type) is Type underlying)
        {
            type = underlying;
        }

        return type.Name switch
        {
            "String" => "NVARCHAR (MAX)",
            "Int32" => "INT",
            "Int16" => "INT",
            "Boolean" => "BIT",
            "DateTimeOffset" => "DATETIMEOFFSET (7)",
            "Double" => "FLOAT (53)",
            "TimeSpan" => "TIME (7)",
            _ => "NVARCHAR (256)",
        };
    }
}
