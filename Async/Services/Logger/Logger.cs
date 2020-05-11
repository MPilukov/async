using Async.Interfaces.Logger;
using System;

namespace Async.Services.Logger
{
    public class Logger : ILogger
    {
        public void Error(string message)
        {
            throw new NotImplementedException();
        }

        public void Info(string message)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message)
        {
            throw new NotImplementedException();
        }
    }
}
