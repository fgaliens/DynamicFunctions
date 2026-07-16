using DynamicFunctions.LexicalAnalysis.LexicalParsers;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.TextAnalysis.Tokens;
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

        services
            .AddOperatorDefinition(TokenType.AddOperator, () => new AddOperatorToken())
            .AddOperatorDefinition(TokenType.SubOperator, () => new SubOperatorToken())
            .AddOperatorDefinition(TokenType.MultOperator, () => new MultOperatorToken())
            .AddOperatorDefinition(TokenType.DivOperator, () => new DivOperatorToken())
            .AddOperatorDefinition(TokenType.PowOperator, () => new PowOperatorToken());

        return services;
    }

    public static IServiceCollection AddLexicalParser<T>(this IServiceCollection services) where T : class, ILexicalParser
    {
        return services.AddSingleton<ILexicalParser, T>();
    }

    public static IServiceCollection AddOperatorDefinition(
        this IServiceCollection services,
        string tokenType,
        Func<OperatorToken> factory)
    {
        return services.AddSingleton(new OperatorDefinition
        {
            TokenType = tokenType,
            Factory = factory,
        });
    }
}