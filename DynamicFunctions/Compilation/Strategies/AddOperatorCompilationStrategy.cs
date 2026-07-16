using System.Reflection.Emit;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.Compilation.Strategies;

public class AddOperatorCompilationStrategy : IOperatorCompilationStrategy
{
    public Type OperatorType => typeof(AddOperatorToken);

    public void Compile(ILGenerator generator, CilCompilationOptions options)
    {
        generator.Emit(OpCodes.Add);
    }
}
