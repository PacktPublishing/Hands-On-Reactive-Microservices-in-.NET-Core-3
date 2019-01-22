using System.Threading;
using System.Threading.Tasks;
using Common.Contracts;
using Common.Contracts.Events;
using Common.Domain;
using Common.Infrastructure.Kafka;
using Newtonsoft.Json.Linq;

namespace ResilientMicroservices.Sample.Customers.Domain.EventHandlers
{
    public class OrderCreatedEventHandler : IServiceEventHandler
    {
        private readonly ICustomerService _service;
        private readonly IKakfaProducer _producer;

        public OrderCreatedEventHandler(ICustomerService service, IKakfaProducer producer)
        {
            _service = service;
            _producer = producer;
        }

        public async Task Handle(JObject jObject, ILog log, CancellationToken cancellationToken)
        {
            log.Info("Handled order created event");
            var order = jObject.ToObject<OrderCreatedEvent>();

            var isValidOrder = await _service.IsCreditLimitAvailable(order.CustomerId, order.Amount, cancellationToken);
            var @event = new OrderValidatedEvent { OrderId = order.Id, IsValid = isValidOrder };
            await _producer.Send(@event, "ReactiveMicroservices");
        }
    }
}