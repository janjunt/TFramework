using System;

namespace TFramework.Core.Caching
{
    /// <summary>
    /// 提供缓存管理器的默认实现。
    /// 缓存管理器提供在缓存持有者上的抽象，允许其轻松的在组件上下文中进行交换并将其隔离。
    /// </summary>
    public class DefaultCacheManager : ICacheManager
    {
        private readonly Type _component;
        private readonly ICacheHolder _cacheHolder;

        /// <summary>
        /// 为给定的组件类型和缓存持有者的实现构造一个新的缓存管理器。
        /// </summary>
        /// <param name="component">应用缓存的组件（上下文）</param>
        /// <param name="cacheHolder">包含缓存实体的缓存持有者</param>
        public DefaultCacheManager(Type component, ICacheHolder cacheHolder)
        {
            _component = component;
            _cacheHolder = cacheHolder;
        }

        /// <summary>
        /// 从缓存持有者中获取一个缓存入口。
        /// </summary>
        /// <typeparam name="TKey">用于获取缓存项的键类型。</typeparam>
        /// <typeparam name="TResult">要从缓存获取的项类型。</typeparam>
        /// <returns>缓存入口。</returns>
        public ICache<TKey, TResult> GetCache<TKey, TResult>()
        {
            return _cacheHolder.GetCache<TKey, TResult>(_component);
        }

        public TResult Get<TKey, TResult>(TKey key, Func<AcquireContext<TKey>, TResult> acquire)
        {
            return GetCache<TKey, TResult>().Get(key, acquire);
        }
    }
}
