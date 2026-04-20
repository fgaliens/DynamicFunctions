using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.SyntaxAnalysis.ContextAnalysis;
using DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

namespace DynamicFunctions.SyntaxAnalysis;

public sealed class SyntaxAnalyzer(IEnumerable<ISyntaxContextAnalyzer> parsers) : ISyntaxAnalyzer
{
    private readonly Dictionary<Type, ISyntaxContextAnalyzer> _parsers =
        parsers.ToDictionary(p => p.TokenType);

    public ISyntaxNode Analyze(IEnumerable<ILexicalToken> tokens)
    {
        var context = new SyntaxAnalysisContext(this);

        foreach (var token in tokens)
        {
            var parser = FindParser(token)
                ?? throw new InvalidOperationException($"Unexpected token type: {token.GetType().Name}");
            parser.Handle(token, context);
        }

        while (context.Operators.Count > 0)
            context.PopAndBuild();

        if (context.Operands.Count != 1)
            throw new InvalidOperationException("Invalid expression: expected a single root node.");

        return context.Operands.Pop();
    }

    private ISyntaxContextAnalyzer? FindParser(ILexicalToken token)
    {
        var type = token.GetType();
        while (type is not null)
        {
            if (_parsers.TryGetValue(type, out var parser))
                return parser;
            type = type.BaseType;
        }
        return null;
    }
}
