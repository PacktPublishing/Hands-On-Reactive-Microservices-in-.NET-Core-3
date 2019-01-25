using System;

namespace Common.Contracts
{
    [Serializable]
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal CreditLimit { get; set; }
    }
}
