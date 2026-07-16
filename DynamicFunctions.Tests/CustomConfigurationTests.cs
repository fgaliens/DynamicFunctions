using DynamicFunctions.Compilation;
using DynamicFunctions.Compilation.Strategies;
using DynamicFunctions.LexicalAnalysis.LexicalParsers;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.SyntaxAnalysis.ContextAnalysis;
using DynamicFunctions.SyntaxAnalysis.SyntaxNodes;
using DynamicFunctions.TextAnalysis.Parsing;
using DynamicFunctions.TextAnalysis.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;

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
    public void CustomCompiler_ReceivesRequest_AndProducesMethod()
    {
        // The custom compiler ignores the tree and compiles "first argument * 2"
        var func = DynamicFunction.Build("x + 100")
            .WithType<double>(cfg => cfg
                .AddCompiler<DoublingFirstArgCompiler>())
            .Create("x");

        Assert.Equal(42.0, func(21.0));
    }

    [Fact]
    public void CustomCompiler_CanInjectRegisteredServices()
    {
        // The custom compiler receives registered FunctionDefinitions through DI
        // and compiles a constant equal to their count
        var func = DynamicFunction.Build("1")
            .WithType<double>(cfg => cfg
                .AddFunctionDefinition("answer", Answer)
                .AddFunctionDefinition("doubleit", DoubleIt)
                .AddCompiler<FunctionCountingCompiler>())
            .Create();

        Assert.Equal(2.0, func());
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

    [Fact]
    public void CustomOperator_ViaAddOperator_Works()
    {
        var func = DynamicFunction.Build("7 % 4")
            .WithType<double>(cfg => cfg
                .AddOperator<ModOperatorToken>("%", il => il.Emit(OpCodes.Rem)))
            .Create();

        Assert.Equal(3.0, func());
    }

    [Fact]
    public void CustomOperator_ViaAddOperator_RespectsPrecedence()
    {
        var func = DynamicFunction.Build("1 + 7 % 4")
            .WithType<double>(cfg => cfg
                .AddOperator<ModOperatorToken>("%", il => il.Emit(OpCodes.Rem)))
            .Create();

        // ModOperatorToken has mul/div precedence: 1 + (7 % 4) = 4
        Assert.Equal(4.0, func());
    }

    [Fact]
    public void CustomOperator_ViaLowLevelHooks_Works()
    {
        var func = DynamicFunction.Build("7 % 4")
            .WithType<double>(cfg => cfg
                .AddTextParser<ModuloTextParser>()
                .AddOperatorDefinition("Modulo", () => new ModOperatorToken())
                .AddCompilationStrategy<ModOperatorCompilationStrategy>())
            .Create();

        Assert.Equal(3.0, func());
    }

    [Fact]
    public void ConsumerOperatorDefinition_OverridesBuiltIn()
    {
        // '+' now produces a custom token whose strategy emits subtraction
        var func = DynamicFunction.Build("5 + 2")
            .WithType<double>(cfg => cfg
                .AddOperatorDefinition(TokenType.AddOperator, () => new CustomAddOperatorToken())
                .AddCompilationStrategy<CustomAddOperatorCompilationStrategy>())
            .Create();

        Assert.Equal(3.0, func());
    }

    [Fact]
    public void ConsumerCompilationStrategy_OverridesBuiltIn()
    {
        // The built-in AddOperatorToken gets a consumer strategy that emits subtraction
        var func = DynamicFunction.Build("5 + 2")
            .WithType<double>(cfg => cfg
                .AddCompilationStrategy<SubtractingAddCompilationStrategy>())
            .Create();

        Assert.Equal(3.0, func());
    }

    [Fact]
    public void CustomRightAssociativeOperator_GroupsRightToLeft()
    {
        var func = DynamicFunction.Build("10 ~ 5 ~ 2")
            .WithType<double>(cfg => cfg
                .AddOperator<RightAssociativeSubToken>("~", il => il.Emit(OpCodes.Sub)))
            .Create();

        // 10 - (5 - 2) = 7, not (10 - 5) - 2 = 3
        Assert.Equal(7.0, func());
    }
}

#region Test Helpers - Custom Implementations

public class DoublingFirstArgCompiler : IFunctionCompiler
{
    public DynamicMethod Compile(ISyntaxNode syntaxNodesTree, CompilationRequest request)
    {
        var method = new DynamicMethod(
            name: "CustomFunc",
            returnType: request.ReturnType,
            parameterTypes: request.Arguments.Select(_ => request.ReturnType).ToArray());

        var generator = method.GetILGenerator();
        generator.Emit(OpCodes.Ldarg_0);
        generator.Emit(OpCodes.Ldc_R8, 2.0);
        generator.Emit(OpCodes.Mul);
        generator.Emit(OpCodes.Ret);

        return method;
    }
}

public class FunctionCountingCompiler(IEnumerable<FunctionDefinition> functions) : IFunctionCompiler
{
    public DynamicMethod Compile(ISyntaxNode syntaxNodesTree, CompilationRequest request)
    {
        var method = new DynamicMethod(
            name: "CustomFunc",
            returnType: request.ReturnType,
            parameterTypes: Type.EmptyTypes);

        var generator = method.GetILGenerator();
        generator.Emit(OpCodes.Ldc_R8, (double)functions.Count());
        generator.Emit(OpCodes.Ret);

        return method;
    }
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

public class ModOperatorToken : OperatorToken
{
    public override int Priority => 0x20;
}

public class ModOperatorCompilationStrategy : IOperatorCompilationStrategy
{
    public Type OperatorType => typeof(ModOperatorToken);

    public void Compile(ILGenerator generator, CilCompilationOptions options)
    {
        generator.Emit(OpCodes.Rem);
    }
}

public class CustomAddOperatorToken : OperatorToken
{
    public override int Priority => 0x30;
}

public class CustomAddOperatorCompilationStrategy : IOperatorCompilationStrategy
{
    public Type OperatorType => typeof(CustomAddOperatorToken);

    public void Compile(ILGenerator generator, CilCompilationOptions options)
    {
        generator.Emit(OpCodes.Sub);
    }
}

public class SubtractingAddCompilationStrategy : IOperatorCompilationStrategy
{
    public Type OperatorType => typeof(AddOperatorToken);

    public void Compile(ILGenerator generator, CilCompilationOptions options)
    {
        generator.Emit(OpCodes.Sub);
    }
}

public class RightAssociativeSubToken : OperatorToken
{
    public override int Priority => 0x30;

    public override bool IsRightAssociative => true;
}

#endregion
