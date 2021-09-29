using Async.Interfaces.Logger;
using Async.Interfaces.Publish;
using System.Threading.Tasks;
using Async.Messages.Logs;

namespace Async.MessageHandlers.Logs
{
    public class LogMessageHandler : MessageHandler<LogMessage>
    {
        private readonly ILogElasticClient _logElasticClient;

        public LogMessageHandler(ILogElasticClient logElasticClient)
        {
            _logElasticClient = logElasticClient;
        }

        public override async Task Handle(LogMessage message)
        {
            await _logElasticClient.Log("index", message.Time, message.Message, message.Level, 
                message.Exception?.ToString() ?? null, message.Request, message.Module);
        }
    }
}
