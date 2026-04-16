using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

namespace DynamicFunctions.SyntaxAnalysis;

public interface ISyntaxAnalyzer
{
    ISyntaxNode Analyze(IEnumerable<ILexicalToken> tokens);
}