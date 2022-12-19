namespace Utils.Events;

[AttributeUsage(AttributeTargets.Method)]
public class CallOptionsAttribute : Attribute
{
    public CallOption Option { get; }

    public CallOptionsAttribute(CallOption option)
    {
        Option = option;
    }
}