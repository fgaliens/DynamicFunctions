using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.LexicalAnalysis.LexicalTokens;

public abstract class LexicalToken : ILexicalToken
{
    private readonly List<TextToken> _textTokens = [];

    public IReadOnlyList<TextToken> TextTokens => _textTokens;

    public void AddSourceToken(TextToken textToken)
    {
        _textTokens.Add(textToken);
    }
    
    public void AddSourceTokens(IEnumerable<TextToken> textTokens)
    {
        _textTokens.AddRange(textTokens);
    }
}