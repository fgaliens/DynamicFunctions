using System.Reflection.Emit;

namespace DynamicFunctions.Compilation.Strategies;

internal sealed class DelegateOperatorCompilationStrategy(Type operatorType, Action<ILGenerator> emit)
    : IOperatorCompilationStrategy
{
    public Type OperatorType => operatorType;

    public void Compile(ILGenerator generator, CilCompilationOptions options)
    {
        emit(generator);
    }
}
