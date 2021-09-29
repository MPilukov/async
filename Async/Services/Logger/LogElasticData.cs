using System;

namespace Async.Services.Logger
{
    public class LogElasticData
    {
        public DateTime Time { get; set; }

        public string Level { get; set; }

        public string Module { get; set; }

        public string Message { get; set; }

        public string Request { get; set; }

        public string Exception { get; set; }
    }
}