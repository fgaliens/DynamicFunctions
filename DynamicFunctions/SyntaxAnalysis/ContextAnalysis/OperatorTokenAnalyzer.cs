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
        if (current is PowOperatorToken && top is PowOperatorToken)
            return false;

        return top.Priority <= current.Priority;
    }
}
