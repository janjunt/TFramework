using Autofac;
using TFramework.Core.Environment.Configuration;
using TFramework.Core.Environment.Descriptor.Models;
using TFramework.Core.Environment.ShellBuilders.Models;

namespace TFramework.Core.Environment.ShellBuilders
{
    public class ShellContext
    {
        public ShellSettings Settings { get; set; }
        public ShellDescriptor Descriptor { get; set; }
        public ShellBlueprint Blueprint { get; set; }
        public ILifetimeScope LifetimeScope { get; set; }
        public IShell Shell { get; set; }
    }
}
