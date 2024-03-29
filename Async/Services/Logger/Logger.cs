﻿using Async.Interfaces.Logger;
using Async.Interfaces.Publish;
using System;
using Async.Messages.Logs;

namespace Async.Services.Logger
{
    public class Logger : ILogger
    {
        private readonly IPublisher _publisher;

        public Logger(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public void Info(string message)
        {
            _publisher.Publish(new LogMessage
            {
                Level = "Info",
                Message = message,
                Time = DateTime.UtcNow,
            });
        }

        public void Warn(string message)
        {
            _publisher.Publish(new LogMessage
            {
                Level = "Warn",
                Message = message,
                Time = DateTime.UtcNow,
            });
        }

        public void Error(string message)
        {
            _publisher.Publish(new LogMessage
            {
                Level = "Error",
                Message = message,
                Time = DateTime.UtcNow,
            });
        }

        public void Info(string message, Exception exception, string request, string module)
        {
            _publisher.Publish(new LogMessage
            {
                Level = "Info",
                Message = message,
                Exception = exception,
                Module = module,
                Request = request,
                Time = DateTime.UtcNow,
            });
        }

        public void Warn(string message, Exception exception, string request, string module)
        {
            _publisher.Publish(new LogMessage
            {
                Level = "Warn",
                Message = message,
                Exception = exception,
                Module = module,
                Request = request,
                Time = DateTime.UtcNow,
            });
        }

        public void Error(string message, Exception exception, string request, string module)
        {
            _publisher.Publish(new LogMessage
            {
                Level = "Error",
                Message = message,
                Exception = exception,
                Module = module,
                Request = request,
                Time = DateTime.UtcNow,
            });
        }

        public void Log(string message, string level)
        {
            _publisher.Publish(new LogMessage
            {
                Level = level,
                Message = message,
                Time = DateTime.UtcNow,
            });
        }

        public void Log(string message, string level, Exception exception, string request, string module)
        {
            _publisher.Publish(new LogMessage
            {
                Level = level,
                Message = message,
                Time = DateTime.UtcNow,
            });
        }
    }
}
