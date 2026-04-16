using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.TextAnalysis;

public interface ITokenizer
{
    IEnumerable<TextToken> Tokenize(string input);
}