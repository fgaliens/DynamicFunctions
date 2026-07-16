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
    [InlineData("2 ^ 3", 8.0)]
    [InlineData("3 ^ 2", 9.0)]
    [InlineData("2 ^ 0", 1.0)]
    [InlineData("4 ^ 0.5", 2.0)]
    public void Power_CompilesAndExecutesCorrectly(string expression, double expected)
    {
        var func = DynamicFunction.Build(expression)
            .WithType<double>()
            .Create();

        Assert.Equal(expected, func());
    }

    [Fact]
    public void Power_IsRightAssociative()
    {
        var func = DynamicFunction.Build("2 ^ 3 ^ 2")
            .WithType<double>()
            .Create();

        // 2 ^ (3 ^ 2) = 512, not (2 ^ 3) ^ 2 = 64
        Assert.Equal(512.0, func());
    }

    [Theory]
    [InlineData("2 + 3", 5L)]
    [InlineData("10 - 3", 7L)]
    [InlineData("3 * 4", 12L)]
    [InlineData("7 / 2", 3L)]
    public void LongType_CompilesAndExecutesCorrectly(string expression, long expected)
    {
        var func = DynamicFunction.Build(expression)
            .WithType<long>()
            .Create();

        Assert.Equal(expected, func());
    }

    [Theory]
    [InlineData("2 ^ 3", 8L)]
    [InlineData("2 ^ 0", 1L)]
    [InlineData("2 ^ -1", 0L)]
    [InlineData("3 ^ 39", 4052555153018976267L)]
    public void LongType_Power_CompilesAndExecutesCorrectly(string expression, long expected)
    {
        // 3 ^ 39 exceeds double precision, so the result must be computed in longs
        var func = DynamicFunction.Build(expression)
            .WithType<long>()
            .Create();

        Assert.Equal(expected, func());
    }

    [Fact]
    public void LongType_Power_IsRightAssociative()
    {
        var func = DynamicFunction.Build("2 ^ 3 ^ 2")
            .WithType<long>()
            .Create();

        Assert.Equal(512L, func());
    }
}
