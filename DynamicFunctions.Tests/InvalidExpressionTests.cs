using DynamicFunctions.Compilation.Exceptions;
using DynamicFunctions.LexicalAnalysis.Exceptions;
using DynamicFunctions.TextAnalysis;

namespace DynamicFunctions.Tests;

public class InvalidExpressionTests
{
    [Theory]
    [InlineData("@")]
    [InlineData("2 & 3")]
    [InlineData("$x")]
    public void UnknownCharacter_ThrowsUnknownCharException(string expression)
    {
        Assert.Throws<UnknownCharException>(() =>
            DynamicFunction.Build(expression)
                .WithType<double>()
                .Create());
    }

    [Theory]
    [InlineData("(2 + 3")]
    [InlineData("((2 + 3)")]
    [InlineData("(((1))")]
    public void UnpairedBracket_ThrowsUnpairedBracketException(string expression)
    {
        Assert.Throws<UnpairedBracketException>(() =>
            DynamicFunction.Build(expression)
                .WithType<double>()
                .Create());
    }

    [Fact]
    public void UnknownVariable_ThrowsUnknownVariableException()
    {
        Assert.Throws<UnknownVariableException>(() =>
            DynamicFunction.Build("x + y")
                .WithType<double>()
                .Create("x"));
    }

    [Fact]
    public void UnknownFunction_ThrowsUnknownFunctionException()
    {
        Assert.Throws<UnknownFunctionException>(() =>
            DynamicFunction.Build("foo(1)")
                .WithType<double>()
                .Create());
    }

    [Fact]
    public void WrongArgumentCount_ThrowsUnexpectedArgsCountException()
    {
        Assert.Throws<UnexpectedArgsCountInFunctionException>(() =>
            DynamicFunction.Build("myabs(1, 2)")
                .WithType<double>(cfg => cfg
                    .AddFunctionDefinition("myabs", FunctionCompilationTests.Abs))
                .Create());
    }

    [Fact]
    public void UnsupportedType_ThrowsUnsupportedFunctionTypeException()
    {
        Assert.Throws<UnsupportedFunctionTypeException>(() =>
            DynamicFunction.Build("1 + 2")
                .WithType<int>()
                .Create());
    }

    [Theory]
    [InlineData("")]
    [InlineData("+ +")]
    [InlineData("* 2")]
    [InlineData("2 +")]
    public void InvalidStructure_ThrowsException(string expression)
    {
        Assert.ThrowsAny<Exception>(() =>
            DynamicFunction.Build(expression)
                .WithType<double>()
                .Create());
    }
}
