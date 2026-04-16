using System.Text.RegularExpressions;

namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class CloseBracketParser : OneCharTextParser
{
    override protected char TargetChar => ')';
    override protected string TokenType => Tokens.TokenType.CloseBracket;
}