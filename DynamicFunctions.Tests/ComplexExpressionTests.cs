namespace DynamicFunctions.Tests;

public class ComplexExpressionTests
{
    [Theory]
    [InlineData("1 + 2 * 3 - 4 / 2", 5.0)]
    [InlineData("(1 + 2) * (3 - 1)", 6.0)]
    [InlineData("2 * (3 + 4) - 1", 13.0)]
    [InlineData("10 / (2 + 3) * 4", 8.0)]
    public void ComplexArithmetic_CompilesCorrectly(string expression, double expected)
    {
        var func = DynamicFunction.Build(expression)
            .WithType<double>()
            .Create();

        Assert.Equal(expected, func());
    }

    [Fact]
    public void ComplexExpressionWithVariablesAndFunctions()
    {
        var func = DynamicFunction.Build("abs(x - y) + max(x, y)")
            .WithType<double>(cfg => cfg
                .AddFunctionDefinition("abs", FunctionCompilationTests.Abs)
                .AddFunctionDefinition("max", FunctionCompilationTests.Max))
            .Create("x", "y");

        Assert.Equal(11.0, func(3.0, 7.0));
    }

    [Fact]
    public void ConstantExpression_NoVariables()
    {
        var func = DynamicFunction.Build("42")
            .WithType<double>()
            .Create();

        Assert.Equal(42.0, func());
    }

    [Fact]
    public void NegativeNumbers_InExpression()
    {
        var func = DynamicFunction.Build("0 - 5 + 3")
            .WithType<double>()
            .Create();

        Assert.Equal(-2.0, func());
    }
}
