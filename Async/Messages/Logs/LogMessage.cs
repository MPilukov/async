using Async.Interfaces.Publish;
using System;

namespace RabbitMq.Messages.Logs
{
    public class LogMessage : Message
    {
        public DateTime Time { get; set; }
        public string Level { get; set; }
        public string Module { get; set; }
        public string Message { get; set; }
        public string Request { get; set; }
        public Exception Exception { get; set; }
    }
}
