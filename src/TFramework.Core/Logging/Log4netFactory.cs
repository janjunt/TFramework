using Castle.Core.Logging;
using log4net.Config;
using System;
using log4net.Repository;
using Microsoft.Extensions.Configuration;
using TFramework.Core.Environment;
using System.Reflection;
using log4net;
using System.Collections.Concurrent;

namespace TFramework.Core.Logging
{
    public class Log4netFactory : AbstractLoggerFactory
    {
        private static ConcurrentDictionary<string, ILoggerRepository> _repositoryCache = new ConcurrentDictionary<string, ILoggerRepository>();
        private static ConcurrentDictionary<string, Assembly> _assemblyCache = new ConcurrentDictionary<string, Assembly>();
        private readonly string _configFilename;
        private readonly IHostEnvironment _hostEnvironment;

        public Log4netFactory(
            IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            _configFilename = configuration["log4net.Config"];
            _hostEnvironment = hostEnvironment;
        }

        public override Castle.Core.Logging.ILogger Create(string name, LoggerLevel level)
        {
            throw new NotSupportedException("日志级别不能在运行时设置。请查看您的配置文件。");
        }

        public override Castle.Core.Logging.ILogger Create(string name)
        {
            var componentAssembly = _assemblyCache.GetOrAdd(name, n => Type.GetType(n).GetTypeInfo().Assembly);
            var repository = LogManager.GetRepository(componentAssembly);

            if (!string.IsNullOrWhiteSpace(_configFilename) && _hostEnvironment.IsFullTrust)
            {
                _repositoryCache.GetOrAdd(repository.Name, n =>
                {
                    XmlConfigurator.ConfigureAndWatch(repository, GetConfigFile(_configFilename));
                    return repository;
                });
            }

            return new Log4netLogger(repository.GetLogger(name), this);
        }
    }
}
