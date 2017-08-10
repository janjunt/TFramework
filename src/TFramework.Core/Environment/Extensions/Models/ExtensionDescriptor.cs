using System.Collections.Generic;

namespace TFramework.Core.Environment.Extensions.Models
{
    public class ExtensionDescriptor
    {
        /// <summary>
        /// 虚拟基路径，"~/Themes"，"~/Modules"，或"~/Core"
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 虚拟基路径下的文件夹名称
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 扩展类型。
        /// </summary>
        public string ExtensionType { get; set; }

        #region 扩展元数据
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string OrchardVersion { get; set; }
        public string Author { get; set; }
        public string WebSite { get; set; }
        public string Tags { get; set; }
        public string AntiForgery { get; set; }
        public string Zones { get; set; }
        public string BaseTheme { get; set; }
        public string SessionState { get; set; }
        #endregion

        public IEnumerable<FeatureDescriptor> Features { get; set; }
    }
}
