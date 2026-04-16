using System.Diagnostics.CodeAnalysis;
using DynamicFunctions.LexicalAnalysis.Exceptions;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.LexicalAnalysis.LexicalParsers;

public class OperatorParser : ILexicalParser
{
    private readonly HashSet<string> _operators =
    [
        TokenType.PlusOperator,
        TokenType.MultOperator,
    ];
    
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

        if (!_operators.Contains(textToken.Type))
        {
            return false;
        }

        OperatorToken operatorToken = textToken.Type switch
        {
            TokenType.PlusOperator => new AddOperatorToken(),
            TokenType.MultOperator => new MultOperatorToken(),
            _ => throw new InvalidTextTokenException($"Unexpected token type '{textToken.Type}'")
        };
        
        operatorToken.AddSourceToken(textToken);
        
        lexicalToken = operatorToken;
        return true;
    }
}