using System;

namespace Async.Interfaces.Logger
{
    public interface ILogger
    {
        void Info(string message);
        void Info(string message, Exception exception, string request, string module);
        void Warn(string message);
        void Warn(string message, Exception exception, string request, string module);
        void Error(string message);
        void Error(string message, Exception exception, string request, string module);
        void Log(string message, string level);
        void Log(string message, string level, Exception exception, string request, string module);
    }
}