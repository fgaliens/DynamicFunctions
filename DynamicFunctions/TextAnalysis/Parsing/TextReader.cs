namespace DynamicFunctions.TextAnalysis.Parsing;

public class TextReader(string text) : ITextReader
{
    public int Index { get; private set; }
    
    public ReadOnlySpan<char> Text => text.AsSpan(Index);
    
    public string Source => text;

    public void Consume(int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);
        Index += length;
    }
}