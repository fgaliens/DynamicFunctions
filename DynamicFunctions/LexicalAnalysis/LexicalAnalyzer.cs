using DynamicFunctions.LexicalAnalysis.Exceptions;
using DynamicFunctions.LexicalAnalysis.LexicalParsers;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.TextAnalysis.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicFunctions.LexicalAnalysis;

public class LexicalAnalyzer(IServiceProvider serviceProvider) : ILexicalAnalyzer
{
    public IEnumerable<ILexicalToken> Analyze(IEnumerable<TextToken> textTokens)
    {
        var lexicalParsersProvider = serviceProvider.GetRequiredService<ILexicalParsersProvider>();
        var lexicalParsers = lexicalParsersProvider.GetLexicalParsers();
        
        using var textTokensWalker = new TextTokensWalker(textTokens);
        while (!textTokensWalker.Finished)
        {
            var parsed = false;
            foreach (var parser in lexicalParsers)
            {
                textTokensWalker.Reset();
                if (parser.TryTokenize(textTokensWalker, out var lexicalToken))
                {
                    yield return lexicalToken;
                    
                    textTokensWalker.Consume();
                    parsed = true;
                    break;
                }
            }

            if (!parsed && !textTokensWalker.Finished)
            {
                var currentToken = textTokensWalker.Current;
                throw new InvalidTextTokenException(
                    $"Token of type {currentToken.Type} at index {currentToken.Index} can't be parsed " +
                    $"({currentToken.Text})");
            }
        }
    }
}