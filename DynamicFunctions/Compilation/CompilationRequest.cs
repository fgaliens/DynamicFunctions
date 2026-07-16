namespace DynamicFunctions.Compilation;

public class CompilationRequest
{
    public required Type ReturnType { get; init; }
    public required IReadOnlyList<string> Arguments { get; init; }
}
