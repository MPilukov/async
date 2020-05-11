namespace Async.Interfaces.Logger
{
    public interface ILogger
    {
        void Info(string message);
        void Error(string message);
        void Warn(string message);
    }
}