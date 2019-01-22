using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Contracts;
using Common.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace ResilientMicroservices.Sample.Customers.Data
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        IMongoCollection<Customer> Customers => MongoDatabase.GetCollection<Customer>("Customer");

        public CustomerRepository(MongoDbSettings settings) : base(settings)
        {
        }

        public async Task<Customer> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Customers.Find(c => c.Id == id, new FindOptions { AllowPartialResults = false }).FirstOrDefault(cancellationToken));
        }

        public async Task New(Customer customer, CancellationToken cancellationToken)
        {
            await Customers.InsertOneAsync(customer, new InsertOneOptions { BypassDocumentValidation = false }, cancellationToken);
        }

        public async Task UpdateCreditLimit(Guid customerId, decimal newCreditLimit, CancellationToken cancellationToken)
        {
            var customer = await GetById(customerId, cancellationToken);
            if (customer == default(Customer))
            {
                throw new CustomerNotFoundException($"Failed to find a customer with ID {customerId}");
            }
            customer.CreditLimit = newCreditLimit;
            await Customers.ReplaceOneAsync(c => c.Id == customerId, customer, new UpdateOptions {IsUpsert = false}, cancellationToken);
        }
    }
}
