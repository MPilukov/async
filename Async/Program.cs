using Async.Interfaces.Cache;
using Async.Interfaces.Publish;
using Async.Services.Cache;
using Async.Services.Publish;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Async
{
    class Program
    {
        private static readonly Action<string> Log = Console.WriteLine; 

        static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            if (services == null)
            {
                Log("Не удалось зарегистрировать некоторые сервисы. Завершаем работу асинка.");
                return;
            }

            var serviceProvider = services.BuildServiceProvider();
            if (!IsSuccessTestServices(serviceProvider))
            {
                Log("Не удалось протестировать работу некоторых сервисов. Завершаем работу асинка.");
                return;
            }

            try
            {
                Log("Зависимости успешно зарегистрированы и протестированы. Запускаем асинк.");
                await serviceProvider.GetService<Executer>().Run();
            }
            catch (Exception exc)
            {
                Log("Не удалось запустить асинк. Завершаем работу.");
                Log($"Exception : {exc}");
            }
        }

        private static bool IsSuccessTestServices(ServiceProvider serviceProvider)
        {
            var maxRetryCounter = 5;
            var timeout = 5000;

            var subscriber = serviceProvider.GetService<ISubscriber>();

            if (!IsSuccessAction(() => subscriber.Subscribe(typeof(RabbitMq.Messages.TestMessage), m => { return Task.FromResult(0); }), 
                maxRetryCounter, timeout, "Не удалось протестировать ISubscriber"))
            {
                return false;
            }

            var cache = serviceProvider.GetService<ICache>();
            if (!IsSuccessAction(() => cache.Get("TestValue"), maxRetryCounter, timeout, "Не удалось протестировать ICache"))
            {
                return false;
            }

            return true;
        }

        private static bool IsSuccessAction(Action action, int maxRetryCounter, int timeout, string errorText)
        {
            var retryCounter = 0;

            while (retryCounter < maxRetryCounter)
            {
                try
                {
                    if (retryCounter > 0)
                    {
                        Thread.Sleep(timeout);
                    }

                    action();
                    return true;
                }
                catch (Exception exc)
                {
                    Log($"{errorText}. Попытка : {retryCounter} : {exc}");
                }

                retryCounter++;
            }

            if (retryCounter == maxRetryCounter)
            {
                Log(errorText);
                return false;
            }

            return true;
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            var config = GetConfiguration();

            services.AddSingleton(config);
            services.AddSingleton<ISubscriber>(str => new Subscriber(config.GetConnectionString("rabbit")));
            services.AddTransient<ICache>(str => new Cache(config.GetConnectionString("redis")));

            RegisterMessageHandlers(services);

            services.AddTransient<Executer>();
            return services;
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        private static void RegisterMessageHandlers(IServiceCollection services)
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

                services.TryAddSingleton(handlerType);
            }
        }
    }
}
