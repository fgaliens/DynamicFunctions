using System.Reflection.Emit;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.Compilation.Strategies;

public class MultOperatorCompilationStrategy : IOperatorCompilationStrategy
{
    public Type OperatorType => typeof(MultOperatorToken);

    public void Compile(ILGenerator generator, CilCompilationOptions options)
    {
        generator.Emit(OpCodes.Mul);
    }
}
