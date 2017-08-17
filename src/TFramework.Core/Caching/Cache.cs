using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TFramework.Core.Caching
{
    public class Cache<TKey, TResult> : ICache<TKey, TResult>
    {
        private readonly ICacheContextAccessor _cacheContextAccessor;
        private readonly ConcurrentDictionary<TKey, CacheEntry> _entries;

        public Cache(ICacheContextAccessor cacheContextAccessor)
        {
            _cacheContextAccessor = cacheContextAccessor;
            _entries = new ConcurrentDictionary<TKey, CacheEntry>();
        }

        public TResult Get(TKey key, Func<AcquireContext<TKey>, TResult> acquire)
        {
            var entry = _entries.AddOrUpdate(key,
                // 新增lambda
                k => AddEntry(k, acquire),
                // 更新lambda
                (k, currentEntry) => UpdateEntry(currentEntry, k, acquire));

            return entry.Result;
        }

        private CacheEntry AddEntry(TKey k, Func<AcquireContext<TKey>, TResult> acquire)
        {
            var entry = CreateEntry(k, acquire);
            PropagateTokens(entry);
            return entry;
        }

        private CacheEntry UpdateEntry(CacheEntry currentEntry, TKey k, Func<AcquireContext<TKey>, TResult> acquire)
        {
            var entry = (currentEntry.Tokens.Any(t => t != null && !t.IsCurrent)) ? CreateEntry(k, acquire) : currentEntry;
            PropagateTokens(entry);
            return entry;
        }

        private void PropagateTokens(CacheEntry entry)
        {
            // 将可变tokens冒泡到父级上下文
            if (_cacheContextAccessor.Current != null)
            {
                foreach (var token in entry.Tokens)
                    _cacheContextAccessor.Current.Monitor(token);
            }
        }


        private CacheEntry CreateEntry(TKey k, Func<AcquireContext<TKey>, TResult> acquire)
        {
            var entry = new CacheEntry();
            var context = new AcquireContext<TKey>(k, entry.AddToken);

            IAcquireContext parentContext = null;
            try
            {
                // 压入context
                parentContext = _cacheContextAccessor.Current;
                _cacheContextAccessor.Current = context;

                entry.Result = acquire(context);
            }
            finally
            {
                // 弹出context
                _cacheContextAccessor.Current = parentContext;
            }
            entry.CompactTokens();
            return entry;
        }

        private class CacheEntry
        {
            private IList<IVolatileToken> _tokens;
            public TResult Result { get; set; }

            public IEnumerable<IVolatileToken> Tokens
            {
                get
                {
                    return _tokens ?? Enumerable.Empty<IVolatileToken>();
                }
            }

            public void AddToken(IVolatileToken volatileToken)
            {
                if (_tokens == null)
                {
                    _tokens = new List<IVolatileToken>();
                }

                _tokens.Add(volatileToken);
            }

            public void CompactTokens()
            {
                if (_tokens != null)
                    _tokens = _tokens.Distinct().ToArray();
            }
        }
    }
}
