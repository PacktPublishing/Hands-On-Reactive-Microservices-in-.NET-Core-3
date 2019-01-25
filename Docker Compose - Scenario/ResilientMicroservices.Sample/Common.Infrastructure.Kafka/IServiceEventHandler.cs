using Common.Domain;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Infrastructure.Kafka
{
    public interface IServiceEventHandler
    {
        Task Handle(JObject jObject, ILog log, CancellationToken cancellationToken);
    }
}
