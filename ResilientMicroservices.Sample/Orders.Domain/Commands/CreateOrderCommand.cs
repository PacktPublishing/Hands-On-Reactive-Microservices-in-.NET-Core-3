using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.Contracts;
using Common.Contracts.Events;
using Common.Domain;
using Common.Infrastructure.Kafka;
using ResilientMicroservices.Sample.Orders.Data;

namespace ResilientMicroservices.Sample.Orders.Domain.Commands
{
    public class CreateOrderCommand : ICommand
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
    }

    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IKakfaProducer _kafkaProducer;

        public CreateOrderCommandHandler(IOrderRepository repository, IMapper mapper, IKakfaProducer kafkaProducer)
        {
            _repository = repository;
            _mapper = mapper;
            _kafkaProducer = kafkaProducer;
        }

        public async Task Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(command);
            await _repository.New(order, cancellationToken);

            await _kafkaProducer.Send(new OrderCreatedEvent
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Amount = order.Amount,
                Status = order.Status
            }, "ReactiveMicroservices");
        }
    }
}
