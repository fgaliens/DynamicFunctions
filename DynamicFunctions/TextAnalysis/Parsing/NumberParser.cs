using System.Text.RegularExpressions;

namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed partial class NumberParser : RegexTextParser
{
    [GeneratedRegex(@"^(\-)?\d+(\.\d+)?")]
    private static partial Regex RegexExpression();
    
    protected override string TokenType => Tokens.TokenType.Number;
    protected override Regex GetRegex()
    {
        return RegexExpression();
    }
}