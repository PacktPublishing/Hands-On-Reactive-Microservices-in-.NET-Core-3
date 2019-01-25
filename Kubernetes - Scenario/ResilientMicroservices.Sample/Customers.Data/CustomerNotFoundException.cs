using System;

namespace ReactiveMicroservices.Sample.Customers.Data
{
    public class CustomerNotFoundException : ArgumentException
    {
        public CustomerNotFoundException(string message) : base(message)
        {
            
        }
    }
}