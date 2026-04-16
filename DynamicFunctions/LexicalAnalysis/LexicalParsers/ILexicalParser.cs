using System.Diagnostics.CodeAnalysis;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.LexicalAnalysis.LexicalParsers;

public interface ILexicalParser
{
    public int Priority { get; }
    bool TryTokenize(ITextTokensWalker textTokensWalker, [NotNullWhen(true)] out ILexicalToken? lexicalToken);
}
