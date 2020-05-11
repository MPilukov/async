using System;

namespace Async.Interfaces.Logger
{
    public interface ILogger
    {
        void Info(string message);
        void Info(string message, Exception exception = null, string request = null, string module = null);
        void Warn(string message);
        void Warn(string message, Exception exception = null, string request = null, string module = null);
        void Error(string message);
        void Error(string message, Exception exception = null, string request = null, string module = null);
        void Log(string message, string level);
        void Log(string message, string level, Exception exception = null, string request = null, string module = null);
    }
}