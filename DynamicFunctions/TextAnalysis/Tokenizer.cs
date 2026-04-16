using DynamicFunctions.TextAnalysis.Parsing;
using DynamicFunctions.TextAnalysis.Tokens;
using TextReader = DynamicFunctions.TextAnalysis.Parsing.TextReader;

namespace DynamicFunctions.TextAnalysis;

internal class Tokenizer(IEnumerable<ITextParser> parsers) : ITokenizer
{
    private readonly ITextParser[] _parsers = parsers
        .OrderBy(p => p.Priority)
        .ToArray();

    public IEnumerable<TextToken> Tokenize(string input)
    {
        var textConsumer = new TextReader(input);
        while (!textConsumer.Text.IsEmpty)
        {
            var parsed = false;
            foreach (var parser in _parsers)
            {
                if (parser.TryParse(textConsumer, out var token))
                {
                    yield return token;
                    
                    parsed = true;
                    break;
                }
            }

            if (!parsed)
            {
                throw new UnknownCharException
                {
                    SourceString = input,
                    Index = textConsumer.Index,
                    Character = textConsumer.Text.IsEmpty ? char.MinValue : textConsumer.Text[0],
                };
            }
        }
    }
}