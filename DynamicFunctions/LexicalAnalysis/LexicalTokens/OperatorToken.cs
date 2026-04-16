namespace DynamicFunctions.LexicalAnalysis.LexicalTokens;

public abstract class OperatorToken : LexicalToken
{
    public abstract int Priority { get; }
}