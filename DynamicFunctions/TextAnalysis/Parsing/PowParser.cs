namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class PowParser : OneCharTextParser
{
    override protected char TargetChar => '^';
    override protected string TokenType => Tokens.TokenType.PowOperator;
}
