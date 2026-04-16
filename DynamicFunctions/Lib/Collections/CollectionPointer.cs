namespace DynamicFunctions.Lib.Collections;

public readonly record struct CollectionPointer<T>
{
    private readonly IReadOnlyList<T> _source;
    private readonly int _index;
    
    private CollectionPointer(IReadOnlyList<T> source, int index)
    {
        _source = source;
        _index = index;
    }
    
    public CollectionPointer(IReadOnlyList<T> source) : this(source, -1)
    {
        _source = source;
        _index = -1;
    }
    
    public CollectionPointer() : this([])
    {
    }

    public T Value => HasValue 
        ? _source[_index] 
        : throw new InvalidOperationException("This collection pointer is out of range");
    
    public bool HasValue => IsIndexInRange();
    public bool HasNext => IsIndexInRange(_index + 1);
    public bool HasPrevious => IsIndexInRange(_index - 1);

    public CollectionPointer<T> Next()
    {
        if (!HasNext)
            throw new InvalidOperationException("Collection does not have next item");
        
        return new CollectionPointer<T>(_source, _index + 1);
    }

    public CollectionPointer<T> Previous()
    {
        if (!HasPrevious)
            throw new InvalidOperationException("Collection does not have previous item");
        
        return new CollectionPointer<T>(_source, _index - 1);
    }

    private bool IsIndexInRange(int? index = null)
    {
        var indexValue = index ?? _index;
        return indexValue >= 0 && indexValue < _source.Count;
    }
}