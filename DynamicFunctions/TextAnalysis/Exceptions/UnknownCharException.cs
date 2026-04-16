namespace DynamicFunctions.TextAnalysis;

public class UnknownCharException : DynamicFunctionsException
{
    public required string SourceString { get; init; }
    public required char Character { get; init; }
    public required int Index { get; init; }
    
    public override string Message => $"Unknown character '{Character}' at index: {Index}";
}