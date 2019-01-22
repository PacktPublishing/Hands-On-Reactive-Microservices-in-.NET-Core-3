using Common.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.Contracts;
using ResilientMicroservices.Sample.Customers.Data;

namespace ResilientMicroservices.Sample.Customers.Domain.Commands
{
    public class NewCustomerCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal CreditLimit { get; set; }
    }

    public class NewCustomerCommandHandler : ICommandHandler<NewCustomerCommand>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public NewCustomerCommandHandler(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task Handle(NewCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<Customer>(command);
            await _repository.New(customer, cancellationToken);
        }
    }
}
