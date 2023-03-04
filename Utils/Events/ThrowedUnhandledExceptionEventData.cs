namespace Utils.Events;

public sealed class UnhandledExceptionEventData : EventData
{
    public Exception Exception { get; }

    public UnhandledExceptionEventData(Exception exception)
    {
        Exception = exception;
    }
}