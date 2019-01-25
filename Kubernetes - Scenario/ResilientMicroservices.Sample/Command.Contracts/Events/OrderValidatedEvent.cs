using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Contracts.Events
{
    [Event("OrderValidated")]
    public class OrderValidatedEvent : IEvent
    {
        public Guid OrderId { get; set; }
        public bool IsValid{ get; set; }
    }
}
