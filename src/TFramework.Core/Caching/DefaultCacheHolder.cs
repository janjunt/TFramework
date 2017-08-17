using System;
using System.Collections.Concurrent;

namespace TFramework.Core.Caching
{
    /// <summary>
    /// 提供缓存持有者的默认实现。
    /// 缓存持有者是负责实际存储缓存实体的引用。
    /// </summary>
    public class DefaultCacheHolder : ICacheHolder
    {
        private readonly ICacheContextAccessor _cacheContextAccessor;
        private readonly ConcurrentDictionary<CacheKey, object> _caches = new ConcurrentDictionary<CacheKey, object>();

        public DefaultCacheHolder(ICacheContextAccessor cacheContextAccessor)
        {
            _cacheContextAccessor = cacheContextAccessor;
        }

        class CacheKey : Tuple<Type, Type, Type>
        {
            public CacheKey(Type component, Type key, Type result)
                : base(component, key, result)
            {
            }
        }

        /// <summary>
        /// 从缓存中获取一个缓存入口。如果没有找到，则创建一个空的并返回。
        /// </summary>
        /// <typeparam name="TKey">组件中的键类型。</typeparam>
        /// <typeparam name="TResult">结果类型。</typeparam>
        /// <param name="component">组件上下文。</param>
        /// <returns>缓存入口，或一个新的，空的，如果没有找到。</returns>
        public ICache<TKey, TResult> GetCache<TKey, TResult>(Type component)
        {
            var cacheKey = new CacheKey(component, typeof(TKey), typeof(TResult));
            var result = _caches.GetOrAdd(cacheKey, k => new Cache<TKey, TResult>(_cacheContextAccessor));
            return (Cache<TKey, TResult>)result;
        }
    }
}
