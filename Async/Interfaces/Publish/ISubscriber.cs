using System;
using System.Threading.Tasks;

namespace Async.Interfaces.Publish
{
    public interface ISubscriber
    {
        void Subscribe(Type messageType, Func<Message, Task> consumerDelegate);
    }
}
