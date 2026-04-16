using DynamicFunctions.LexicalAnalysis;
using DynamicFunctions.LexicalAnalysis.Extensions;
using DynamicFunctions.TextAnalysis;
using DynamicFunctions.TextAnalysis.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicFunctions.Tests;

public class Temp
{
    [Fact]
    public void Test1()
    {
        var functionText = "10 * (x + 8) * sin(x)";

        var serviceProvider = new ServiceCollection()
            .AddTextAnalysis()
            .AddLexicalAnalysis()
            .BuildServiceProvider();

        var tokenizer = serviceProvider.GetRequiredService<ITokenizer>();
        var lexicalAnalyzer = serviceProvider.GetRequiredService<ILexicalAnalyzer>();

        var tokens = tokenizer.Tokenize(functionText).ToArray();
        var lexicalTokens = lexicalAnalyzer.Analyze(tokens).ToArray();

        int i = 0;
    }
}
