using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.LexicalAnalysis.LexicalParsers;

public class OperatorDefinition
{
    public required string TokenType { get; init; }
    public required Func<OperatorToken> Factory { get; init; }
}
