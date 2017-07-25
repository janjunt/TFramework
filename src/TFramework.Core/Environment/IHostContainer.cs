namespace TFramework.Core.Environment
{
    public interface IHostContainer
    {
        T Resolve<T>();
    }
}
