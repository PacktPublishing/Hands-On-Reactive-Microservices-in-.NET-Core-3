using System;
using System.Threading;
using System.Threading.Tasks;

namespace ResilientMicroservices.Sample.Customers.Domain
{
    public interface ICustomerService
    {
        Task<bool> IsCreditLimitAvailable(Guid customerId, decimal allocatedCredit, CancellationToken cancellationToken);
    }
}