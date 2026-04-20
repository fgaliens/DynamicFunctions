using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.SyntaxAnalysis.ContextAnalysis;

public interface ISyntaxContextAnalyzer
{
    Type TokenType { get; }
    void Handle(ILexicalToken token, SyntaxAnalysisContext context);
}
