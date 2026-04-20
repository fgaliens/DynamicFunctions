using System.Reflection.Emit;

namespace DynamicFunctions.Compilation;

public class CilCompilationOptions
{
    public required DynamicMethod DynamicMethod { get; init; }
    public required IReadOnlyDictionary<string, int> Arguments { get; init; }
    public required IReadOnlyDictionary<string, FunctionDefinition> Functions { get; init; }
    public required NumberType Type { get; init; }
}
