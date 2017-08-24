namespace TFramework.Core.Environment
{
    /// <summary>
    /// 运行环境的抽像接口
    /// </summary>
    public interface IHostEnvironment
    {
        bool IsFullTrust { get; }
        string MapPath(string virtualPath);

        bool IsAssemblyLoaded(string name);

        void RestartAppDomain();
    }
}
