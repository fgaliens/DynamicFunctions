using System.Text.RegularExpressions;

namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed partial class VariableParser : RegexTextParser
{
    [GeneratedRegex(@"^\w[\w\d]*")]
    private static partial Regex RegexExpression();
    
    override protected string TokenType => Tokens.TokenType.Text;
    override protected Regex GetRegex()
    {
        return RegexExpression();
    }
}