using AutoMapper;
using Common.Contracts;
using ResilientMicroservices.Sample.Customers.Domain.Commands;

namespace ResilientMicroservices.Sample.Customers.Domain
{
    public class ContractToCommandMapperProfile : Profile
    {
        public ContractToCommandMapperProfile()
        {
            CreateMap<Customer, CreateCustomerCommand>()
                .ForMember(c => c.Id, options => options.MapFrom(source => source.Id))
                .ForMember(c => c.Name, options => options.MapFrom(source => source.Name))
                .ForMember(c => c.CreditLimit, options => options.MapFrom(source => source.CreditLimit));
        }
    }
}