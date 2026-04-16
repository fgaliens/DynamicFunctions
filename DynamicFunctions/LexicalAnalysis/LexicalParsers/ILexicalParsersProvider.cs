namespace DynamicFunctions.LexicalAnalysis.LexicalParsers;

public interface ILexicalParsersProvider
{
    IReadOnlyCollection<ILexicalParser> GetLexicalParsers();
}