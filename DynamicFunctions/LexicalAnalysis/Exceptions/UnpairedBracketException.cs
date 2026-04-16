using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.LexicalAnalysis.Exceptions;

public class UnpairedBracketException(in TextToken token) : DynamicFunctionsException(GetMessage(token))
{
    private static string GetMessage(in TextToken textToken)
    {
        return $"There is an unpaired bracket at index {textToken.Index}";
    }
}