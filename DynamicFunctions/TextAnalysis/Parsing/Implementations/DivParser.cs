namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class DivParser : TextPatternParser
{
    override protected string Pattern => "/";
    override protected string TokenType => Tokens.TokenType.DivOperator;
}
