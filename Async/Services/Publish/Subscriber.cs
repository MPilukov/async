using Async.Interfaces.Publish;
using EasyNetQ;
using System;
using System.Threading.Tasks;
using EasyNetQ.NonGeneric;
using Async.Interfaces.Logger;

namespace Async.Services.Publish
{
    public class Subscriber : ISubscriber
    {
        private static IBus _bus;
        private readonly ILogger _logger;
        private static volatile string isInitialized = null;
        private static readonly object initLock = new object();

        public Subscriber(string connectionString, ILogger logger)
        {
            Init(connectionString);
            _logger = logger;
        }

        private static void Init(string connectionString)
        {
            if (isInitialized == null)
            {
                lock (initLock)
                {
                    if (isInitialized == null)
                    {
                        _bus = RabbitHutch.CreateBus(connectionString,
                                    services => services.Register<ITypeNameSerializer, TypeNameSerializer>()
                                        .Register<IConventions, MyConventions>());


                        isInitialized = "";
                    }
                }
            }
        }

        private Task ConsumerDelegateWrapper(Message message, Func<Message, Task> consumerDelegate)
        {
            return Task.Run(() => consumerDelegate(message));
        }

        public void Subscribe(Type messageType, Func<Message, Task> consumerDelegate)
        {
            async Task ConsumerDelegate(Message msg)
            {
                try
                {
                    await consumerDelegate(msg);
                }
                catch (Exception exception)
                {
                    _logger.Error($"Error in handling message : {msg} : {exception}");
                }
            }

            var subscription = _bus.SubscribeAsync(
                    messageType,
                    string.Empty,
                    message => message is Message msg ?
                        ConsumerDelegateWrapper(msg, ConsumerDelegate) : Task.FromResult(0),
                    configure => {
                        configure.WithPrefetchCount(20);
                    });
        }
    }
}
