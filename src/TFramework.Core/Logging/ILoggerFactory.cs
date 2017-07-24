using System;

namespace TFramework.Core.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(Type type);
    }
}
