using System;

namespace Common.Contracts.Events
{
    [Event("OrderShipped")]
    public class OrderShippedEvent : IEvent
    {
        public Guid Id { get; set; }
    }
}
