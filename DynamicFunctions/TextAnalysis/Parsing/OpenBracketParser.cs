using System.Text.RegularExpressions;

namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class OpenBracketParser : OneCharTextParser
{
    protected override char TargetChar => '(';
    protected override string TokenType => Tokens.TokenType.OpenBracket;
    
}