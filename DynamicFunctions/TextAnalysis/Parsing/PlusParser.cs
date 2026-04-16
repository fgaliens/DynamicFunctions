namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class PlusParser : OneCharTextParser
{
    protected override char TargetChar => '+';
    protected override string TokenType => Tokens.TokenType.PlusOperator;
}