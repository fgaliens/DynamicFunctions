namespace DynamicFunctions.Tests;

public class FunctionCompilationTests
{
    public static double GetPi() => Math.PI;
    public static double Abs(double a) => Math.Abs(a);
    public static double Neg(double a) => -a;
    public static double Max(double a, double b) => Math.Max(a, b);

    [Fact]
    public void ZeroArgFunction_CompilesAndExecutes()
    {
        var func = DynamicFunction.Build("pi()")
            .WithType<double>(cfg => cfg
                .AddFunctionDefinition("pi", GetPi))
            .Create();

        Assert.Equal(Math.PI, func());
    }

    [Fact]
    public void SingleArgFunction_CompilesAndExecutes()
    {
        var func = DynamicFunction.Build("abs(x)")
            .WithType<double>(cfg => cfg
                .AddFunctionDefinition("abs", Abs))
            .Create("x");

        Assert.Equal(5.0, func(-5.0));
    }

    [Fact]
    public void TwoArgFunction_CompilesAndExecutes()
    {
        var func = DynamicFunction.Build("max(x, y)")
            .WithType<double>(cfg => cfg
                .AddFunctionDefinition("max", Max))
            .Create("x", "y");

        Assert.Equal(7.0, func(3.0, 7.0));
    }

    [Fact]
    public void FunctionInExpression_CompilesCorrectly()
    {
        var func = DynamicFunction.Build("abs(x) + 1")
            .WithType<double>(cfg => cfg
                .AddFunctionDefinition("abs", Abs))
            .Create("x");

        Assert.Equal(6.0, func(-5.0));
    }

    [Fact]
    public void NestedFunctionCalls_CompilesCorrectly()
    {
        var func = DynamicFunction.Build("abs(neg(x))")
            .WithType<double>(cfg => cfg
                .AddFunctionDefinition("abs", Abs)
                .AddFunctionDefinition("neg", Neg))
            .Create("x");

        Assert.Equal(5.0, func(5.0));
    }
}
