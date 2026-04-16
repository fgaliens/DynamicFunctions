using System.Diagnostics.CodeAnalysis;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.LexicalAnalysis.LexicalParsers;

public class WhiteSpaceParser : ILexicalParser
{
    public int Priority => 0x1000;

    public bool TryTokenize(
        ITextTokensWalker textTokensWalker, 
        [NotNullWhen(true)] out ILexicalToken? lexicalToken)
    {
        lexicalToken = default;
        
        if (!textTokensWalker.TryGetNextToken(out var textToken))
        {
            return false;
        }

        if (textToken.Type != TokenType.WhiteSpace)
        {
            return false;
        }

        var emptyToken = new EmptyToken();
        emptyToken.AddSourceToken(textToken);
        
        lexicalToken = emptyToken;
        return true;
    }
}