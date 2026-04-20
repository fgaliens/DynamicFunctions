using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

namespace DynamicFunctions.SyntaxAnalysis.ContextAnalysis;

public sealed class ConstantTokenAnalyzer : ISyntaxContextAnalyzer
{
    public Type TokenType => typeof(NumberToken);

    public void Handle(ILexicalToken token, SyntaxAnalysisContext context)
    {
        context.Operands.Push(new ConstantNode((NumberToken)token));
    }
}
