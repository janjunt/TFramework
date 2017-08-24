namespace TFramework.Core.Caching
{
    public interface ICacheContextAccessor
    {
        IAcquireContext Current { get; set; }
    }
}
