using System;

namespace Common.Contracts
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EventAttribute : Attribute
    {
        public string Name { get; }

        public EventAttribute(string name)
        {
            Name = name;
        }
    }
}
