using Common.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.Contracts;
using Common.Contracts.Events;
using Common.Infrastructure.Kafka;
using ResilientMicroservices.Sample.Customers.Data;

namespace ResilientMicroservices.Sample.Customers.Domain.Commands
{
    public class CreateCustomerCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal CreditLimit { get; set; }
    }

    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        private readonly IKakfaProducer _kafkaProducer;

        public CreateCustomerCommandHandler(ICustomerRepository repository, IMapper mapper, IKakfaProducer kafkaProducer)
        {
            _repository = repository;
            _mapper = mapper;
            _kafkaProducer = kafkaProducer;
        }

        public async Task Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<Customer>(command);
            await _repository.New(customer, cancellationToken);

            await _kafkaProducer.Send(new CustomerCreatedEvent
            {
                Id = customer.Id,
                Name = customer.Name,
                CreditLimit = customer.CreditLimit
            }, "ReactiveMicroservices");
        }
    }
}
