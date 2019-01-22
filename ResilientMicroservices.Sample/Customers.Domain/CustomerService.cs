using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Contracts;
using ResilientMicroservices.Sample.Customers.Data;

namespace ResilientMicroservices.Sample.Customers.Domain
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsCreditLimitAvailable(Guid customerId, decimal allocatedCredit, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetById(customerId, cancellationToken);
            if (customer == default(Customer))
            {
                throw new CustomerNotFoundException($"Failed to find a customer with ID {customerId}");
            }

            return customer.CreditLimit >= allocatedCredit;
        }
    }
}
