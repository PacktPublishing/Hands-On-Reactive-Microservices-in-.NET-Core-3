using System;

namespace Common.Contracts.Events
{
    [Event("OrderCreated")]
    public class OrderCreatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public OrderStatus Status { get; set; }
    }
}
