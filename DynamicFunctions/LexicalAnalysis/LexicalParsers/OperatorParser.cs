using System.Diagnostics.CodeAnalysis;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.LexicalAnalysis.LexicalParsers;

public class OperatorParser : ILexicalParser
{
    private readonly Dictionary<string, Func<OperatorToken>> _operators = new()
    {
        { TokenType.PlusOperator, () => new AddOperatorToken() },
        { TokenType.MultOperator, () => new MultOperatorToken() },
        { TokenType.PowOperator, () => new PowOperatorToken() },
    };
    
    public int Priority => 0x20;

    public bool TryTokenize(
        ITextTokensWalker textTokensWalker, 
        [NotNullWhen(true)] out ILexicalToken? lexicalToken)
    {
        lexicalToken = default;
        
        if (!textTokensWalker.TryGetNextToken(out var textToken))
        {
            return false;
        }

        if (!_operators.TryGetValue(textToken.Type, out var operatorCreator))
        {
            return false;
        }

        var operatorToken = operatorCreator();
        
        operatorToken.AddSourceToken(textToken);
        
        lexicalToken = operatorToken;
        return true;
    }
}

