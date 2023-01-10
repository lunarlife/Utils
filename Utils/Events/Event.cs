using System.Reflection;
using Utils.Exceptions;

namespace Utils.Events;

public delegate void EventHandler();
public delegate void EventHandler<in T>(T value) 
    where T : EventData;
public sealed class Event
{
    private readonly List<EventHandler> _handlers = new();
    private readonly object _lockObj = new();
    public void Invoke()
    {

        lock (_lockObj)
            for (var index = 0; index < _handlers.Count; index++)
            {
                var handler = _handlers[index];
                try
                {
                    handler.Invoke();
                }
                catch (TargetInvocationException e)
                {
                    var ex = e.InnerException;
                    while (true)
                    {
                        if (ex?.InnerException is not { } exception) break;
                        ex = exception;
                    }

                    throw ex;
                }
            }
    }
    
    public void AddListener(EventHandler handler)
    {
        lock (_lockObj)
        {
            if (_handlers.Contains(handler)) throw new EventException("handler already registered");
            _handlers.Add(handler);
        }
    }
    public void RemoveListener(EventHandler handler)
    {
        lock (_lockObj)
        {
            if (!_handlers.Contains(handler)) throw new EventException("handler is not registered");
            _handlers.Add(handler);
        }
    }
}

public sealed class Event<T> where T : EventData
{
    private readonly List<EventHandler<T>> _handlers = new();
    private readonly object _lockObj = new();

    public void Invoke(T value)
    {
        lock (_lockObj)
        {
            for (var index = 0; index < _handlers.Count; index++)
            {
                var handler = _handlers[index];
                try
                {
                    handler.Invoke(value);
                }
                catch (TargetInvocationException e)
                {
                    var ex = e.InnerException;
                    while (true)
                    {
                        if (ex?.InnerException is not { } exception) break;
                        ex = exception;
                    }

                    throw ex;
                }
            }

            EventManager.InvokeListeners(value);
        }
    }

    public void AddListener(EventHandler<T> handler)
    {
        lock (_lockObj)
        {
            if (_handlers.Contains(handler)) throw new EventException("handler already registered");
            _handlers.Add(handler);
        }
    }
    public void RemoveListener(EventHandler<T> handler)
    {
        lock (_lockObj)
        {
            if (!_handlers.Contains(handler)) throw new EventException("handler is not registered");
            _handlers.Add(handler);
        }
    }
}
