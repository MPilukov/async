using Async.Interfaces.Logger;
using System;

namespace Async.Services.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Info(string message, Exception exception = null, string request = null, string module = null)
        {
            Console.WriteLine(message);
        }
        public void Error(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(string message, Exception exception = null, string request = null, string module = null)
        {
            Console.WriteLine(message);
        }

        public void Warn(string message)
        {
            Console.WriteLine(message);
        }

        public void Warn(string message, Exception exception = null, string request = null, string module = null)
        {
            Console.WriteLine(message);
        }

        public void Log(string message, string level)
        {
            Console.WriteLine(message);
        }

        public void Log(string message, string level, Exception exception = null, string request = null, string module = null)
        {
            Console.WriteLine(message);
        }
    }
}
