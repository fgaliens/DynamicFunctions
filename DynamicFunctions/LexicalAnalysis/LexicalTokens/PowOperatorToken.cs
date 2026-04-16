namespace DynamicFunctions.LexicalAnalysis.LexicalTokens;

public class PowOperatorToken : OperatorToken
{
    public override int Priority => 0x10;
}