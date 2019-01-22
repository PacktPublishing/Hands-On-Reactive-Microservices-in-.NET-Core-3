using AutoMapper;
using Common.Contracts;
using ResilientMicroservices.Sample.Orders.Domain.Commands;

namespace ResilientMicroservices.Sample.Orders.Domain
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