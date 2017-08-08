using TFramework.Core.Environment.Configuration;
using TFramework.Core.Environment.ShellBuilders;

namespace TFramework.Core.Environment
{
    public interface IHost
    {
        /// <summary>
        /// 启动时调用一次即可配置应用程序域，同时加载/应用已存在的shell配置
        /// </summary>
        void Initialize();

        /// <summary>
        /// 当明确知道已安装的模块/扩展列表已更改，并且需要重新加载时，从外部调用。
        /// </summary>
        void ReloadExtensions();

        /// <summary>
        /// 在每次请求开始时，提供即时重新初始化的调用点
        /// </summary>
        void BeginRequest();

        /// <summary>
        /// 在每次请求结束时，用以确定性地提交和处理未完成的活动时调用
        /// </summary>
        void EndRequest();

        ShellContext GetShellContext(ShellSettings shellSettings);

        /// <summary>
        /// 可用于构建shell配置代码的临时自包含实例。
        /// 服务可以从此实例中解析，用于配置并初始化它的存储
        /// 在这种情况下可以解决服务来配置和初始化它的存储。
        /// </summary>
        IWorkContextScope CreateStandaloneEnvironment(ShellSettings shellSettings);
    }
}
