using System.Diagnostics.CodeAnalysis;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.LexicalAnalysis.LexicalParsers;

public class VariableParser(ILexicalAnalyzer lexicalAnalyzer) : ILexicalParser
{
    public int Priority => 0x15;

    public bool TryTokenize(
        ITextTokensWalker textTokensWalker,
        [NotNullWhen(true)] out ILexicalToken? lexicalToken)
    {
        lexicalToken = default;
        
        if (!textTokensWalker.TryGetNextToken(out var textToken))
        {
            return false;
        }

        if (textToken.Type != TokenType.Text)
        {
            return false;
        }
        
        var variableToken = new VariableToken
        {
            Name = textToken.Text.ToString(),
        };
        variableToken.AddSourceToken(textToken);
        
        lexicalToken = variableToken;
        return true;
    }
}