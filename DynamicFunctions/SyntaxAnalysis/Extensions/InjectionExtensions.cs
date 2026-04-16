using Microsoft.Extensions.DependencyInjection;

namespace DynamicFunctions.SyntaxAnalysis.Extensions;

public static class InjectionExtensions
{
    public static IServiceCollection AddSyntaxAnalysis(this IServiceCollection services)
    {
        return services.AddSingleton<ISyntaxAnalyzer, SyntaxAnalyzer>();
    }
}
