using System.Reflection.Emit;
using DynamicFunctions.LexicalAnalysis.LexicalTokens;

namespace DynamicFunctions.Compilation.Strategies;

public class DivOperatorCompilationStrategy : IOperatorCompilationStrategy
{
    public Type OperatorType => typeof(DivOperatorToken);

    public void Compile(ILGenerator generator, CilCompilationOptions options)
    {
        generator.Emit(OpCodes.Div);
    }
}
