﻿using Castle.Core.Logging;
using log4net.Config;
using System;
using log4net.Repository;
using Microsoft.Extensions.Configuration;
using TFramework.Core.Environment;
using System.Reflection;
using log4net;
using System.Collections.Concurrent;
using System.Linq;

namespace TFramework.Core.Logging
{
    public class TfLog4netFactory : AbstractLoggerFactory
    {
        private static ConcurrentDictionary<string, ILoggerRepository> _repositoryCache = new ConcurrentDictionary<string, ILoggerRepository>();
        private static ConcurrentDictionary<string, Assembly> _assemblyCache = new ConcurrentDictionary<string, Assembly>();
        private readonly string _configFilename;
        private readonly IHostEnvironment _hostEnvironment;

        public TfLog4netFactory(
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
            Type componentType = null;
            Assembly componentAssembly = null;

            var assemblyName = _assemblyCache.Keys
                .Where(n => name.StartsWith(n, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(n => n.Length)
                .FirstOrDefault();
            if (!string.IsNullOrEmpty(assemblyName))
            {
                componentAssembly = _assemblyCache[assemblyName];
                componentType = componentAssembly.GetType(name, false, true);
            }

            if (componentType == null)
            {
                componentAssembly = Type.GetType(name).GetTypeInfo().Assembly;
            }
            _assemblyCache.GetOrAdd(componentAssembly.FullName, componentAssembly);

            var repository = _repositoryCache.GetOrAdd(componentAssembly.FullName, n =>
             {
                 var r = LogManager.CreateRepository(n);
                 if (!string.IsNullOrWhiteSpace(_configFilename) && _hostEnvironment.IsFullTrust)
                 {
                     XmlConfigurator.ConfigureAndWatch(r, GetConfigFile(_configFilename));
                 }
                 return r;
             });

            return new TfLog4netLogger(repository.GetLogger(name), this);
        }
    }
}
