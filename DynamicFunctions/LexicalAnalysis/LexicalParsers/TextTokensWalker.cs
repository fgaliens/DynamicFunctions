using DynamicFunctions.TextAnalysis.Tokens;

namespace DynamicFunctions.LexicalAnalysis.LexicalParsers;

internal class TextTokensWalker(IEnumerable<TextToken> textTokens) : IDisposable, ITextTokensWalker
{
    private const int BufferCapacity = 5;
    
    private readonly IEnumerator<TextToken> _textTokensEnumerator = textTokens.GetEnumerator();
    private readonly Queue<TextToken> _buffer = new(capacity: BufferCapacity);
    private readonly Queue<TextToken> _tempBuffer = new(capacity: BufferCapacity);
    private bool _disposed = false;
    private bool _finished = false;

    public bool Finished => _finished && _buffer.Count == 0;

    public TextToken Current => GetCurrentToken();
    
    public bool TryGetNextToken(out TextToken token)
    {
        ThrowIfDisposed();

        if (_tempBuffer.TryDequeue(out token))
        {
            return true;
        }

        if (_textTokensEnumerator.MoveNext())
        {
            token = _textTokensEnumerator.Current;
            _buffer.Enqueue(token);
            
            return true;
        }
        
        _finished = true;

        token = default;
        return false;
    }

    public bool ReturnToPrevious()
    {
        throw new NotImplementedException();
    }

    public void Consume()
    {
        ThrowIfDisposed();
        
        _buffer.ReplaceWithValuesFrom(_tempBuffer);
    }

    public void Reset()
    {
        ThrowIfDisposed();
        
        _tempBuffer.ReplaceWithValuesFrom(_buffer);
        
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;
        _textTokensEnumerator.Dispose();
    }

    private TextToken GetCurrentToken()
    {
        if (_buffer.TryPeek(out var token))
        {
            return token;
        }
        
        if (_tempBuffer.TryPeek(out token))
        {
            return token;
        }
        
        return !_finished ? _textTokensEnumerator.Current : new TextToken
        {
            Index = 0,
            Length = 0,
            Type = "<End of expression>",
            Source = ""
        };
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }
}

file static class QueueExtensions
{
    public static void ReplaceWithValuesFrom<T>(this Queue<T> target, Queue<T> source)
    {
        target.Clear();
        target.EnsureCapacity(source.Count);
        
        foreach (var item in source)
        {
            target.Enqueue(item);
        }
    }
}