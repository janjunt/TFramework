namespace TFramework.Core.Caching
{
    public interface IVolatileToken
    {
        bool IsCurrent { get; }
    }
}
