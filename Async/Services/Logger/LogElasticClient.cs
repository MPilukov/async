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
            var EsNode = new Uri(connectionSetting);
            var EsConfig = new ConnectionSettings(EsNode);
            _elasticClient = new ElasticClient(EsConfig);

            //var settings = new IndexSettings { NumberOfReplicas = 1, NumberOfShards = 2 };

            //var indexConfig = new IndexState
            //{
            //    Settings = settings
            //};
        }

        public async Task Log(string index, DateTime time, string message, string level, string exception = null, 
            string request = null, string module = null)
        {
            var data = new LogData
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
                .UpdateAsync(DocumentPath<LogData>.Id(new Id(Guid.NewGuid())), x => x.Doc(data).Index(index).DocAsUpsert());
        }

        class LogData
        {
            public DateTime Time { get; set; }

            public string Level { get; set; }

            public string Module { get; set; }

            public string Message { get; set; }

            public string Request { get; set; }

            public string Exception { get; set; }
        }
    }
}
