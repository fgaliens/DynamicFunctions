using System.Reflection.Emit;
using DynamicFunctions.Compilation;
using DynamicFunctions.LexicalAnalysis;
using DynamicFunctions.SyntaxAnalysis;
using DynamicFunctions.TextAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicFunctions;

public static class DynamicFunction
{
    public static Builder Build(string expression)
    {
        return new Builder(expression);
    }

    public class Builder(string expression)
    {
        public Builder<TFunc> WithType<TFunc>(Action<DynamicFunctionConfiguration<TFunc>>? cfg = null)
        {
            var serviceProvider = new ServiceCollection()
                .AddDynamicFunctions(cfg)
                .BuildServiceProvider();
            
            return new Builder<TFunc>(serviceProvider, expression);
        }
    }
    
    public class Builder<T>(IServiceProvider serviceProvider, string expression)
    {
        public Func<T> Create()
        {
            var dynamicMethod = BuildInternal();
            return dynamicMethod.CreateDelegate<Func<T>>();
        }
        
        public Func<T, T> Create(string arg)
        {
            var dynamicMethod = BuildInternal(arg);
            return dynamicMethod.CreateDelegate<Func<T, T>>();
        }
        
        public Func<T, T, T> Create(string arg1, string arg2)
        {
            var dynamicMethod = BuildInternal(arg1, arg2);
            return dynamicMethod.CreateDelegate<Func<T, T, T>>();
        }
        
        public Func<T, T, T, T> Create(string arg1, string arg2, string arg3)
        {
            var dynamicMethod = BuildInternal(arg1, arg2, arg3);
            return dynamicMethod.CreateDelegate<Func<T, T, T, T>>();
        }
        
        public Func<T, T, T, T, T> Create(string arg1, string arg2, string arg3, string arg4)
        {
            var dynamicMethod = BuildInternal(arg1, arg2, arg3, arg4);
            return dynamicMethod.CreateDelegate<Func<T, T, T, T, T>>();
        }

        private DynamicMethod BuildInternal(params string[] args)
        {
            var tokenizer = serviceProvider.GetRequiredService<ITokenizer>();
            var lexicalAnalyzer = serviceProvider.GetRequiredService<ILexicalAnalyzer>();
            var syntaxAnalyzer = serviceProvider.GetRequiredService<ISyntaxAnalyzer>();

            var tokens = tokenizer.Tokenize(expression);
            var lexicalTokens = lexicalAnalyzer.Analyze(tokens);
            var syntaxNodesTree = syntaxAnalyzer.Analyze(lexicalTokens);

            var dynamicMethod = new DynamicMethod(
                name: "DynamicFunc",
                returnType: typeof(T),
                parameterTypes: args.Select(_ => typeof(T)).ToArray());

            var compiler = serviceProvider.GetService<ICompiler>() ?? new CilCompiler(new CilCompilationOptions
            {
                DynamicMethod = dynamicMethod,
                Arguments = args
                    .Select((x, i) => (Index: i, Arg: x))
                    .ToDictionary(k => k.Arg, v => v.Index),
                Functions = serviceProvider.GetServices<FunctionDefinition>()
                    .ToDictionary(k => k.Name, v => v),
                Type = NumberType.Double,
            });

            syntaxNodesTree.Accept(compiler);
            compiler.Complete();

            return dynamicMethod;
        }
    }
}
