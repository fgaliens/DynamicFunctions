using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.LexicalAnalysis.LexicalTokens;

public interface ILexicalToken
{
    IReadOnlyList<TextToken> TextTokens { get; }
}