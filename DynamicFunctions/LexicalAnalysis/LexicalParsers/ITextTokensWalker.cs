using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.LexicalAnalysis.LexicalParsers;

public interface ITextTokensWalker
{
    bool TryGetNextToken(out TextToken token);
    bool ReturnToPrevious();
}