using Async.Interfaces.Logger;
using System;

namespace Async.Services.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Info(string message)
        {
            WriteToConsole(message);
        }

        public void Info(string message, Exception exception = null, string request = null, string module = null)
        {
            WriteToConsole(message);
        }
        public void Error(string message)
        {
            WriteToConsole(message);
        }

        public void Error(string message, Exception exception = null, string request = null, string module = null)
        {
            WriteToConsole(message);
        }

        public void Warn(string message)
        {
            WriteToConsole(message);
        }

        public void Warn(string message, Exception exception = null, string request = null, string module = null)
        {
            WriteToConsole(message);
        }

        public void Log(string message, string level)
        {
            WriteToConsole(message);
        }

        public void Log(string message, string level, Exception exception = null, string request = null, string module = null)
        {
            WriteToConsole(message);
        }

        private static void WriteToConsole(string message)
        {
            Console.WriteLine(message);
        }
    }
}
