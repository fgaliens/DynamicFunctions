namespace DynamicFunctions.TextAnalysis.Parsing;

internal sealed class SymbolTextParser(string pattern, string tokenType) : TextPatternParser
{
    protected override string Pattern => pattern;
    protected override string TokenType => tokenType;
}
