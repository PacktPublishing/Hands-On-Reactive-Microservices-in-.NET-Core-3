using System;

namespace Common.Contracts
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public OrderStatus Status { get; set; }
    }

    public enum OrderStatus
    {
        Created,
        Shipped,
        Cancelled
    }
}
