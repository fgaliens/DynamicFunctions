using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.TextAnalysis.Parsing;

public abstract class TextPatternParser : ITextParser
{
    public int Priority => 1;

    public bool TryParse(ITextReader reader, out TextToken token)
    {
        token = default;
            
        if (reader.Text.IsEmpty || !reader.Text[..Pattern.Length].SequenceEqual(Pattern))
        {
            return false;
        }
        
        token = new TextToken
        {
            Index = reader.Index,
            Length = Pattern.Length,
            Type = TokenType,
            Source = reader.Source,
        };
    
        reader.Consume(Pattern.Length);

        return true;
    }
    
    protected abstract string Pattern { get; }
    protected abstract string TokenType { get; }
}