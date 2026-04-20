using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

namespace DynamicFunctions.SyntaxAnalysis.ContextAnalysis;

public sealed class VariableTokenAnalyzer : ISyntaxContextAnalyzer
{
    public Type TokenType => typeof(VariableToken);

    public void Handle(ILexicalToken token, SyntaxAnalysisContext context)
    {
        context.Operands.Push(new VariableNode((VariableToken)token));
    }
}
