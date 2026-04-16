using DynamicFunctions.LexicalAnalysis.LexicalParsers;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicFunctions.LexicalAnalysis.Extensions;

public static class InjectionExtensions
{
    public static IServiceCollection AddLexicalAnalysis(this IServiceCollection services)
    {
        services
            .AddSingleton<ILexicalAnalyzer, LexicalAnalyzer>()
            .AddSingleton<ILexicalParsersProvider, LexicalParsersProviders>();
            
        services
            .AddLexicalParser<BracketsParser>()
            .AddLexicalParser<FunctionParser>()
            .AddLexicalParser<VariableParser>()
            .AddLexicalParser<NumberParser>()
            .AddLexicalParser<OperatorParser>()
            .AddLexicalParser<WhiteSpaceParser>();
        
        return services;
    }
    
    public static IServiceCollection AddLexicalParser<T>(this IServiceCollection services) where T : class, ILexicalParser
    {
        return services.AddSingleton<ILexicalParser, T>();
    }
}