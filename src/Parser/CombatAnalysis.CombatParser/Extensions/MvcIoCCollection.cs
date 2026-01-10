using CombatAnalysis.CombatParser.Helpers;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.CombatParser.Services;
using MvvmCross.IoC;

namespace CombatAnalysis.CombatParser.Extensions;

public static class MvcIoCCollection
{
    public static void CombatParserDependencies(this IMvxIoCProvider provider)
    {
        provider.RegisterType<ICombatParserService, CombatParserService>();
        provider.RegisterType<IHttpClientHelper, HttpClientHelper>();
    }
}
