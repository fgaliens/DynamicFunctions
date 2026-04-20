using DynamicFunctions.TextAnalysis.Parsing;
using Microsoft.Extensions.DependencyInjection;
using TextReader = DynamicFunctions.TextAnalysis.Parsing.TextReader;

namespace DynamicFunctions.TextAnalysis.Extensions;

public static class InjectionExtensions
{
    public static IServiceCollection AddTextAnalysis(this IServiceCollection services)
    {
        services
            .AddSingleton<ITokenizer, Tokenizer>()
            .AddTransient<ITextReader, TextReader>();
        
        services
            .AddTextParser<CloseBracketParser>()
            .AddTextParser<CommaParser>()
            .AddTextParser<MultParser>()
            .AddTextParser<DivParser>()
            .AddTextParser<NumberParser>()
            .AddTextParser<OpenBracketParser>()
            .AddTextParser<AddParser>()
            .AddTextParser<SubParser>()
            .AddTextParser<PowParser>()
            .AddTextParser<TextParser>()
            .AddTextParser<WhitespaceParser>();

        return services;
    }

    public static IServiceCollection AddTextParser<T>(this IServiceCollection services) where T : class, ITextParser
    {
        return services.AddSingleton<ITextParser, T>();
    }
}