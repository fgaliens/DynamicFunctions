using System.Reflection.Emit;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.Compilation.Strategies;

public class SubOperatorCompilationStrategy : IOperatorCompilationStrategy
{
    public Type OperatorType => typeof(SubOperatorToken);

    public void Compile(ILGenerator generator, CilCompilationOptions options)
    {
        generator.Emit(OpCodes.Sub);
    }
}
