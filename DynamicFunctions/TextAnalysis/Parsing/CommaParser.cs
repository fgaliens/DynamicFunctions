namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class CommaParser : OneCharTextParser
{
    override protected char TargetChar => ',';
    override protected string TokenType => Tokens.TokenType.Comma;
}