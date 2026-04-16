using System.Diagnostics.CodeAnalysis;
using DynamicFunctions.LexicalAnalysis.Exceptions;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.LexicalAnalysis.LexicalParsers;

public class BracketsParser(ILexicalAnalyzer lexicalAnalyzer) : ILexicalParser
{
    public int Priority => 0x0;

    public bool TryTokenize(
        ITextTokensWalker textTokensWalker, 
        [NotNullWhen(true)] out ILexicalToken? lexicalToken)
    {
        lexicalToken = default;
        
        if (!textTokensWalker.TryGetNextToken(out var textToken))
        {
            return false;
        }

        if (textToken.Type != TokenType.OpenBracket)
        {
            return false;
        }
        
        var groupToken = new GroupToken();
        groupToken.AddSourceToken(textToken);
        
        var innerTokens = EnumerateTextTokensBetweenBrackets(textTokensWalker, groupToken)
// #if DEBUG
//                 .ToArray()
// #endif
            ;
        var lexicalTokens = lexicalAnalyzer.Analyze(innerTokens);
        groupToken.AddInnerTokens(lexicalTokens);
        
        lexicalToken = groupToken;
        return true;
    }

    private static IEnumerable<TextToken> EnumerateTextTokensBetweenBrackets(
        ITextTokensWalker textTokensWalker,
        GroupToken groupToken)
    {
        var bracketsBalance = 1;
        
        while (textTokensWalker.TryGetNextToken(out var textToken))
        {
            if (textToken.Type == TokenType.OpenBracket)
            {
                bracketsBalance++;
            }
            else if (textToken.Type == TokenType.CloseBracket)
            {
                bracketsBalance--;
            }

            if (bracketsBalance == 0)
            {
                groupToken.AddSourceToken(textToken);
                yield break;
            } 
            if (bracketsBalance < 0)
            {
                throw new UnpairedBracketException(textToken);
            }
            
            yield return textToken;
        }
        
        throw new UnpairedBracketException(groupToken.TextTokens[0]);
    }
}