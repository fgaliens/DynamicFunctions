namespace DynamicFunctions.Tests;

public class OperatorPrecedenceTests
{
    [Theory]
    [InlineData("2 + 3 * 4", 14.0)]
    [InlineData("10 - 2 * 3", 4.0)]
    [InlineData("6 / 2 + 1", 4.0)]
    [InlineData("8 / 4 * 2", 4.0)]
    public void OperatorPrecedence_IsRespected(string expression, double expected)
    {
        var func = DynamicFunction.Build(expression)
            .WithType<double>()
            .Create();

        Assert.Equal(expected, func());
    }

    [Theory]
    [InlineData("(2 + 3) * 4", 20.0)]
    [InlineData("10 / (2 + 3)", 2.0)]
    [InlineData("(1 + 2) * (3 + 4)", 21.0)]
    [InlineData("((2 + 3))", 5.0)]
    public void Brackets_OverridePrecedence(string expression, double expected)
    {
        var func = DynamicFunction.Build(expression)
            .WithType<double>()
            .Create();

        Assert.Equal(expected, func());
    }

    [Theory]
    [InlineData("1 + 2 + 3", 6.0)]
    [InlineData("10 - 3 - 2", 5.0)]
    [InlineData("2 * 3 * 4", 24.0)]
    [InlineData("100 / 5 / 4", 5.0)]
    public void LeftAssociativity_IsRespected(string expression, double expected)
    {
        var func = DynamicFunction.Build(expression)
            .WithType<double>()
            .Create();

        Assert.Equal(expected, func());
    }
}
