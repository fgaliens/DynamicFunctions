namespace DynamicFunctions.LexicalAnalysis.LexicalTokens;

public class FunctionToken : LexicalToken
{
    private readonly List<IGroupToken> _arguments = [];

    public required string Name { get; init; }
    
    public IReadOnlyList<ILexicalToken> Arguments => _arguments;
    
    public FunctionToken AddArgument(IGroupToken token)
    {
        _arguments.Add(token);
        return this;
    }
}