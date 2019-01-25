using System;

namespace Common.Contracts.Events
{
    [Event("CreditLimitChanged")]
    public class CreditLimitChangedEvent : IEvent
    {
        public Guid CustomerId { get; set; }
        public decimal CreditLimit { get; set; }
    }
}
