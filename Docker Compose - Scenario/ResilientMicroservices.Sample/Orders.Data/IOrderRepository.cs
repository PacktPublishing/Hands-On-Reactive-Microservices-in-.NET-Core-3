using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Contracts;

namespace ReactiveMicroservices.Sample.Orders.Data
{
    public interface IOrderRepository
    {
        Task<Order> GetById(Guid id, CancellationToken cancellationToken);
        Task New(Order order, CancellationToken cancellationToken);
        Task ShipOrder(Guid id, CancellationToken cancellationToken);
        Task CancelOrder(Guid id, CancellationToken cancellationToken);
    }
}