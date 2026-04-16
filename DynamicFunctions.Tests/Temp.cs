using DynamicFunctions.LexicalAnalysis;
using DynamicFunctions.LexicalAnalysis.Extensions;
using DynamicFunctions.SyntaxAnalysis;
using DynamicFunctions.SyntaxAnalysis.Extensions;
using DynamicFunctions.TextAnalysis;
using DynamicFunctions.TextAnalysis.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicFunctions.Tests;

public class Temp
{
    [Fact]
    public void Test1()
    {
        //var functionText = "-10+x";
        //var functionText = "10 * (x + 8) * sin(x)";
        var functionText = "10 * (x + 8) * sin(cos(x))";

        var serviceProvider = new ServiceCollection()
            .AddTextAnalysis()
            .AddLexicalAnalysis()
            .AddSyntaxAnalysis()
            .BuildServiceProvider();

        var tokenizer = serviceProvider.GetRequiredService<ITokenizer>();
        var lexicalAnalyzer = serviceProvider.GetRequiredService<ILexicalAnalyzer>();
        var syntaxAnalyzer = serviceProvider.GetRequiredService<ISyntaxAnalyzer>();

        var tokens = tokenizer.Tokenize(functionText).ToArray();
        var lexicalTokens = lexicalAnalyzer.Analyze(tokens).ToArray();
        var syntaxNodes = syntaxAnalyzer.Analyze(lexicalTokens);

        int i = 0;
    }
}
