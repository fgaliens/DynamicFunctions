using System.Reflection.Emit;
using DynamicFunctions.Compilation.Exceptions;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

namespace DynamicFunctions.Compilation;

public class CilCompiler(CilCompilationOptions options) : ICompiler
{
    private readonly ILGenerator _generator = options.DynamicMethod.GetILGenerator();

    public void CompileNode(ConstantNode node)
    {
        switch (options.Type)
        {
            case NumberType.Long:
                _generator.Emit(OpCodes.Ldc_I8, node.Token.LongValue);
                break;
            case NumberType.Double:
                _generator.Emit(OpCodes.Ldc_R8, node.Token.DoubleValue);
                break;
            default:
                throw new UnsupportedFunctionTypeException(options.Type.ToString());
        }
    }

    public void CompileNode(VariableNode node)
    {
        var variableName = node.Token.Name;
        if (!options.Arguments.TryGetValue(variableName, out var index))
        {
            throw new UnknownVariableException(variableName);
        }
        
        _generator.Emit(OpCodes.Ldarg, index);
    }

    public void CompileNode(BinaryOperatorNode node)
    {
        var opCode = node.Operator switch
        {
            AddOperatorToken => OpCodes.Add,
            SubOperatorToken => OpCodes.Sub,
            MultOperatorToken => OpCodes.Mul,
            DivOperatorToken => OpCodes.Div,
            _ => throw new UnsupportedOperatorException(node.Operator.GetType().Name),
        };
        
        _generator.Emit(opCode);
    }

    public void CompileNode(FunctionCallNode node)
    {
        var functionName = node.Token.Name;
        if (!options.Functions.TryGetValue(functionName, out var functionDefinition))
        {
            throw new UnknownFunctionException(functionName);
        }

        if (functionDefinition.ArgsCount != node.Arguments.Count)
        {
            throw new UnexpectedArgsCountInFunctionException(functionName);
        }
        
        _generator.Emit(OpCodes.Call, functionDefinition.Method);
    }

    public void Complete()
    {
        _generator.Emit(OpCodes.Ret);
    }


}
