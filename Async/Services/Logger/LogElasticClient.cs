using Async.Interfaces.Logger;
using Nest;
using System;
using System.Threading.Tasks;

namespace Async.Services.Logger
{
    public class LogElasticClient : ILogElasticClient
    {
        private readonly IElasticClient _elasticClient;
        public LogElasticClient(string connectionSetting)
        {
            var uri = new Uri(connectionSetting);
            var settings = new ConnectionSettings(uri);

            _elasticClient = new ElasticClient(settings);
        }

        public async Task Log(string index, DateTime time, string message, string level, string exception = null, 
            string request = null, string module = null)
        {
            var data = new LogElasticData
            {
                Time = time,
                Message = message,
                Level = level,
                Exception = exception,
                Request = request,
                Module = module,
            };

            if (!(await _elasticClient.Indices.ExistsAsync(index)).Exists)
            {
                await _elasticClient.Indices.CreateAsync(index);
            }

            var upsert = await _elasticClient
                .UpdateAsync(DocumentPath<LogElasticData>.Id(
                        new Id(Guid.NewGuid())),
                    x => x.Doc(data).Index(index).DocAsUpsert());
        }
    }
}
