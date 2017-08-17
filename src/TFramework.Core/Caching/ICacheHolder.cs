using System;

namespace TFramework.Core.Caching
{
    public interface ICacheHolder : ISingletonDependency
    {
        ICache<TKey, TResult> GetCache<TKey, TResult>(Type component);
    }
}
