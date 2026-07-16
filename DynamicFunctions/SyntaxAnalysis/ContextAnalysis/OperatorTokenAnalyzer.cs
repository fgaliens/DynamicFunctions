using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.SyntaxAnalysis.ContextAnalysis;

public sealed class OperatorTokenAnalyzer : ISyntaxContextAnalyzer
{
    public Type TokenType => typeof(OperatorToken);

    public void Handle(ILexicalToken token, SyntaxAnalysisContext context)
    {
        var op = (OperatorToken)token;
        while (context.Operators.Count > 0 && ShouldPop(context.Operators.Peek(), op))
            context.PopAndBuild();
        context.Operators.Push(op);
    }

    private static bool ShouldPop(OperatorToken top, OperatorToken current)
    {
        // Lower Priority value means higher precedence.
        return current.IsRightAssociative
            ? top.Priority < current.Priority
            : top.Priority <= current.Priority;
    }
}
