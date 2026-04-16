using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.TextAnalysis.Parsing;

public abstract class OneCharTextParser : ITextParser
{
    public int Priority => 1;

    public bool TryParse(ITextReader reader, out TextToken token)
    {
        token = default;
            
        if (reader.Text.IsEmpty || reader.Text[0] != TargetChar)
        {
            return false;
        }
        
        token = new TextToken
        {
            Index = reader.Index,
            Length = 1,
            Type = TokenType,
            Source = reader.Source,
        };
    
        reader.Consume(1);

        return true;
    }
    
    protected abstract char TargetChar { get; }
    protected abstract string TokenType { get; }
}