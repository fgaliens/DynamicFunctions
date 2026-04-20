using System.Reflection.Emit;
using DynamicFunctions.Compilation;
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
        var functionText = "-10+x";
        //var functionText = "10 * (x + 8) * sin(x)";
        //var functionText = "10 * (sin(x) + 8)";

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
        var syntaxNodesTree = syntaxAnalyzer.Analyze(lexicalTokens);

        var sinFunction = new FunctionDefinition
        {
            Name = "sin",
            ArgsCount = 1,
            Method = typeof(Math).GetMethod(nameof(Math.Sin))!,
        };

        var dynamicMethod = new DynamicMethod(
            name: "DynamicFunc",                                                                                                                                          
            returnType: typeof(double),                                                                                                                                 
            parameterTypes: [typeof(double)]);
        var compiler = new CilCompiler(new CilCompilationOptions
        {
            DynamicMethod = dynamicMethod,
            Arguments = new Dictionary<string, int>
            {
                {
                    "x", 0
                }
            },
            Functions = new Dictionary<string, FunctionDefinition>
            {
                {
                    sinFunction.Name, sinFunction
                }
            },
            Type = NumberType.Double,
        });
        
        syntaxNodesTree.Accept(compiler);
        compiler.Complete();

        var dlg = dynamicMethod.CreateDelegate<Func<double, double>>();
        var res1 = dlg(0);
        var res2 = dlg(1);
        var res3 = dlg(10);
        
        
        int i = 0;
    }
}
