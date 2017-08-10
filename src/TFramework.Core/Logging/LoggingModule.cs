using Autofac;
using Autofac.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;

namespace TFramework.Core.Logging
{
    public class LoggingModule : Module
    {
        private readonly ConcurrentDictionary<string, ILogger> _loggerCache;

        public LoggingModule()
        {
            _loggerCache = new ConcurrentDictionary<string, ILogger>();
        }

        protected override void Load(ContainerBuilder moduleBuilder)
        {
            moduleBuilder.RegisterType<CastleLoggerFactory>().As<ILoggerFactory>().InstancePerLifetimeScope();
            moduleBuilder.RegisterType<Log4netFactory>()
                .As<Castle.Core.Logging.ILoggerFactory>()
                .InstancePerLifetimeScope();

            moduleBuilder.Register(CreateLogger).As<ILogger>().InstancePerDependency();
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry,
            IComponentRegistration registration)
        {
            var implementationType = registration.Activator.LimitType;

            // 根据此类型构建一个actions数组用于把logger赋值给成员属性
            var injectors = BuildLoggerInjectors(implementationType).ToArray();

            // 如果没有logger属性，就没有需要活动事件钩子的理由
            if (!injectors.Any())
                return;

            // 否则，如果激活一个该组件的实例，则在实例上注入loggers
            registration.Activated += (s, e) =>
            {
                foreach (var injector in injectors)
                    injector(e.Context, e.Instance);
            };
        }

        private IEnumerable<Action<IComponentContext, object>> BuildLoggerInjectors(Type componentType)
        {
            // 寻找类型为“ILogger”的可写属性
            var loggerProperties = componentType
                .GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
                .Select(p => new
                {
                    PropertyInfo = p,
                    p.PropertyType,
                    IndexParameters = p.GetIndexParameters(),
                    Accessors = p.GetAccessors(false)
                })
                .Where(x => x.PropertyType == typeof (ILogger)) // 必须是logger类型
                .Where(x => x.IndexParameters.Count() == 0) // 不能是索引器
                .Where(x => x.Accessors.Length != 1 || x.Accessors[0].ReturnType == typeof (void));
            // 必须是可读可写或可写属性

            // 返回一个actions数组用于解析logger并赋值给属性
            foreach (var entry in loggerProperties)
            {
                var propertyInfo = entry.PropertyInfo;

                yield return (ctx, instance) =>
                {
                    string component = componentType.ToString();
                    if (component != instance.GetType().ToString())
                    {
                        return;
                    }
                    var logger = _loggerCache.GetOrAdd(component,
                        key => ctx.Resolve<ILogger>(new TypedParameter(typeof (Type), componentType)));
                    propertyInfo.SetValue(instance, logger, null);
                };
            }
        }

        private static ILogger CreateLogger(IComponentContext context, IEnumerable<Parameter> parameters)
        {
            var loggerFactory = context.Resolve<ILoggerFactory>();
            var containingType = parameters.TypedAs<Type>();
            return loggerFactory.CreateLogger(containingType);
        }
    }
}
