using System;
using Utils;

namespace Utils.Events
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class EventHandlerAttribute : Attribute
    {
        public Priority Priority { get; }
 
        public EventHandlerAttribute(Priority priority)
        {
            Priority = priority;
        }
        public EventHandlerAttribute()
        {
            Priority = Priority.Normal;
        }
    }
}