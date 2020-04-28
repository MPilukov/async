using RabbitMq.Messages;
using System.Threading.Tasks;

namespace Async.Interfaces.Publish
{
    public abstract class MessageHandler<T> : MessageHandlerBase where T : Message
    {
        public abstract Task Handle(T message);
    }
}
