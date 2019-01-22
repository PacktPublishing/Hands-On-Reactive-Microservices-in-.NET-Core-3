using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Common.Contracts;
using ResilientMicroservices.Sample.Customers.Domain.Commands;

namespace ResilientMicroservices.Sample.Customers.Domain
{
    public class CommandToContractMapperProfile : Profile
    {
        public CommandToContractMapperProfile()
        {
            CreateMap<CreateCustomerCommand, Customer>()
                .ForMember(c => c.Id, options => options.MapFrom(source => source.Id))
                .ForMember(c => c.Name, options => options.MapFrom(source => source.Name))
                .ForMember(c => c.CreditLimit, options => options.MapFrom(source => source.CreditLimit));
        }
    }
}
