namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class PowParser : TextPatternParser
{
    override protected string Pattern => "^";
    override protected string TokenType => Tokens.TokenType.PowOperator;
}
