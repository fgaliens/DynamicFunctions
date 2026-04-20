using System.Reflection;

namespace DynamicFunctions.Compilation;

public class FunctionDefinition
{
    public required string Name { get; init; }
    public required int ArgsCount { get; init; }
    public required MethodInfo Method { get; init; }
}
