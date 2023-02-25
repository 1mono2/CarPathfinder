using UniRx;

public class Counter
{
    private readonly IntReactiveProperty _count;
    public IReadOnlyReactiveProperty<int> Count => _count;
    
    public Counter()
    {
        _count = new IntReactiveProperty(0);
    }
    
    public Counter(int count)
    {
        _count = new IntReactiveProperty(count);
    }
    
    public void  AddCount(int count)
    {
        _count.Value += count;
    }
    
    public void SubCount(int count)
    {
        _count.Value -= count;
    }
    
    public int GetCount()
    {
        return _count.Value;
    }
}
