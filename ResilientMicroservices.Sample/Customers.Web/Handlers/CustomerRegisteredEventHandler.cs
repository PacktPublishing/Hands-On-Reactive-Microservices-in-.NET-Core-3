using Common.Domain;
using Common.Infrastructure.Kafka;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ResilientMicroservices.Sample.Customers.Web.Handlers
{
    public class CustomerRegisteredEventHandler : IServiceEventHandler
    {
        public Task Handle(JObject jObject, ILog log, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
