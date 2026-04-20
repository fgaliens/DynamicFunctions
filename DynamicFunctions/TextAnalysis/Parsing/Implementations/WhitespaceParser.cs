using System.Text.RegularExpressions;

namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed partial class WhitespaceParser : RegexPatternParser
{
    [GeneratedRegex(@"^\s+")]
    private static partial Regex RegexExpression();
    
    override protected string TokenType => Tokens.TokenType.WhiteSpace;
    override protected Regex GetRegex()
    {
        return RegexExpression();
    }
}