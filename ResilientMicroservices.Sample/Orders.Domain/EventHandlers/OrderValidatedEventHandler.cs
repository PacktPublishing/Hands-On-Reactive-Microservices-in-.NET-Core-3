using System.Threading;
using System.Threading.Tasks;
using Common.Contracts.Events;
using Common.Domain;
using Common.Infrastructure.Kafka;
using Newtonsoft.Json.Linq;

namespace ResilientMicroservices.Sample.Orders.Domain.EventHandlers
{
    public class OrderValidatedEventHandler : IServiceEventHandler
    {
        private readonly IKakfaProducer _producer;

        public OrderValidatedEventHandler(IKakfaProducer producer)
        {
            _producer = producer;
        }
        public async Task Handle(JObject jObject, ILog log, CancellationToken cancellationToken)
        {
            var orderValidated = jObject.ToObject<OrderValidatedEvent>();
            if (orderValidated.IsValid)
            {
                var orderShippedEvent = new OrderShippedEvent
                {
                    Id = orderValidated.OrderId
                };
                await _producer.Send(orderShippedEvent, "ReactiveMicroservices");
            }
            else
            {
                var orderCancelledEvent = new OrderCancelledEvent
                {
                    Id = orderValidated.OrderId
                };
                await _producer.Send(orderCancelledEvent, "ReactiveMicroservices");
            }
        }
    }
}
