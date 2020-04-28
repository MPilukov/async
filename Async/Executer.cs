using Async.Interfaces.Publish;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Async
{
    public class Executer
    {
        private readonly ISubscriber _subscriber;
        private readonly IServiceProvider _serviceProvider;

        public Executer(IServiceProvider serviceProvider, ISubscriber subscriber)
        {
            _serviceProvider = serviceProvider;
            _subscriber = subscriber;
        }

        public async Task Run()
        {
            Subscribe(_subscriber);

            while (true)
            {
                await Task.Delay(10000);
            }
        }

        private void Subscribe(ISubscriber subscriber)
        {
            var handlerTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(MessageHandlerBase))).ToList();

            foreach (var handlerType in handlerTypes)
            {
                var messageHandlerBaseType = handlerType.BaseType;
                if (messageHandlerBaseType == null)
                {
                    continue;
                }

                var genericTypes = messageHandlerBaseType.GenericTypeArguments;

                if (genericTypes.Length == 0)
                {
                    continue;
                }

                var messageType = genericTypes.First();

                var handlerItem = _serviceProvider.GetService(handlerType);
                var handler = (MessageHandlerBase)handlerItem;

                var handleMethod = handlerType.GetMethod("Handle");
                subscriber.Subscribe(messageType,
                    m =>
                    {
                        return Task.Factory.StartNew(() =>
                        {
                            handleMethod?.Invoke(handler, new object[] { m });
                        });
                    });
            }
        }
    }
}
