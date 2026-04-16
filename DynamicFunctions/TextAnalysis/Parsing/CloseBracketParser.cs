using System.Text.RegularExpressions;

namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class CloseBracketParser : OneCharTextParser
{
    protected override char TargetChar => ')';
    protected override string TokenType => Tokens.TokenType.CloseBracket;
}