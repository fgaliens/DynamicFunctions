namespace DynamicFunctions.LexicalAnalysis.LexicalParsers;

public class LexicalParsersProviders(IEnumerable<ILexicalParser> unsortedParsers) : ILexicalParsersProvider
{
    private readonly ILexicalParser[] _parsers = unsortedParsers
        .OrderBy(x => x.Priority)
        .ToArray();
    
    public IReadOnlyCollection<ILexicalParser> GetLexicalParsers()
    {
        return _parsers;
    }
}