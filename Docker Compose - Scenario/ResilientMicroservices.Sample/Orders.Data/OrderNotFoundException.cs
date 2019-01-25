using System;

namespace ReactiveMicroservices.Sample.Orders.Data
{
    public class OrderNotFoundException : ArgumentException
    {
        public OrderNotFoundException(string message) : base(message)
        {
        }
    }
}