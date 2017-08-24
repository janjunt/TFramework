using TFramework.Core.Localization;
using TFramework.Core.Logging;

namespace TFramework.Core
{
    /// <summary>
    /// 每工作单元（即Web请求）实例化服务的基接口。
    /// </summary>
    public interface IDependency
    {
    }

    /// <summary>
    /// 每shell/租户实例化服务的基接口。
    /// </summary>
    public interface ISingletonDependency : IDependency
    {
    }

    /// <summary>
    /// 仅在工作单元实例化服务的基接口。
    /// 此接口用于保证它们不被单例依赖意外的引用。
    /// </summary>
    public interface IUnitOfWorkDependency : IDependency
    {
    }

    /// <summary>
    /// 每次使用时实例化服务的基接口
    /// </summary>
    public interface ITransientDependency : IDependency
    {
    }


    public abstract class Component : IDependency
    {
        protected Component()
        {
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }
    }
}
