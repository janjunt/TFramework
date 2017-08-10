using System.Collections.Generic;
using System.Linq;

namespace TFramework.Core.Environment.Descriptor.Models
{
    /// <summary>
    /// 包含租户启用功能的快照。
    /// 信息通过IShellDescriptorManager从shell中抽取，并通过IShellDescriptorCache缓存到主机。
    /// 它传递给ICompositionStrategy来构建ShellBlueprint
    /// </summary>
    public class ShellDescriptor
    {
        public ShellDescriptor()
        {
            Features = Enumerable.Empty<ShellFeature>();
            Parameters = Enumerable.Empty<ShellParameter>();
        }

        public int SerialNumber { get; set; }
        public IEnumerable<ShellFeature> Features { get; set; }
        public IEnumerable<ShellParameter> Parameters { get; set; }
    }

    public class ShellFeature
    {
        public string Name { get; set; }
    }

    public class ShellParameter
    {
        public string Component { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
