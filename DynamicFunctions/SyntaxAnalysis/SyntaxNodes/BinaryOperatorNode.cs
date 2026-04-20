using DynamicFunctions.Compilation;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

public sealed class BinaryOperatorNode : ISyntaxNode
{
    public BinaryOperatorNode(OperatorToken @operator, ISyntaxNode left, ISyntaxNode right)
    {
        Operator = @operator;
        Left = left;
        Right = right;
    }

    public OperatorToken Operator { get; }
    public ISyntaxNode Left { get; }
    public ISyntaxNode Right { get; }
    
    public void Accept(ICompiler compiler)
    {
        Left.Accept(compiler);
        Right.Accept(compiler);
        compiler.CompileNode(this);
    }
}
