# DynamicFunctions

A .NET library for parsing and compiling mathematical expressions into executable delegates at runtime using CIL (Common Intermediate Language) emission.

## Features

- **Runtime expression compilation** - parse string expressions like `"x + y * 2"` and compile them into strongly-typed `Func<>` delegates
- **Variable support** - define named variables that become function parameters
- **Custom function definitions** - register your own functions to be called within expressions
- **Arithmetic operators** - addition (`+`), subtraction (`-`), multiplication (`*`), division (`/`), power (`^`), with correct operator precedence and associativity
- **Custom operators** - register your own operators with their own precedence and associativity
- **Parentheses grouping** - override default precedence with brackets
- **Numeric types** - supports `double` and `long` result types
- **Extensible pipeline** - plug in custom text parsers, lexical parsers, syntax analyzers, and compilers
- **CIL compilation** - expressions compile directly to IL, providing near-native execution performance

## Warning

> **This library is in early development (v0.0.1).** The public API is subject to breaking changes in future versions without prior notice. Do not rely on the current API surface for production use without pinning a specific version.

## Usage

### Basic arithmetic

```csharp
var func = DynamicFunction.Build("2 + 3 * 4")
    .WithType<double>()
    .Create();

Console.WriteLine(func()); // 14.0
```

### Variables

```csharp
var func = DynamicFunction.Build("x * x + x")
    .WithType<double>()
    .Create("x");

Console.WriteLine(func(3.0)); // 12.0
```

```csharp
var func = DynamicFunction.Build("x + y")
    .WithType<double>()
    .Create("x", "y");

Console.WriteLine(func(3.0, 5.0)); // 8.0
```

### Custom functions

```csharp
static double Abs(double a) => Math.Abs(a);
static double Max(double a, double b) => Math.Max(a, b);

var func = DynamicFunction.Build("abs(x - y) + max(x, y)")
    .WithType<double>(cfg => cfg
        .AddFunctionDefinition("abs", Abs)
        .AddFunctionDefinition("max", Max))
    .Create("x", "y");

Console.WriteLine(func(3.0, 7.0)); // 11.0
```

### Zero-argument functions

```csharp
static double GetPi() => Math.PI;

var func = DynamicFunction.Build("pi() * 2")
    .WithType<double>(cfg => cfg
        .AddFunctionDefinition("pi", GetPi))
    .Create();

Console.WriteLine(func()); // 6.283185307179586
```

### Nested function calls

```csharp
static double Abs(double a) => Math.Abs(a);
static double Neg(double a) => -a;

var func = DynamicFunction.Build("abs(neg(x))")
    .WithType<double>(cfg => cfg
        .AddFunctionDefinition("abs", Abs)
        .AddFunctionDefinition("neg", Neg))
    .Create("x");

Console.WriteLine(func(5.0)); // 5.0
```

### Using `long` type

```csharp
var func = DynamicFunction.Build("x + y")
    .WithType<long>()
    .Create("x", "y");

Console.WriteLine(func(3L, 5L)); // 8
```

### Custom operators

Define an operator token carrying precedence (lower `Priority` value binds tighter) and associativity, then register it with a symbol and the IL to emit (both operands are already on the stack):

```csharp
class ModOperatorToken : OperatorToken
{
    public override int Priority => 0x20; // same tier as * and /
    public override bool IsRightAssociative => false;
}

var func = DynamicFunction.Build("1 + 7 % 4")
    .WithType<double>(cfg => cfg
        .AddOperator<ModOperatorToken>("%", il => il.Emit(OpCodes.Rem)))
    .Create();

Console.WriteLine(func()); // 4.0
```

Each pipeline stage can also be extended separately:

- `AddOperatorDefinition(tokenType, factory)` - map a text token type to an operator token (overrides built-ins for the same token type)
- `AddCompilationStrategy<T>()` - register an `IOperatorCompilationStrategy` that emits IL for an operator token type (overrides built-ins for the same operator)

### Extensibility

You can extend the parsing and compilation pipeline through configuration:

```csharp
var func = DynamicFunction.Build("2 + 3")
    .WithType<double>(cfg => cfg
        .AddTextParser<MyCustomTextParser>()
        .AddLexicalParser<MyCustomLexicalParser>()
        .AddSyntaxContextAnalyzer<MyCustomAnalyzer>()
        .AddCompiler<MyCustomCompiler>())
    .Create();
```

`AddCompiler<T>` replaces the whole compilation stage. `T` implements `IFunctionCompiler`: it receives the syntax tree and a `CompilationRequest` (return type, argument names) and returns a ready `DynamicMethod`. Services registered in the pipeline (function definitions, operator compilation strategies, etc.) can be injected through its constructor.

## Requirements

- .NET 10.0+
