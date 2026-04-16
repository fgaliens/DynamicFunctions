namespace DynamicFunctions.LexicalAnalysis.LexicalTokens;

public class GroupToken : LexicalToken, IGroupToken
{
    private readonly List<ILexicalToken> _tokens = [];

    public IReadOnlyList<ILexicalToken> InnerTokens => _tokens;

    public void AddInnerTokens(IEnumerable<ILexicalToken> tokens)
    {
        _tokens.AddRange(tokens);
    }
}