using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

public sealed class NumberNode : ISyntaxNode
{
    public NumberNode(NumberToken token)
    {
        Token = token;
    }

    public NumberToken Token { get; }
}
