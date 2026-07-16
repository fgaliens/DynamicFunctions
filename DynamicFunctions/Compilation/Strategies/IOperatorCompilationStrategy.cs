using System.Reflection.Emit;

namespace DynamicFunctions.Compilation.Strategies;

public interface IOperatorCompilationStrategy
{
    Type OperatorType { get; }

    void Compile(ILGenerator generator, CilCompilationOptions options);
}
