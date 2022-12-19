using System.Collections;
using System.Reflection;
using Utils.Exceptions;

namespace Utils.Events
{
    public static class EventManager
    {
        private static Dictionary<Type, List<EventMethod>> _methods = new();
        private static List<object> _instanceValidateListeners = new();
        private static List<Type> _staticValidateListeners = new();
        private static object _regLock = new();

        public static bool IsRegistered(this object listener) => _instanceValidateListeners.Contains(listener);
        public static bool IsRegisteredListener(object listener) => _instanceValidateListeners.Contains(listener);
        public static void RegisterEvent<T>(this IEventCaller caller, Action<T> method) where T : Event => RegisterEventFromCaller(caller, method);
        public static void RegisterEvent<T>(IEventCaller<T> caller, Action<T> method) where T : Event => RegisterEventFromCaller(caller, method);
        public static void RegisterEventFromCaller<T>(IEventCaller caller, Action<T> method) where T : Event => AddMethodToListeners(method.Target, method.Method, typeof(T), null, null, caller);
        public static void RegisterEventFromCaller<T>(IEventCaller<T> caller, Action<T> method) where T : Event => AddMethodToListeners(method.Target, method.Method, typeof(T), null, null, caller);

        public static async void RegisterListeners(params object[] listeners) => await AddListeners(listeners.Select(l => l.GetType()).ToArray(), listeners);
        public static async void RegisterListener(this object o) => await AddListeners(new[] { o.GetType() }, new[] { o });


        public static void RegisterEvents(object o) => RegisterListeners(o);


        private static async Task AddListeners(Type[] listenerTypes, IList<object> listenerObjects)
        {

            var add = new Dictionary<Type, List<EventMethod>>();
            foreach (var zip in listenerTypes.Zip(listenerObjects))
            {
                var isStatic = zip.Value2 is null;
                if (isStatic)
                {
                    if (_staticValidateListeners.Contains(zip.Value1))
                        throw new EventException("listener already registered");
                }
                else if (_instanceValidateListeners.Contains(zip.Value2))
                    throw new EventException("listener already registered");

                var flags = BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public;
                if (isStatic)
                    flags |= BindingFlags.Static;
                else
                    flags |= BindingFlags.Instance;
                var baseType = zip.Value1;
                while (baseType != null)
                {
                    foreach (var method in baseType.GetMethods(flags))
                    {
                        var arguments = method.GetParameters();
                        if (method.GetCustomAttributes().FirstOrDefault(att => att is EventHandlerAttribute) is
                                not { } attribute || arguments.Length != 1 ||
                            !arguments[0].ParameterType.IsSubclassOf(typeof(Event))) continue;
                        var eventType = arguments[0].ParameterType;
                        var options = method.GetCustomAttributes().FirstOrDefault(att => att is CallOptionsAttribute) as
                            CallOptionsAttribute;
                        var em = new EventMethod
                        {
                            Listener = zip.Value2,
                            Method = method,
                            Priority = (attribute as EventHandlerAttribute)!.Priority,
                            CallOption = options?.Option ?? CallOption.None,
                            Caller = null,
                            ListenerType = zip.Value1
                        };
                        if (add.ContainsKey(eventType)) add[eventType].Add(em);
                        else
                            add.Add(eventType, new List<EventMethod> { em });
                    }

                    if (isStatic) break;
                    baseType = baseType.BaseType;
                }

                if (isStatic)
                    _staticValidateListeners.Add(zip.Value1);
                else
                    _instanceValidateListeners.Add(zip.Value2!);
            }
            foreach (var (type, methods) in add)
            {
                List<EventMethod> list;
                lock (_regLock)
                {
                    if (_methods.ContainsKey(type))
                    {
                        list = _methods[type];
                        list.AddRange(methods);
                    }
                    else _methods.Add(type, list = methods);
                }
                await Task.Run(() =>
                {
                    lock (_regLock)
                        list.Sort();
                });
            }
            add.Clear();
        }

        private static void AddMethodToListeners(object listener, MethodInfo method, Type eventType, EventHandlerAttribute eventHandler, CallOptionsAttribute callOptions, object caller)
        {
            lock (_regLock)
            {
                List<EventMethod> eventMethods;
                if (_methods.ContainsKey(eventType)) eventMethods = _methods[eventType];
                else
                    _methods.Add(eventType, eventMethods = new List<EventMethod>());
                eventMethods.Add(new EventMethod
                {
                    Listener = listener,
                    Method = method,
                    Priority = eventHandler?.Priority ?? Priority.Normal,
                    CallOption = callOptions?.Option ?? CallOption.None,
                    Caller = caller,
                    ListenerType = listener.GetType()
                });
                eventMethods.Sort();
            }
        }
        
        public static void UnregisterEvents(object listener)
        {
            if (!_instanceValidateListeners.Contains(listener)) throw new EventException("listener not registered");
            _instanceValidateListeners.Remove(listener);
            lock (_regLock)
                foreach (var list in _methods.Values)
                    for (var i = list.Count - 1; i >= 0; i--)
                        if (list[i].Listener == listener)
                            list.RemoveAt(i);
        }

        public static void UnregisterListener(this object listener) => UnregisterEvents(listener);
        public static void UnregisterEventsSafity(this object listener)
        {
            if (_instanceValidateListeners.Contains(listener)) UnregisterListener(listener);
        }

        public static void UnregisterStaticEvents(Type type)
        {
            if (!_staticValidateListeners.Contains(type)) throw new EventException("listener not registered");
            _staticValidateListeners.Remove(type);
            lock (_regLock)
                foreach (var list in _methods.Values)
                    for (var i = list.Count - 1; i >= 0; i--)
                        if (list[i].ListenerType == type)
                            list.RemoveAt(i);
        }

        public static void CallEvent(Event e) => InvokeEvent(null, e);

        public static void CallEvent(this IEventCaller o, Event e)
        {
            InvokeEvent(o, e);
        }
        public static void CallEvent<T>(this IEventCaller<T> o, T e) where T : Event => InvokeEvent(o, e);

        private static void InvokeEvent(object o, Event e)
        {
            if (!_methods.ContainsKey(e.GetType())) return;
            List<EventMethod> methods;
            methods = _methods[e.GetType()];
            for (var index = 0; index < methods.Count; index++)
            {
                var method = methods[index];
                if (method.Caller is not null && method.Caller != o) continue;
                /*if (method.CallOption == CallOption.SynchronousOnly)
                    CallEvent(new SyncMethodInvokeEvent(method.Method!, method.Listener, e));*/
                if (method.CallOption == CallOption.None)
                    method.Method!.Invoke(method.Listener, new object[] { e });
                else
                {
                    var method1 = method;
                    Task.Run(() => method1.Method!.Invoke(method1.Listener, new object[] { e }));
                }
            }
        }

       
        

        public static void RegisterStaticEvents<T>() => RegisterStaticEvents(typeof(T));
        public static void RegisterStaticEvents<T, T1>() => RegisterStaticEvents(typeof(T), typeof(T1));

        public static async void RegisterStaticEvents(params Type[] types) => await AddListeners(types, Enumerable.Repeat<object>(null!, types.Length).ToArray());

        private class EventMethod : IComparable<EventMethod>
        {
            public MethodInfo Method;
            public Priority Priority;
            public object Listener;
            public CallOption CallOption;
            public object Caller;
            public Type ListenerType;
            public int CompareTo(EventMethod other) =>
                ReferenceEquals(this, other) ? 0 :
                ReferenceEquals(null, other) ? 1 : Priority.CompareTo(other.Priority);
        }
    }
}