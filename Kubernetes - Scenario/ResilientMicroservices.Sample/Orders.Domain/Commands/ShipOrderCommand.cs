using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.Contracts.Events;
using Common.Domain;
using Common.Infrastructure.Kafka;
using ReactiveMicroservices.Sample.Orders.Data;

namespace ReactiveMicroservices.Sample.Orders.Domain.Commands
{
    public class ShipOrderCommand : ICommand
    {
        public Guid Id { get; set; }
    }

    public class ShipOrderCommandHandler : ICommandHandler<ShipOrderCommand>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IKakfaProducer _kafkaProducer;

        public ShipOrderCommandHandler(IOrderRepository repository, IMapper mapper, IKakfaProducer kafkaProducer)
        {
            _repository = repository;
            _mapper = mapper;
            _kafkaProducer = kafkaProducer;
        }

        public async Task Handle(ShipOrderCommand command, CancellationToken cancellationToken)
        {
            await _repository.ShipOrder(command.Id, cancellationToken);

            await _kafkaProducer.Send(new OrderShippedEvent
            {
                Id = command.Id
            }, "ReactiveMicroservices");
        }
    }
}
