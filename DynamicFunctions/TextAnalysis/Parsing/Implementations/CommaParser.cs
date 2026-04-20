namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class CommaParser : TextPatternParser
{
    override protected string Pattern => ",";
    override protected string TokenType => Tokens.TokenType.Comma;
}