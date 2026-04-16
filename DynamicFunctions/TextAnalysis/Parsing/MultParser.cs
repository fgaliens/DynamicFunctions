namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class MultParser : OneCharTextParser
{
    override protected char TargetChar => '*';
    override protected string TokenType => Tokens.TokenType.MultOperator;
}