using DynamicFunctions.SyntaxAnalysis.ContextAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicFunctions.SyntaxAnalysis.Extensions;

public static class InjectionExtensions
{
    public static IServiceCollection AddSyntaxAnalysis(this IServiceCollection services)
    {
        return services
            .AddSyntaxContextAnalyzer<EmptyTokenAnalyzer>()
            .AddSyntaxContextAnalyzer<ConstantTokenAnalyzer>()
            .AddSyntaxContextAnalyzer<VariableTokenAnalyzer>()
            .AddSyntaxContextAnalyzer<FunctionTokenAnalyzer>()
            .AddSyntaxContextAnalyzer<GroupTokenAnalyzer>()
            .AddSyntaxContextAnalyzer<OperatorTokenAnalyzer>()
            .AddSingleton<ISyntaxAnalyzer, SyntaxAnalyzer>();
    }

    public static IServiceCollection AddSyntaxContextAnalyzer<T>(this IServiceCollection services) 
        where T : class, ISyntaxContextAnalyzer
    {
        return services.AddSingleton<ISyntaxContextAnalyzer, T>();
    }
}
