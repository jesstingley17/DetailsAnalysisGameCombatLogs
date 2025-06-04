using CombatAnalysis.CombatParserAPI.Consts;

namespace CombatAnalysis.CombatParserAPI.Helpers;

internal static class CreateEnvironmentHelper
{
    public static void UseAppsettings(ConfigurationManager configuration)
    {
        var specs = configuration.GetSection("Players:Specs").GetChildren();
        PlayerInfoConfiguration.Specs = specs?.ToDictionary(entry => entry.Key, entry => entry.Value) ?? [];

        var classes = configuration.GetSection("Players:Classes").GetChildren();
        PlayerInfoConfiguration.Classes = classes?.ToDictionary(entry => entry.Key, entry => entry.Value) ?? [];

        var bosses = configuration.GetSection("Players:Bosses").GetChildren();
        PlayerInfoConfiguration.Bosses = bosses?.ToDictionary(entry => entry.Key, entry => entry.Value) ?? [];
    }

    public static void UseEnvVariables()
    {
        var specs = Environment.GetEnvironmentVariable("Players_Specs");
        PlayerInfoConfiguration.Specs = SettingsHelper.ConvertToDictionary(specs) ?? [];

        var classes = Environment.GetEnvironmentVariable("Players_Classes");
        PlayerInfoConfiguration.Classes = SettingsHelper.ConvertToDictionary(classes) ?? [];

        var bosses = Environment.GetEnvironmentVariable("Players_Bosses");
        PlayerInfoConfiguration.Bosses = SettingsHelper.ConvertToDictionary(bosses) ?? [];
    }
}
