using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.SyntaxAnalysis.ContextAnalysis;

public sealed class GroupTokenAnalyzer : ISyntaxContextAnalyzer
{
    public Type TokenType => typeof(GroupToken);

    public void Handle(ILexicalToken token, SyntaxAnalysisContext context)
    {
        var groupToken = (GroupToken)token;
        context.Operands.Push(context.Analyzer.Analyze(groupToken.InnerTokens));
    }
}
