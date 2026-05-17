using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Persistence.Mongo
{
    public class RabbitMqNewSettings
    {
        public string HostName { get; set; } = null!;

        public string ExchangeName { get; set; } = null!;

        public string QueueName { get; set; } = null!;
    }
}
