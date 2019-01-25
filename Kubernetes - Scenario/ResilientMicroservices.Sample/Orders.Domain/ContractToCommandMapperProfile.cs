using AutoMapper;
using Common.Contracts;
using ReactiveMicroservices.Sample.Orders.Domain.Commands;

namespace ReactiveMicroservices.Sample.Orders.Domain
{
    public class ContractToCommandMapperProfile : Profile
    {
        public ContractToCommandMapperProfile()
        {
            CreateMap<Order, CreateOrderCommand>()
                .ForMember(c => c.Id, options => options.MapFrom(source => source.Id))
                .ForMember(c => c.CustomerId, options => options.MapFrom(source => source.CustomerId))
                .ForMember(c => c.Amount, options => options.MapFrom(source => source.Amount));
        }
    }
}