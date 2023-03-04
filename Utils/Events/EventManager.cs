using System.Reflection;
using Utils.Exceptions;

namespace Utils.Events;

public static class EventManager
{
    private static readonly Dictionary<Type, List<Delegate>> Handlers = new();
    private static readonly object HandlersLock = new();
    private static List<IEventListener> _listeners = new();
    internal static void InvokeListeners(EventData data)
    {
        var type = data.GetType();
        lock (HandlersLock)
        {
            if (!Handlers.ContainsKey(type)) return;
            var delegates = Handlers[type];
            for (var index = 0; index < delegates.Count; index++)
            {
                var handler = delegates[index];
                try
                {
                    handler.DynamicInvoke(data);
                }
                catch (TargetInvocationException e)
                {
                    var ex = e.InnerException;
                    while (ex is not null)
                    {
                        Console.WriteLine(ex);
                        ex = ex?.InnerException;
                    }
                }
            }
        }
    }

    public static void RegisterEvent<T>(EventHandler<T> handler) where T : EventData
    {
        var type = typeof(T);
        lock (HandlersLock)
        {
            if (Handlers.ContainsKey(type))
            {
                var list = Handlers[type];
                if (list.Contains(handler)) throw new EventException("handler already registered");
                list.Add(handler);
            }
            else
                Handlers.Add(type, new List<Delegate> { handler });
        }
    }
    public static void RegisterEvent(Type type, EventHandler<EventData> handler)
    {
        lock (HandlersLock)
        {
            if (Handlers.ContainsKey(type))
            {
                var list = Handlers[type];
                if (list.Contains(handler)) throw new EventException("handler already registered");
                list.Add(handler);
            }
            else
                Handlers.Add(type, new List<Delegate> { handler });
        }
    }
    public static void UnregisterEvent<T>(EventHandler<T> handler) where T : EventData
    {
        var type = typeof(T);
        lock (HandlersLock)
        {
            if (!Handlers.ContainsKey(type))
                throw new EventException("handler is not registered");
            var list = Handlers[type];
            if (!list.Contains(handler)) throw new EventException("handler is not registered");
            list.Remove(handler);
        }
    }
    public static void UnregisterEvent(Type type, EventHandler<EventData> handler)
    {
        lock (HandlersLock)
        {
            if (!Handlers.ContainsKey(type))
                throw new EventException("handler is not registered");
            var list = Handlers[type];
            if (!list.Contains(handler)) throw new EventException("handler is not registered");
            list.Remove(handler);
        }
    }
    public static void UnregisterEvents(IEventListener listener)
    {
        lock (Handlers)
        {
            if (!_listeners.Contains(listener))
                throw new EventException("listener is not registered");
            foreach (var delegates in Handlers.Values)
                for (var i = delegates.Count - 1; i >= 0; i--)
                    if (delegates[i].Target == listener)
                        delegates.RemoveAt(i);
        }
    }

    public static void RegisterEvents(IEventListener listener) => RegisterEvents(listener.GetType(), listener);
    public static void RegisterStaticEvents(Type type) => RegisterEvents(type, null);

    private static void RegisterEvents(Type listenerType, IEventListener? listener)
    {
       
        var flags = BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public;
        var isStatic = listener is null;
        if (isStatic)
            flags |= BindingFlags.Static;
        else
            flags |= BindingFlags.Instance;
        var baseType = listenerType;
        lock (HandlersLock)
        {
            if (listener is not null && _listeners.Contains(listener))
                throw new EventException("listener already registered");
            while (baseType != null)
            {
                foreach (var method in baseType.GetMethods(flags))
                {
                    var arguments = method.GetParameters();
                    if (method.GetCustomAttributes().FirstOrDefault(att => att is EventHandlerAttribute) is
                            not { } || arguments.Length != 1 ||
                        !typeof(EventData).IsAssignableFrom(arguments[0].ParameterType)) continue;
                    var eventType = arguments[0].ParameterType;

                    void HandlerInvoke(EventData data) => method.Invoke(listener, new object[] { data });
                    if (Handlers.ContainsKey(eventType))
                    {
                        var list = Handlers[eventType];
                        if (list.Contains(HandlerInvoke)) throw new EventException("handler already registered");
                        list.Add(HandlerInvoke);
                    }
                    else
                        Handlers.Add(eventType, new List<Delegate> { HandlerInvoke });
                }
                if (isStatic) break;
                baseType = baseType.BaseType;
            }
            if(listener is not null)
                _listeners.Add(listener);
        }
    }
}