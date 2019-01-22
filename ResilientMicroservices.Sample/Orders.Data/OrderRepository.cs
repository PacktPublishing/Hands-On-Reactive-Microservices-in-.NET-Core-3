using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Contracts;
using Common.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace ResilientMicroservices.Sample.Orders.Data
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        IMongoCollection<Order> Orders => MongoDatabase.GetCollection<Order>("Order");

        public OrderRepository(MongoDbSettings settings) : base(settings)
        {
        }

        public async Task<Order> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Orders.Find(c => c.Id == id, new FindOptions { AllowPartialResults = false }).FirstOrDefault(cancellationToken));
        }

        public async Task New(Order order, CancellationToken cancellationToken)
        {
            order.Status = OrderStatus.Created;
            await Orders.InsertOneAsync(order, new InsertOneOptions { BypassDocumentValidation = false }, cancellationToken);
        }

        public async Task ShipOrder(Guid id, CancellationToken cancellationToken)
        {
            var order = await GetById(id, cancellationToken);
            if (order == default(Order))
            {
                throw new OrderNotFoundException($"Failed to find a order with ID {id}");
            }
            order.Status = OrderStatus.Shipped;
            await Orders.ReplaceOneAsync(c => c.Id == id, order, new UpdateOptions { IsUpsert = false }, cancellationToken);
        }

        public async Task CancelOrder(Guid id, CancellationToken cancellationToken)
        {
            var order = await GetById(id, cancellationToken);
            if (order == default(Order))
            {
                throw new OrderNotFoundException($"Failed to find a order with ID {id}");
            }
            order.Status = OrderStatus.Cancelled;
            await Orders.ReplaceOneAsync(c => c.Id == id, order, new UpdateOptions { IsUpsert = false }, cancellationToken);
        }
    }
}
