using System;
using System.Collections.Generic;
using TFramework.Core.Environment.Configuration;
using TFramework.Core.Environment.Descriptor.Models;
using TFramework.Core.Environment.Extensions.Models;

namespace TFramework.Core.Environment.ShellBuilders.Models
{
    /// <summary>
    /// 包含为特定租户初始化IoC容器所需的信息。
    /// 该模型由ICompositionStrategy创建，并被传入IShellContainerFactory。
    /// </summary>
    public class ShellBlueprint
    {
        public ShellSettings Settings { get; set; }
        public ShellDescriptor Descriptor { get; set; }

        public IEnumerable<DependencyBlueprint> Dependencies { get; set; }
        public IEnumerable<ControllerBlueprint> Controllers { get; set; }
        public IEnumerable<ControllerBlueprint> HttpControllers { get; set; }
        public IEnumerable<RecordBlueprint> Records { get; set; }
    }

    public class ShellBlueprintItem
    {
        public Type Type { get; set; }
        public Feature Feature { get; set; }
    }

    public class DependencyBlueprint : ShellBlueprintItem
    {
        public IEnumerable<ShellParameter> Parameters { get; set; }
    }

    public class ControllerBlueprint : ShellBlueprintItem
    {
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
    }

    public class RecordBlueprint : ShellBlueprintItem
    {
        public string TableName { get; set; }
    }
}
