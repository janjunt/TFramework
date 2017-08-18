using System.Collections.Generic;
using TFramework.Core.Caching;

namespace TFramework.Core.Environment
{
    /// <summary>
    /// 提供将Shims连接到HostContainer的能力
    /// </summary>
    public class TfHostContainerRegistry
    {
        private static readonly IList<Weak<IShim>> _shims = new List<Weak<IShim>>();
        private static ITfHostContainer _hostContainer;
        private static readonly object _syncLock = new object();

        public static void RegisterShim(IShim shim)
        {
            lock (_syncLock)
            {
                CleanupShims();

                _shims.Add(new Weak<IShim>(shim));
                shim.HostContainer = _hostContainer;
            }
        }

        public static void RegisterHostContainer(ITfHostContainer container)
        {
            lock (_syncLock)
            {
                CleanupShims();

                _hostContainer = container;
                RegisterContainerInShims();
            }
        }

        private static void RegisterContainerInShims()
        {
            foreach (var shim in _shims)
            {
                var target = shim.Target;
                if (target != null)
                {
                    target.HostContainer = _hostContainer;
                }
            }
        }

        private static void CleanupShims()
        {
            for (int i = _shims.Count - 1; i >= 0; i--)
            {
                if (_shims[i].Target == null)
                    _shims.RemoveAt(i);
            }
        }
    }
}
