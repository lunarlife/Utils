using System.Collections;
using Utils.Exceptions;

namespace Utils.Pool;

public class ObjectPool<T> where T : class 
{
    private readonly Func<T> _instancer;
    public bool IsExpandable { get; }
    private readonly List<T> _pooleds = new();
    private readonly object _lock = new();

    public ObjectPool(bool isExpandable, Func<T> instancer)
    {
        _instancer = instancer;
        IsExpandable = isExpandable;
    }
    public ObjectPool()
    {
    }
    public T Take()
    {
        lock (_lock)
        {
            if (_pooleds.Count == 0)
            {
                if(IsExpandable)
                    _pooleds.Add(_instancer!.Invoke());
                else 
                    throw new PoolException("pool is empty");
            }
            var p = _pooleds.Last()!;
            _pooleds.Remove(p);
            return p;
        }
    }
    public bool TryTake(out T take)
    {
        lock (_lock)
        {
            if (_pooleds.Count == 0)
            {
                if(IsExpandable)
                    _pooleds.Add(_instancer!.Invoke());
                else
                {
                    take = null;
                    return false;
                }
            }
            take = _pooleds[^1];
            _pooleds.Remove(take);
            return true;
        }
    }
    public bool TryTake(int count, out T[] take)
    {
        lock (_lock)
        {
            if (_pooleds.Count < count)
            {
                if (IsExpandable)
                {
                    _pooleds.AddRange(Enumerable.Repeat(_instancer!.Invoke(), count - _pooleds.Count));
                }
                else
                {
                    take = null;
                    return false;
                }
            }
            take = _pooleds.TakeLast(count).ToArray();
            _pooleds.RemoveRange(_pooleds.Count - count, count);
            return true;
        }
    }

    public void Return(T obj)
    {
        lock (_lock)
        {
            if (_pooleds.Contains(obj)) throw new PoolException("object contains in pool");
            _pooleds.Add(obj);
        }
    }
    public void Return(IEnumerable<T> objects)
    {
        lock (_lock)
        {
            var objs = objects as T[] ?? objects.ToArray();
            if (objs.Any(o => _pooleds.Contains(o))) throw new PoolException("any object contains in pool");
            _pooleds.AddRange(objs);
        }
    }
}