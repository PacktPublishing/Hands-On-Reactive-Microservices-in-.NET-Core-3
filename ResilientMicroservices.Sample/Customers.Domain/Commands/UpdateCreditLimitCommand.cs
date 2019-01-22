using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Contracts.Events;
using Common.Domain;
using Common.Infrastructure.Kafka;
using ResilientMicroservices.Sample.Customers.Data;

namespace ResilientMicroservices.Sample.Customers.Domain.Commands
{
    public class UpdateCreditLimitCommand : ICommand
    {
        public Guid CustomerId { get; set; }
        public decimal CreditLimit { get; set; }
    }

    public class UpdateCreditLimitCommandHandler : ICommandHandler<UpdateCreditLimitCommand>
    {
        private readonly ICustomerRepository _repository;
        private readonly IKakfaProducer _kafkaProducer;

        public UpdateCreditLimitCommandHandler(ICustomerRepository repository, IKakfaProducer kafkaProducer)
        {
            _repository = repository;
            _kafkaProducer = kafkaProducer;
        }

        public async Task Handle(UpdateCreditLimitCommand command, CancellationToken cancellationToken)
        {
            await _repository.UpdateCreditLimit(command.CustomerId, command.CreditLimit, cancellationToken);

            await _kafkaProducer.Send(new CreditLimitChangedEvent
            {
                CustomerId = command.CustomerId,
                CreditLimit = command.CreditLimit
            }, "ReactiveMicroservices");
        }
    }
}
