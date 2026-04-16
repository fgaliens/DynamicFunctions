using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.LexicalAnalysis;

public interface ILexicalAnalyzer
{
    IEnumerable<ILexicalToken> Analyze(IEnumerable<TextToken> textTokens);
}