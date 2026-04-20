using DynamicFunctions.Compilation;
using DynamicFunctions.LexicalAnalysis.LexicalParsers;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.SyntaxAnalysis.ContextAnalysis;
using DynamicFunctions.SyntaxAnalysis.SyntaxNodes;
using DynamicFunctions.TextAnalysis.Parsing;
using DynamicFunctions.TextAnalysis.Tokens;
using System.Diagnostics.CodeAnalysis;

namespace DynamicFunctions.Tests;

public class CustomConfigurationTests
{
    private static double Answer() => 42.0;
    private static double DoubleIt(double a) => a * 2;
    private static double Avg(double a, double b) => (a + b) / 2;
    private static double Clamp(double x, double lo, double hi) => Math.Clamp(x, lo, hi);
    private static double Sum4(double a, double b, double c, double d) => a + b + c + d;
    private static double Add(double a, double b) => a + b;
    private static double Scale(double a) => a * 10;

    [Fact]
    public void CustomFunctionDefinition_ZeroArgs_Works()
    {
        var func = DynamicFunction.Build("answer()")
            .WithType<double>(cfg => cfg
                .AddFunctionDefinition("answer", Answer))
            .Create();

        Assert.Equal(42.0, func());
    }

    [Fact]
    public void CustomFunctionDefinition_OneArg_Works()
    {
        var func = DynamicFunction.Build("doubleit(x)")
            .WithType<double>(cfg => cfg
                .AddFunctionDefinition("doubleit", DoubleIt))
            .Create("x");

        Assert.Equal(10.0, func(5.0));
    }

    [Fact]
    public void CustomFunctionDefinition_TwoArgs_Works()
    {
        var func = DynamicFunction.Build("avg(x, y)")
            .WithType<double>(cfg => cfg
                .AddFunctionDefinition("avg", Avg))
            .Create("x", "y");

        Assert.Equal(5.0, func(3.0, 7.0));
    }

    [Fact]
    public void CustomFunctionDefinition_ThreeArgs_Works()
    {
        var func = DynamicFunction.Build("clamp(x, lo, hi)")
            .WithType<double>(cfg => cfg
                .AddFunctionDefinition("clamp", Clamp))
            .Create("x", "lo", "hi");

        Assert.Equal(5.0, func(10.0, 0.0, 5.0));
    }

    [Fact]
    public void CustomFunctionDefinition_FourArgs_Works()
    {
        var func = DynamicFunction.Build("sum4(a, b, c, d)")
            .WithType<double>(cfg => cfg
                .AddFunctionDefinition("sum4", Sum4))
            .Create("a", "b", "c", "d");

        Assert.Equal(10.0, func(1.0, 2.0, 3.0, 4.0));
    }

    [Fact]
    public void MultipleFunctionDefinitions_Work()
    {
        var func = DynamicFunction.Build("add(x, y) * scale(z)")
            .WithType<double>(cfg => cfg
                .AddFunctionDefinition("add", Add)
                .AddFunctionDefinition("scale", Scale))
            .Create("x", "y", "z");

        Assert.Equal(150.0, func(3.0, 2.0, 3.0));
    }

    [Fact]
    public void CustomCompiler_IsUsed()
    {
        // The custom compiler is registered but since it doesn't emit IL properly,
        // it should fail at delegate creation or execution
        Assert.ThrowsAny<Exception>(() =>
        {
            var func = DynamicFunction.Build("1 + 2")
                .WithType<double>(cfg => cfg
                    .AddCompiler<TrackingCompiler>())
                .Create();
            func();
        });
    }

    [Fact]
    public void CustomTextParser_IsRegistered()
    {
        // Adding a custom text parser that handles '%' as a modulo character
        // Without a corresponding lexical parser it won't fully compile,
        // but it verifies the configuration pipeline accepts custom parsers
        var func = DynamicFunction.Build("2 + 3")
            .WithType<double>(cfg => cfg
                .AddTextParser<ModuloTextParser>())
            .Create();

        Assert.Equal(5.0, func());
    }

    [Fact]
    public void CustomLexicalParser_IsRegistered()
    {
        // Verifies that custom lexical parsers can be registered via configuration
        var func = DynamicFunction.Build("2 + 3")
            .WithType<double>(cfg => cfg
                .AddLexicalParser<CustomKeywordLexicalParser>())
            .Create();

        Assert.Equal(5.0, func());
    }

    [Fact]
    public void CustomSyntaxContextAnalyzer_IsRegistered()
    {
        // Verifies that custom syntax context analyzers can be registered via configuration
        var func = DynamicFunction.Build("1 + 2")
            .WithType<double>(cfg => cfg
                .AddSyntaxContextAnalyzer<CustomSyntaxAnalyzer>())
            .Create();

        Assert.Equal(3.0, func());
    }
}

#region Test Helpers - Custom Implementations

public class TrackingCompiler : ICompiler
{
    public void CompileNode(ConstantNode node) { }
    public void CompileNode(VariableNode node) { }
    public void CompileNode(BinaryOperatorNode node) { }
    public void CompileNode(FunctionCallNode node) { }
    public void Complete() { }
}

public class ModuloTextParser : ITextParser
{
    public int Priority => 100;

    public bool TryParse(ITextReader reader, out TextToken token)
    {
        token = default;
        if (reader.Text.Length > 0 && reader.Text[0] == '%')
        {
            token = new TextToken
            {
                Index = reader.Index,
                Length = 1,
                Type = "Modulo",
                Source = reader.Source,
            };
            reader.Consume(1);
            return true;
        }
        return false;
    }
}

public class CustomKeywordLexicalParser : ILexicalParser
{
    public int Priority => 100;

    public bool TryTokenize(ITextTokensWalker textTokensWalker, [NotNullWhen(true)] out ILexicalToken? lexicalToken)
    {
        lexicalToken = null;
        return false;
    }
}

public class CustomSyntaxAnalyzer : ISyntaxContextAnalyzer
{
    public Type TokenType => typeof(ILexicalToken);
    public void Handle(ILexicalToken token, SyntaxAnalysisContext context) { }
}

#endregion
