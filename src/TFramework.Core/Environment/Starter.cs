using Autofac;
using System;
using TFramework.Core.Caching;
using TFramework.Core.Events;
using TFramework.Core.Logging;

namespace TFramework.Core.Environment
{
    public static class Starter
    {
        public static IContainer CreateHostContainer(Action<ContainerBuilder> registrations)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new CollectionOrderModule());
            builder.RegisterModule(new LoggingModule());
            builder.RegisterModule(new EventsModule());
            builder.RegisterModule(new CacheModule());


            var container = builder.Build();


            return container;
        }

        private static void RegisterVolatileProvider<TRegister, TService>(ContainerBuilder builder) where TService : IVolatileProvider
        {
            builder.RegisterType<TRegister>()
                .As<TService>()
                .As<IVolatileProvider>()
                .SingleInstance();
        }

        public static IHost CreateHost(Action<ContainerBuilder> registrations)
        {
            var container = CreateHostContainer(registrations);

            return container.Resolve<IHost>();
        }

    }
}
