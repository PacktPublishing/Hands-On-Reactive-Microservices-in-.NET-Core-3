using System;

namespace Common.Contracts.Events
{
    [Event("CustomerCreated")]
    public class CustomerCreatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal CreditLimit { get; set; }
    }
}
