using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using DynamicFunctions.LexicalAnalysis.Exceptions;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.LexicalAnalysis.LexicalParsers;

public class NumberParser : ILexicalParser
{
    public int Priority => 0x100;

    public bool TryTokenize(
        ITextTokensWalker textTokensWalker, 
        [NotNullWhen(true)] out ILexicalToken? lexicalToken)
    {
        lexicalToken = default;
        
        if (!textTokensWalker.TryGetNextToken(out var textToken))
        {
            return false;
        }

        if (textToken.Type != TokenType.Number)
        {
            return false;
        }

        var longParsed = long.TryParse(textToken.Text, out var longValue);
        var doubleParsed = double.TryParse(textToken.Text, CultureInfo.InvariantCulture, out var doubleValue);

        if (!longParsed && !doubleParsed)
        {
            throw new InvalidTextTokenException($"Unable to parse value '{textToken.Text}' to number");
        }

        var numberToken = new NumberToken
        {
            DoubleValue = doubleParsed ? doubleValue : longValue,
            LongValue = longParsed ? longValue : (long)Math.Round(doubleValue),
        };
        numberToken.AddSourceToken(textToken);
        
        lexicalToken = numberToken;
        return true;
    }
}