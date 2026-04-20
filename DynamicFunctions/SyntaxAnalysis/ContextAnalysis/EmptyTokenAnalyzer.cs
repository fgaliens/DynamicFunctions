using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.SyntaxAnalysis.ContextAnalysis;

public sealed class EmptyTokenAnalyzer : ISyntaxContextAnalyzer
{
    public Type TokenType => typeof(EmptyToken);
    public void Handle(ILexicalToken token, SyntaxAnalysisContext context) { }
}
