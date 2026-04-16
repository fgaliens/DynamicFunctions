namespace DynamicFunctions.LexicalAnalysis.LexicalTokens;

public class SubOperatorToken : OperatorToken
{
    public override int Priority => 0x30;
}