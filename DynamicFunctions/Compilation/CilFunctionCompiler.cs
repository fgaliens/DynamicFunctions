using System.Reflection.Emit;
using DynamicFunctions.Compilation.Exceptions;
using DynamicFunctions.Compilation.Strategies;
using DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

namespace DynamicFunctions.Compilation;

public class CilFunctionCompiler(
    IEnumerable<FunctionDefinition> functions,
    IEnumerable<IOperatorCompilationStrategy> operatorStrategies) : IFunctionCompiler
{
    private readonly Dictionary<string, FunctionDefinition> _functions =
        functions.ToDictionary(k => k.Name, v => v);

    private readonly Dictionary<Type, IOperatorCompilationStrategy> _operators =
        BuildOperatorsMap(operatorStrategies);

    public DynamicMethod Compile(ISyntaxNode syntaxNodesTree, CompilationRequest request)
    {
        var dynamicMethod = new DynamicMethod(
            name: "DynamicFunc",
            returnType: request.ReturnType,
            parameterTypes: request.Arguments.Select(_ => request.ReturnType).ToArray());

        var compiler = new CilCompiler(new CilCompilationOptions
        {
            DynamicMethod = dynamicMethod,
            Arguments = request.Arguments
                .Select((x, i) => (Index: i, Arg: x))
                .ToDictionary(k => k.Arg, v => v.Index),
            Functions = _functions,
            Operators = _operators,
            Type = GetNumberType(request.ReturnType),
        });

        syntaxNodesTree.Accept(compiler);
        compiler.Complete();

        return dynamicMethod;
    }

    private static NumberType GetNumberType(Type returnType)
    {
        if (returnType == typeof(long))
        {
            return NumberType.Long;
        }

        if (returnType == typeof(double))
        {
            return NumberType.Double;
        }

        throw new UnsupportedFunctionTypeException(returnType.Name);
    }

    private static Dictionary<Type, IOperatorCompilationStrategy> BuildOperatorsMap(
        IEnumerable<IOperatorCompilationStrategy> strategies)
    {
        var operators = new Dictionary<Type, IOperatorCompilationStrategy>();

        foreach (var strategy in strategies)
        {
            operators.TryAdd(strategy.OperatorType, strategy);
        }

        return operators;
    }
}
