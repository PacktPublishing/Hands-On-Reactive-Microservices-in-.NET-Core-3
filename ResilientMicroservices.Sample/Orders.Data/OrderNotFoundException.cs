using System;

namespace ResilientMicroservices.Sample.Orders.Data
{
    public class OrderNotFoundException : ArgumentException
    {
        public OrderNotFoundException(string message) : base(message)
        {
        }
    }
}