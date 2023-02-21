using System;

public abstract class Singleton<T> : IDisposable where T :class, new()
{
    private static T _instance;
    
    public static T I
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }

    ~Singleton()
    {
        Dispose();
    }

    public virtual void Dispose()
    {
        _instance = null;
    }
}
