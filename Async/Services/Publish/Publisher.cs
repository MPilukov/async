using Async.Interfaces.Publish;
using EasyNetQ;

namespace Async.Services.Publish
{
    public class Publisher : IPublisher
    {
        private static IBus _bus;
        private static volatile string isInitialized = null;
        private static readonly object initLock = new object();

        public Publisher(string connectionString)
        {
            Init(connectionString);
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

        public void Publish<T>(T message) where T : class
        {
            _bus.Publish(message);
        }
    }
}
