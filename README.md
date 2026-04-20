# DynamicFunctions

A .NET library for parsing and compiling mathematical expressions into executable delegates at runtime using CIL (Common Intermediate Language) emission.

## Features

- **Runtime expression compilation** - parse string expressions like `"x + y * 2"` and compile them into strongly-typed `Func<>` delegates
- **Variable support** - define named variables that become function parameters
- **Custom function definitions** - register your own functions to be called within expressions
- **Arithmetic operators** - addition (`+`), subtraction (`-`), multiplication (`*`), division (`/`), with correct operator precedence
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

## Requirements

- .NET 10.0+
