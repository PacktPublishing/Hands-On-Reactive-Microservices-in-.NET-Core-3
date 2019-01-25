using AutoMapper;
using Common.Contracts;
using ReactiveMicroservices.Sample.Orders.Domain.Commands;

namespace ReactiveMicroservices.Sample.Orders.Domain
{
    public class CommandToContractMapperProfile : Profile
    {
        public CommandToContractMapperProfile()
        {
            CreateMap<CreateOrderCommand, Order>()
                .ForMember(c => c.Id, options => options.MapFrom(source => source.Id))
                .ForMember(c => c.CustomerId, options => options.MapFrom(source => source.CustomerId))
                .ForMember(c => c.Amount, options => options.MapFrom(source => source.Amount))
                .ForMember(c => c.Status, options => options.MapFrom(source => OrderStatus.Created));
        }
    }
}
