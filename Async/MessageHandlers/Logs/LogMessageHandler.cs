using Async.Interfaces.Logger;
using Async.Interfaces.Publish;
using RabbitMq.Messages.Logs;
using System.Threading.Tasks;

namespace Async.MessageHandlers.Logs
{
    public class LogMessageHandler : MessageHandler<LogMessage>
    {
        private readonly ILogElasticClient _logElasticClient;

        public LogMessageHandler(ILogElasticClient logElasticClient)
        {
            _logElasticClient = logElasticClient;
        }

        public override Task Handle(LogMessage message)
        {
            return _logElasticClient.Log("index", message.Time, message.Message, message.Level, 
                message.Exception?.ToString() ?? null, message.Request, message.Module);
        }
    }
}
