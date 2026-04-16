namespace DynamicFunctions.LexicalAnalysis.LexicalTokens;

public interface IGroupToken : ILexicalToken
{
    IReadOnlyList<ILexicalToken> InnerTokens { get; }
}