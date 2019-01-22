using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Contracts.Events
{
    [Event("CustomerRegistered")]
    public class CustomerRegisteredEvent : IEvent
    {
        public Customer Customer { get; set; }
    }
}
