using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.Contracts;
using Common.Domain;
using Microsoft.AspNetCore.Mvc;
using ReactiveMicroservices.Sample.Orders.Domain.Commands;

namespace ReactiveMicroservices.Sample.Orders.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILog _log;
        private readonly IMapper _mapper;
        private readonly ICommandHandler<CreateOrderCommand> _commandHandler;

        public OrdersController(ILog log, IMapper mapper,
            ICommandHandler<CreateOrderCommand> commandHandler)
        {
            _log = log;
            _mapper = mapper;
            _commandHandler = commandHandler;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> NewOrder([FromBody] Order order)
        {
            if (order == default(Order))
            {
                return BadRequest("Body empty or null");
            }
            _log.Info("Received Order data:", order);
            var command = _mapper.Map<CreateOrderCommand>(order);
            await _commandHandler.Handle(command, CancellationToken.None);
            return Ok();
        }
    }
}
