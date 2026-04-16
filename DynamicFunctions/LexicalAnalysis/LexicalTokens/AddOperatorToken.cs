namespace DynamicFunctions.LexicalAnalysis.LexicalTokens;

public class AddOperatorToken : OperatorToken
{
    public override int Priority => 0x30;
}