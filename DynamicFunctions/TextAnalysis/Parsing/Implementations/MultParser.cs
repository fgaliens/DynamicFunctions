namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class MultParser : TextPatternParser
{
    override protected string Pattern => "*";
    override protected string TokenType => Tokens.TokenType.MultOperator;
}