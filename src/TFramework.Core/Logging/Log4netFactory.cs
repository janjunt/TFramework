using Castle.Core.Logging;
using log4net.Config;
using System;
using log4net.Repository;
using Microsoft.Extensions.Configuration;
using TFramework.Core.Environment;

namespace TFramework.Core.Logging
{
    public class Log4netFactory : AbstractLoggerFactory
    {
        private static bool _isFileWatched = false;

        private ILoggerRepository _loggerRepository;

        public Log4netFactory(
            ILoggerRepository loggerRepository, 
            IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            _loggerRepository = loggerRepository;
            var configFilename = configuration["log4net.Config"];
            if (!_isFileWatched && !string.IsNullOrWhiteSpace(configFilename) && hostEnvironment.IsFullTrust)
            {
                XmlConfigurator.ConfigureAndWatch(loggerRepository, GetConfigFile(configFilename));
                _isFileWatched = true;
            }
        }

        public override Castle.Core.Logging.ILogger Create(string name, LoggerLevel level)
        {
            throw new NotSupportedException("日志级别不能在运行时设置。请查看您的配置文件。");
        }

        public override Castle.Core.Logging.ILogger Create(string name)
        {
            return new Log4netLogger(_loggerRepository.GetLogger(name), this);
        }
    }
}
