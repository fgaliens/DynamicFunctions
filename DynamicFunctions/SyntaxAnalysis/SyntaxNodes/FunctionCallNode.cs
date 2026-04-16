using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

public sealed class FunctionCallNode : ISyntaxNode
{
    public FunctionCallNode(FunctionToken token, IReadOnlyList<ISyntaxNode> arguments)
    {
        Token = token;
        Arguments = arguments;
    }

    public FunctionToken Token { get; }
    public IReadOnlyList<ISyntaxNode> Arguments { get; }
}
