using DynamicFunctions.Compilation.Exceptions;

namespace DynamicFunctions.Tests;

public class OperatorCompilationTests
{
    [Theory]
    [InlineData("2 + 3", 5.0)]
    [InlineData("10 + 0", 10.0)]
    [InlineData("1.5 + 2.5", 4.0)]
    [InlineData("0 + 0", 0.0)]
    public void Addition_CompilesAndExecutesCorrectly(string expression, double expected)
    {
        var func = DynamicFunction.Build(expression)
            .WithType<double>()
            .Create();

        Assert.Equal(expected, func());
    }

    [Theory]
    [InlineData("10 - 3", 7.0)]
    [InlineData("0 - 5", -5.0)]
    [InlineData("3.5 - 1.5", 2.0)]
    [InlineData("5 - 5", 0.0)]
    public void Subtraction_CompilesAndExecutesCorrectly(string expression, double expected)
    {
        var func = DynamicFunction.Build(expression)
            .WithType<double>()
            .Create();

        Assert.Equal(expected, func());
    }

    [Theory]
    [InlineData("3 * 4", 12.0)]
    [InlineData("0 * 100", 0.0)]
    [InlineData("2.5 * 2", 5.0)]
    [InlineData("1 * 1", 1.0)]
    public void Multiplication_CompilesAndExecutesCorrectly(string expression, double expected)
    {
        var func = DynamicFunction.Build(expression)
            .WithType<double>()
            .Create();

        Assert.Equal(expected, func());
    }

    [Theory]
    [InlineData("10 / 2", 5.0)]
    [InlineData("7 / 2", 3.5)]
    [InlineData("0 / 5", 0.0)]
    [InlineData("1 / 4", 0.25)]
    public void Division_CompilesAndExecutesCorrectly(string expression, double expected)
    {
        var func = DynamicFunction.Build(expression)
            .WithType<double>()
            .Create();

        Assert.Equal(expected, func());
    }

    [Theory]
    [InlineData("2 ^ 3")]
    [InlineData("3 ^ 2")]
    [InlineData("2 ^ 0")]
    [InlineData("4 ^ 0.5")]
    public void Power_ThrowsUnsupportedOperatorException(string expression)
    {
        // Power operator is parsed but not yet implemented in CilCompiler
        Assert.Throws<UnsupportedOperatorException>(() =>
            DynamicFunction.Build(expression)
                .WithType<double>()
                .Create());
    }
}
