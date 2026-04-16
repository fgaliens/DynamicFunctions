using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.TextAnalysis.Parsing;

public interface ITextParser
{
    int Priority { get; }
    bool TryParse(ITextReader reader, out TextToken token);
}