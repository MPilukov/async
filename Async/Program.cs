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
using System.Threading.Tasks;

namespace Async
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = ConfigureServices();

            var serviceProvider = services.BuildServiceProvider();

            await serviceProvider.GetService<Executer>().Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            var config = GetConfiguration();

            services.AddSingleton(config);
            services.AddSingleton<ISubscriber>(
                str => new Subscriber(config.GetConnectionString("rabbit")));
            services.AddTransient<ICache>(
                str => new Cache(config.GetConnectionString("redis")));

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
