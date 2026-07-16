using System.Reflection.Emit;
using DynamicFunctions.Compilation;
using DynamicFunctions.Compilation.Exceptions;
using DynamicFunctions.Compilation.Extensions;
using DynamicFunctions.Compilation.Strategies;
using DynamicFunctions.LexicalAnalysis.Extensions;
using DynamicFunctions.LexicalAnalysis.LexicalParsers;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.SyntaxAnalysis.ContextAnalysis;
using DynamicFunctions.SyntaxAnalysis.Extensions;
using DynamicFunctions.TextAnalysis.Extensions;
using DynamicFunctions.TextAnalysis.Parsing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DynamicFunctions;

internal static class DynamicFunctionsExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDynamicFunctions<TFunction>(Action<DynamicFunctionConfiguration<TFunction>>? cfg = null)
        {
            if (typeof(TFunction) != typeof(long)
                && typeof(TFunction) != typeof(double))
            {
                throw new UnsupportedFunctionTypeException(typeof(TFunction).Name);
            }
            
            if (cfg is not null)
            {
                var configuration = new DynamicFunctionConfiguration<TFunction>(services);
                cfg(configuration);
            }

            return services
                .AddTextAnalysis()
                .AddLexicalAnalysis()
                .AddSyntaxAnalysis()
                .AddCompilation();
        }
    }
}

public class DynamicFunctionConfiguration<TFunc>(IServiceCollection services)
{
    public DynamicFunctionConfiguration<TFunc> AddTextParser<T>() where T : class, ITextParser
    {
        services.AddTextParser<T>();
        return this;
    }
    
    public DynamicFunctionConfiguration<TFunc> AddLexicalParser<T>() where T : class, ILexicalParser
    {
        services.AddLexicalParser<T>();
        return this;
    }
    
    public DynamicFunctionConfiguration<TFunc> AddSyntaxContextAnalyzer<T>() where T : class, ISyntaxContextAnalyzer
    {
        services.AddSyntaxContextAnalyzer<T>();
        return this;
    }
    
    public DynamicFunctionConfiguration<TFunc> AddOperatorDefinition(string tokenType, Func<OperatorToken> factory)
    {
        services.AddOperatorDefinition(tokenType, factory);
        return this;
    }

    public DynamicFunctionConfiguration<TFunc> AddCompilationStrategy<T>() where T : class, IOperatorCompilationStrategy
    {
        services.AddCompilationStrategy<T>();
        return this;
    }

    /// <summary>
    /// Registers a custom binary operator across the whole pipeline:
    /// a text parser for <paramref name="symbol"/>, an operator definition producing
    /// <typeparamref name="TToken"/> and a compilation strategy emitting <paramref name="emit"/>.
    /// Precedence and associativity are taken from the token's
    /// <see cref="OperatorToken.Priority"/> and <see cref="OperatorToken.IsRightAssociative"/>.
    /// </summary>
    public DynamicFunctionConfiguration<TFunc> AddOperator<TToken>(string symbol, Action<ILGenerator> emit)
        where TToken : OperatorToken, new()
    {
        var tokenType = typeof(TToken).Name;

        services.AddSingleton<ITextParser>(new SymbolTextParser(symbol, tokenType));
        services.AddOperatorDefinition(tokenType, () => new TToken());
        services.AddSingleton<IOperatorCompilationStrategy>(
            new DelegateOperatorCompilationStrategy(typeof(TToken), emit));

        return this;
    }

    public DynamicFunctionConfiguration<TFunc> AddCompiler<T>() where T : class, IFunctionCompiler
    {
        services.TryAddSingleton<IFunctionCompiler, T>();
        return this;
    }

    public DynamicFunctionConfiguration<TFunc> AddFunctionDefinition(string functionName, Func<TFunc> function)
    {
        var functionDefinition = new FunctionDefinition
        {
            Name = functionName,
            ArgsCount = 0,
            Method = function.Method,
        };

        services.AddSingleton(functionDefinition);
        return this;
    }
    
    public DynamicFunctionConfiguration<TFunc> AddFunctionDefinition(string functionName, Func<TFunc, TFunc> function)
    {
        var functionDefinition = new FunctionDefinition
        {
            Name = functionName,
            ArgsCount = 1,
            Method = function.Method,
        };

        services.AddSingleton(functionDefinition);
        return this;
    }
    
    public DynamicFunctionConfiguration<TFunc> AddFunctionDefinition(string functionName, Func<TFunc, TFunc, TFunc> function)
    {
        var functionDefinition = new FunctionDefinition
        {
            Name = functionName,
            ArgsCount = 2,
            Method = function.Method,
        };

        services.AddSingleton(functionDefinition);
        return this;
    }
    
    public DynamicFunctionConfiguration<TFunc> AddFunctionDefinition(string functionName, Func<TFunc, TFunc, TFunc, TFunc> function)
    {
        var functionDefinition = new FunctionDefinition
        {
            Name = functionName,
            ArgsCount = 3,
            Method = function.Method,
        };

        services.AddSingleton(functionDefinition);
        return this;
    }
    
    public DynamicFunctionConfiguration<TFunc> AddFunctionDefinition(string functionName, Func<TFunc, TFunc, TFunc, TFunc, TFunc> function)
    {
        var functionDefinition = new FunctionDefinition
        {
            Name = functionName,
            ArgsCount = 4,
            Method = function.Method,
        };

        services.AddSingleton(functionDefinition);
        return this;
    }
}