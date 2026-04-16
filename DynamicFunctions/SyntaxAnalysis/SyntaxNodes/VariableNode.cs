using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

public sealed class VariableNode : ISyntaxNode
{
    public VariableNode(VariableToken token)
    {
        Token = token;
    }

    public VariableToken Token { get; }
}
