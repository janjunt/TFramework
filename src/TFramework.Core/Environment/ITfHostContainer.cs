namespace TFramework.Core.Environment
{
    public interface ITfHostContainer
    {
        T Resolve<T>();
    }
}
