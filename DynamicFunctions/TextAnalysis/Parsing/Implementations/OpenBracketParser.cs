namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class OpenBracketParser : TextPatternParser
{
    override protected string Pattern => "(";
    override protected string TokenType => Tokens.TokenType.OpenBracket;
    
}