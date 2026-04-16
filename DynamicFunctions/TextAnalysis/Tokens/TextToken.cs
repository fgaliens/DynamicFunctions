namespace DynamicFunctions.TextAnalysis.Tokens;

public readonly record struct TextToken
{
    public required int Index { get; init; }
    public required int Length { get; init; }
    public required string Type { get; init; }
    public required string Source { get; init; }
    
    public ReadOnlySpan<char> Text => Source.AsSpan(Index, Length);
}