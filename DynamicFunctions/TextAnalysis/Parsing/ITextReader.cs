namespace DynamicFunctions.TextAnalysis.Parsing;

public interface ITextReader
{
    int Index { get; }
    ReadOnlySpan<char> Text { get; }
    string Source { get; }
    void Consume(int length);
}