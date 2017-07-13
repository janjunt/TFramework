namespace TFramework.Core
{
    /// <summary>
    /// 每工作单元（即Web请求）实例化服务的基础接口。
    /// </summary>
    public interface IDependency
    {
    }

    /// <summary>
    /// 每shell/租户实例化服务的基础接口。
    /// </summary>
    public interface ISingletonDependency : IDependency
    {
    }

    /// <summary>
    /// Base interface for services that may *only* be instantiated in a unit of work.
    /// This interface is used to guarantee they are not accidentally referenced by a singleton dependency.
    /// </summary>
    public interface IUnitOfWorkDependency : IDependency
    {
    }

    /// <summary>
    /// Base interface for services that are instantiated per usage.
    /// </summary>
    public interface ITransientDependency : IDependency
    {
    }


    //public abstract class Component : IDependency
    //{
    //    protected Component()
    //    {
    //        Logger = NullLogger.Instance;
    //        T = NullLocalizer.Instance;
    //    }

    //    public ILogger Logger { get; set; }
    //    public Localizer T { get; set; }
    //}
}
