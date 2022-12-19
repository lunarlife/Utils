using System.Reflection;

namespace Utils.Events;

public class SyncMethodInvokeEvent : Event
{
    public MethodInfo Method { get; }
    public object Target { get; }
    public object[] Arguments { get; }
    
    public SyncMethodInvokeEvent(MethodInfo method, object target, params object[] arguments)
    {
        Arguments = arguments;
        Method = method;
        Target = target;
    }
    public SyncMethodInvokeEvent(Action action)
    {
        Method = action.Method;
        Target = action.Target;
    }
}