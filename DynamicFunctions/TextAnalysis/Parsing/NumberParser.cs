using System.Text.RegularExpressions;

namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed partial class NumberParser : RegexTextParser
{
    [GeneratedRegex(@"^(\-)?\d+(\.\d+)?")]
    private static partial Regex RegexExpression();
    
    override protected string TokenType => Tokens.TokenType.Number;
    override protected Regex GetRegex()
    {
        return RegexExpression();
    }
}