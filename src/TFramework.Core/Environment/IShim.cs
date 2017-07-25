namespace TFramework.Core.Environment
{
    /// <summary>
    /// 需要访问HostContainer实例的ASP.NET单例服务的垫片实现的接口。
    /// </summary>
    public interface IShim
    {
        IHostContainer HostContainer { get; set; }
    }
}
