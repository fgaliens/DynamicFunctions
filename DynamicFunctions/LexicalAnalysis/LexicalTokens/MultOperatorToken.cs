namespace DynamicFunctions.LexicalAnalysis.LexicalTokens;

public class MultOperatorToken : OperatorToken
{
    public override int Priority => 0x20;
}