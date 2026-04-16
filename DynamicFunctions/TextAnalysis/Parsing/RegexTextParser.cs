using System.Text.RegularExpressions;
using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.TextAnalysis.Parsing;

public abstract class RegexTextParser : ITextParser
{
    public virtual int Priority => 1;

    public bool TryParse(ITextReader reader, out TextToken token)
    {
        token = default;
        
        var regex = GetRegex();
        var matches = regex.EnumerateMatches(reader.Text);
        if (!matches.MoveNext())
        {
            return false;
        }
        
        var match = matches.Current;
        if (match.Index != 0)
        {
            return false;
        }

        token = new TextToken
        {
            Index = reader.Index,
            Length = match.Length,
            Type = TokenType,
            Source = reader.Source,
        };
        
        reader.Consume(match.Length);

        return true;
    }

    protected abstract string TokenType { get; }
    protected abstract Regex GetRegex();
}