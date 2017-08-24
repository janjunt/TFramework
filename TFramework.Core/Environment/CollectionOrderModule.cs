using Autofac.Core;
using System;
using System.Collections;
using System.Reflection;

namespace TFramework.Core.Environment
{
    internal class CollectionOrderModule : IModule
    {
        public void Configure(IComponentRegistry componentRegistry)
        {
            componentRegistry.Registered += (sender, registered) => {
                // 仅处理枚举的解析
                var limitType = registered.ComponentRegistration.Activator.LimitType;
                if (typeof(IEnumerable).IsAssignableFrom(limitType))
                {
                    registered.ComponentRegistration.Activated += (sender2, activated) => {
                        // Autofac's IEnumerable解析返回的是一个数组
                        if (activated.Instance is Array)
                        {
                            // 组件的顺序需要FIFO，不是FILO
                            Array.Reverse((Array)activated.Instance);
                        }
                    };
                }
            };
        }
    }
}
