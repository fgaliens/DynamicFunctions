namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class PlusParser : OneCharTextParser
{
    override protected char TargetChar => '+';
    override protected string TokenType => Tokens.TokenType.PlusOperator;
}