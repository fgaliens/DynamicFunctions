using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.SyntaxAnalysis.Exceptions;
using DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

namespace DynamicFunctions.SyntaxAnalysis.ContextAnalysis;

public sealed class SyntaxAnalysisContext(ISyntaxAnalyzer analyzer)
{
    public Stack<ISyntaxNode> Operands { get; } = new();
    public Stack<OperatorToken> Operators { get; } = new();
    public ISyntaxAnalyzer Analyzer { get; } = analyzer;

    public void PopAndBuild()
    {
        if (Operands.Count <= 1 || Operators.Count == 0)
            throw new UnexpectedTokensSequenceException();

        var op = Operators.Pop();
        var right = Operands.Pop();
        var left = Operands.Pop();
        Operands.Push(new BinaryOperatorNode(op, left, right));
    }
}
