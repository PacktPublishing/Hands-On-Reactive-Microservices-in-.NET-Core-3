using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.Contracts;
using Common.Domain;
using Microsoft.AspNetCore.Mvc;
using ReactiveMicroservices.Sample.Customers.Domain.Commands;

namespace ReactiveMicroservices.Sample.Customers.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILog _log;
        private readonly IMapper _mapper;
        private readonly ICommandHandler<CreateCustomerCommand> _createCustomerHandler;
        private readonly ICommandHandler<UpdateCreditLimitCommand> _updateCreditLimitCommandHandler;

        public CustomerController(ILog log, IMapper mapper,
            ICommandHandler<CreateCustomerCommand> createCustomerHandler,
            ICommandHandler<UpdateCreditLimitCommand> updateCreditLimitCommandHandler)
        {
            _log = log;
            _mapper = mapper;
            _createCustomerHandler = createCustomerHandler;
            _updateCreditLimitCommandHandler = updateCreditLimitCommandHandler;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> NewCustomer([FromBody] Customer customer)
        {
            if (customer == default(Customer))
            {
                return BadRequest("Body empty or null");
            }

            _log.Info("Received Customer data:", customer);
            var command = _mapper.Map<CreateCustomerCommand>(customer);
            await _createCustomerHandler.Handle(command, CancellationToken.None);
            return Ok();
        }

        // PUT api/customer/1111-1111-1111-1111
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] decimal creditLimit)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Customer ID is empty");
            }

            if (creditLimit == default(decimal))
            {
                return BadRequest("Body empty or null");
            }

            _log.Info($"Received credit update information for customer with Id: {id}");
            var command = new UpdateCreditLimitCommand
            {
                CustomerId = id,
                CreditLimit = creditLimit
            };
            await _updateCreditLimitCommandHandler.Handle(command, CancellationToken.None);
            return Ok();
        }
    }
}