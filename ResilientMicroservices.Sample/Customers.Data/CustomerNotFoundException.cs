using System;

namespace ResilientMicroservices.Sample.Customers.Data
{
    public class CustomerNotFoundException : ArgumentException
    {
        public CustomerNotFoundException(string message) : base(message)
        {
            
        }
    }
}