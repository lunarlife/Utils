namespace Utils.Events;

public interface IEventCaller
{
    
}
public interface IEventCaller<T> where T : Event
{
}