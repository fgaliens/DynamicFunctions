namespace DynamicFunctions.LexicalAnalysis.LexicalTokens;

public class NumberToken : LexicalToken
{
    public required double DoubleValue { get; init; }
    
    public required long LongValue { get; init; }
}