using Async.Interfaces.Logger;
using System;

namespace Async.Services.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Error(string message)
        {
            Console.WriteLine(message);
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Warn(string message)
        {
            Console.WriteLine(message);
        }
    }
}
