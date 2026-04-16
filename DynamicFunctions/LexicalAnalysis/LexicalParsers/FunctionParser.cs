using System.Diagnostics.CodeAnalysis;
using DynamicFunctions.LexicalAnalysis.Exceptions;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.LexicalAnalysis.LexicalParsers;

public class FunctionParser(ILexicalAnalyzer lexicalAnalyzer) : ILexicalParser
{
    public int Priority => 0x10;

    public bool TryTokenize(
        ITextTokensWalker textTokensWalker, 
        [NotNullWhen(true)] out ILexicalToken? lexicalToken)
    {
        lexicalToken = default;
        
        if (!textTokensWalker.TryGetNextToken(out var textToken) 
            || !textTokensWalker.TryGetNextToken(out var openBracketToken)
            || !textTokensWalker.TryGetNextToken(out var nextToken))
        {
            return false;
        }

        if (textToken.Type != TokenType.Text || openBracketToken.Type != TokenType.OpenBracket)
        {
            return false;
        }
        
        var functionToken = new FunctionToken
        {
            Name = textToken.Text.ToString(),
        };
        
        lexicalToken = functionToken;
        
        functionToken.AddSourceToken(textToken);
        functionToken.AddSourceToken(openBracketToken);

        if (nextToken.Type == TokenType.CloseBracket)
        {
            functionToken.AddSourceToken(nextToken);
            return true;
        }
        
        var bracketsBalance = 1;
        var tokensBuffer = new List<TextToken>();

        do
        {
            if (nextToken.Type == TokenType.OpenBracket)
            {
                bracketsBalance++;
            }
            else if (nextToken.Type == TokenType.CloseBracket)
            {
                bracketsBalance--;
            }
            
            if (bracketsBalance < 0)
            {
                throw new UnpairedBracketException(nextToken);
            }

            if (bracketsBalance == 0)
            {
                var lexicalTokens = lexicalAnalyzer.Analyze(tokensBuffer);
                
                var argument = new GroupToken();
                argument.AddSourceTokens(tokensBuffer);
                argument.AddInnerTokens(lexicalTokens);
                
                functionToken.AddSourceToken(nextToken);
                functionToken.AddArgument(argument);
                
                return true;
            }
            
            if (bracketsBalance == 1 && nextToken.Type == TokenType.Comma)
            {
                var lexicalTokens = lexicalAnalyzer.Analyze(tokensBuffer);
                
                var argument = new GroupToken();
                argument.AddSourceTokens(tokensBuffer);
                argument.AddInnerTokens(lexicalTokens);
                
                functionToken.AddSourceToken(nextToken);
                functionToken.AddArgument(argument);
                
                tokensBuffer.Clear();
            }
            else
            {
                tokensBuffer.Add(nextToken);
            }
        } while (textTokensWalker.TryGetNextToken(out nextToken));
        
        throw new UnpairedBracketException(openBracketToken);
    }
}