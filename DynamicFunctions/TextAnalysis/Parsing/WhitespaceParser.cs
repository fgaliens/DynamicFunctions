using System.Text.RegularExpressions;

namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed partial class WhitespaceParser : RegexTextParser
{
    [GeneratedRegex(@"^\s+")]
    private static partial Regex RegexExpression();
    
    protected override string TokenType => Tokens.TokenType.WhiteSpace;
    protected override Regex GetRegex()
    {
        return RegexExpression();
    }
}