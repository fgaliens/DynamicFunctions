using System.Diagnostics.CodeAnalysis;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.LexicalAnalysis.LexicalParsers;

public class OperatorParser(IEnumerable<OperatorDefinition> definitions) : ILexicalParser
{
    private readonly Dictionary<string, Func<OperatorToken>> _operators = BuildOperatorsMap(definitions);

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

    private static Dictionary<string, Func<OperatorToken>> BuildOperatorsMap(
        IEnumerable<OperatorDefinition> definitions)
    {
        var operators = new Dictionary<string, Func<OperatorToken>>();

        foreach (var definition in definitions)
        {
            operators.TryAdd(definition.TokenType, definition.Factory);
        }

        return operators;
    }
}

