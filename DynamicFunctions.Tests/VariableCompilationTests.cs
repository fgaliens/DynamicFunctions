namespace DynamicFunctions.Tests;

public class VariableCompilationTests
{
    [Fact]
    public void SingleVariable_CompilesAndExecutesCorrectly()
    {
        var func = DynamicFunction.Build("x + 1")
            .WithType<double>()
            .Create("x");

        Assert.Equal(6.0, func(5.0));
    }

    [Fact]
    public void TwoVariables_CompilesAndExecutesCorrectly()
    {
        var func = DynamicFunction.Build("x + y")
            .WithType<double>()
            .Create("x", "y");

        Assert.Equal(8.0, func(3.0, 5.0));
    }

    [Fact]
    public void ThreeVariables_CompilesAndExecutesCorrectly()
    {
        var func = DynamicFunction.Build("x + y + z")
            .WithType<double>()
            .Create("x", "y", "z");

        Assert.Equal(6.0, func(1.0, 2.0, 3.0));
    }

    [Fact]
    public void FourVariables_CompilesAndExecutesCorrectly()
    {
        var func = DynamicFunction.Build("a + b + c + d")
            .WithType<double>()
            .Create("a", "b", "c", "d");

        Assert.Equal(10.0, func(1.0, 2.0, 3.0, 4.0));
    }

    [Fact]
    public void VariableUsedMultipleTimes_CompilesCorrectly()
    {
        var func = DynamicFunction.Build("x * x + x")
            .WithType<double>()
            .Create("x");

        Assert.Equal(12.0, func(3.0));
    }
}
