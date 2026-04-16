using System.Text.RegularExpressions;

namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed partial class VariableParser : RegexTextParser
{
    [GeneratedRegex(@"^\w[\w\d]*")]
    private static partial Regex RegexExpression();
    
    protected override string TokenType => Tokens.TokenType.Text;
    protected override Regex GetRegex()
    {
        return RegexExpression();
    }
}