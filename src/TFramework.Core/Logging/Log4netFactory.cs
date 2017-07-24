using Castle.Core.Logging;
using log4net;
using log4net.Config;
using System;
using Microsoft.AspNetCore.Hosting;

namespace TFramework.Core.Logging
{
    public class Log4netFactory : AbstractLoggerFactory
    {
        private static bool _isFileWatched = false;

        //public Log4netFactory(IHostingEnvironment hostingEnvironment) 
        //    : this(ConfigurationManager.AppSettings["log4net.Config"], hostingEnvironment) { }

        public Log4netFactory(string configFilename, IHostingEnvironment hostingEnvironment)
        {
            //if (!_isFileWatched && !string.IsNullOrWhiteSpace(configFilename) && hostingEnvironment.IsFullTrust)
            //{
            //    // Only monitor configuration file in full trust
            //    XmlConfigurator.ConfigureAndWatch(GetConfigFile(configFilename));
            //    _isFileWatched = true;
            //}
        }

        public override Castle.Core.Logging.ILogger Create(string name, LoggerLevel level)
        {
            throw new NotSupportedException("日志级别不能在运行时设置。请查看您的配置文件。");
        }

        public override Castle.Core.Logging.ILogger Create(string name)
        {
            return new Log4netLogger(LogManager.GetLogger("", name), this);
        }
    }
}
