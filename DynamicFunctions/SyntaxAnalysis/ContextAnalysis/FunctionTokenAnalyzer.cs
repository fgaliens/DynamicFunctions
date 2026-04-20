using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

namespace DynamicFunctions.SyntaxAnalysis.ContextAnalysis;

public sealed class FunctionTokenAnalyzer : ISyntaxContextAnalyzer
{
    public Type TokenType => typeof(FunctionToken);

    public void Handle(ILexicalToken token, SyntaxAnalysisContext context)
    {
        var functionToken = (FunctionToken)token;
        var arguments = functionToken.Arguments
            .Cast<IGroupToken>()
            .Select(arg => context.Analyzer.Analyze(arg.InnerTokens))
            .ToList();
        context.Operands.Push(new FunctionCallNode(functionToken, arguments));
    }
}
