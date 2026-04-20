namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class SubParser : TextPatternParser
{
    override protected string Pattern => "-";
    override protected string TokenType => Tokens.TokenType.SubOperator;
}
