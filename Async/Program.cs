using Async.Interfaces.Cache;
using Async.Interfaces.Logger;
using Async.Interfaces.Publish;
using Async.Services.Cache;
using Async.Services.Logger;
using Async.Services.Publish;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Async.Messages;

namespace Async
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            if (services == null)
            {
                Console.WriteLine("Failed to register some services. Stop async");
                return;
            }

            var serviceProvider = services.BuildServiceProvider();
            var logger = GetLogger(serviceProvider);

            if (!IsSuccessTestServices(serviceProvider, logger))
            {
                logger.Error("Failed to test the work before start async. Stop async");
                return;
            }

            try
            {
                await serviceProvider.GetService<Executer>().Run();
            }
            catch (Exception exc)
            {
                logger.Error("Failed to start async. Stop async");
                logger.Error($"Exception : {exc}");
            }
        }

        private static ILogger GetLogger(IServiceProvider serviceProvider)
        {
            try
            {
                var logger = serviceProvider.GetService<ILogger>();
                return logger;
            }
            catch (Exception exc)
            {
                var logger = new ConsoleLogger();
                logger.Error(exc.ToString());
                return logger;
            }
        }

        private static bool IsSuccessTestServices(IServiceProvider serviceProvider, ILogger logger)
        {
            const int maxRetryCounter = 5;
            const int timeout = 5000;

            var subscriber = serviceProvider.GetService<ISubscriber>();

            if (!IsSuccessAction(() => subscriber.Subscribe(typeof(TestMessage), m => Task.FromResult(0)), 
                maxRetryCounter, timeout, "Failed to test ISubscriber", logger))
            {
                return false;
            }

            var cache = serviceProvider.GetService<ICache>();
            if (!IsSuccessAction(() => cache.Get("TestValue"), maxRetryCounter, timeout, "Failed to test ICache", logger))
            {
                return false;
            }

            var publisher = serviceProvider.GetService<IPublisher>();
            if (!IsSuccessAction(() => publisher.Publish(new TestMessage()),
                maxRetryCounter, timeout, "Failed to test IPublisher", logger))
            {
                return false;
            }

            return true;
        }

        private static bool IsSuccessAction(Action action, int maxRetryCounter, int timeout, string errorText, ILogger logger)
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
                    logger.Warn($"{errorText}. Attempt : {retryCounter} : {exc}");
                }

                retryCounter++;
            }

            if (retryCounter == maxRetryCounter)
            {
                logger.Warn(errorText);
                return false;
            }

            return true;
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            var config = GetConfiguration();
            services.AddSingleton(config);

            var publisher = new Publisher(config.GetConnectionString("rabbit"));
            services.AddSingleton<IPublisher>(str => publisher);

            services.AddTransient<ILogElasticClient>(str => new LogElasticClient(config.GetConnectionString("elastic")));
            services.AddTransient<ILogger>(str => new Logger(publisher));

            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger>();

            services.AddSingleton<ISubscriber>(str => new Subscriber(config.GetConnectionString("rabbit"), logger));
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
