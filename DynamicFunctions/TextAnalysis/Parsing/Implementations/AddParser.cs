namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class AddParser : TextPatternParser
{
    override protected string Pattern => "+";
    override protected string TokenType => Tokens.TokenType.AddOperator;
}