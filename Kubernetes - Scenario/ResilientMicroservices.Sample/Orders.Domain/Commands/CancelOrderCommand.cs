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
    public class CancelOrderCommand : ICommand
    {
        public Guid Id { get; set; }
    }

    public class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IKakfaProducer _kafkaProducer;

        public CancelOrderCommandHandler(IOrderRepository repository, IMapper mapper, IKakfaProducer kafkaProducer)
        {
            _repository = repository;
            _mapper = mapper;
            _kafkaProducer = kafkaProducer;
        }

        public async Task Handle(CancelOrderCommand command, CancellationToken cancellationToken)
        {
            await _repository.CancelOrder(command.Id, cancellationToken);

            await _kafkaProducer.Send(new OrderCancelledEvent
            {
                Id = command.Id
            }, "ReactiveMicroservices");
        }
    }
}
