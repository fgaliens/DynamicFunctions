namespace DynamicFunctions.TextAnalysis.Parsing;

public sealed class CommaParser : OneCharTextParser
{
    protected override char TargetChar => ',';
    protected override string TokenType => Tokens.TokenType.Comma;
}