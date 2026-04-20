namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class CloseBracketParser : TextPatternParser
{
    override protected string Pattern => ")";
    override protected string TokenType => Tokens.TokenType.CloseBracket;
}