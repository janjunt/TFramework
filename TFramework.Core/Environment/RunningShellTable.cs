using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TFramework.Core.Environment.Configuration;

namespace TFramework.Core.Environment
{
    public interface IRunningShellTable
    {
        void Add(ShellSettings settings);
        void Remove(ShellSettings settings);
        void Update(ShellSettings settings);
        ShellSettings Match(HttpContext httpContext);
        ShellSettings Match(string host, string appRelativeCurrentExecutionFilePath);
    }

    public class RunningShellTable : IRunningShellTable
    {
        private IEnumerable<ShellSettings> _shells = Enumerable.Empty<ShellSettings>();
        private IDictionary<string, IEnumerable<ShellSettings>> _shellsByHost;
        private readonly ConcurrentDictionary<string, ShellSettings> _shellsByHostAndPrefix = new ConcurrentDictionary<string, ShellSettings>(StringComparer.OrdinalIgnoreCase);

        private ShellSettings _fallback;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public void Add(ShellSettings settings)
        {
            _lock.EnterWriteLock();
            try
            {
                _shells = _shells
                    .Where(s => s.Name != settings.Name)
                    .Concat(new[] { settings })
                    .ToArray();

                Organize();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Remove(ShellSettings settings)
        {
            _lock.EnterWriteLock();
            try
            {
                _shells = _shells
                    .Where(s => s.Name != settings.Name)
                    .ToArray();

                Organize();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Update(ShellSettings settings)
        {
            _lock.EnterWriteLock();
            try
            {
                _shells = _shells
                    .Where(s => s.Name != settings.Name)
                    .ToArray();

                _shells = _shells
                    .Concat(new[] { settings })
                    .ToArray();

                Organize();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        private void Organize()
        {
            var qualified =
                _shells.Where(x => !string.IsNullOrEmpty(x.RequestUrlHost) || !string.IsNullOrEmpty(x.RequestUrlPrefix));

            var unqualified = _shells
                .Where(x => string.IsNullOrEmpty(x.RequestUrlHost) && string.IsNullOrEmpty(x.RequestUrlPrefix))
                .ToList();

            _shellsByHost = qualified
                .SelectMany(s => s.RequestUrlHost == null || s.RequestUrlHost.IndexOf(',') == -1 ? new[] { s } :
                    s.RequestUrlHost.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                     .Select(h => new ShellSettings(s) { RequestUrlHost = h }))
                .GroupBy(s => s.RequestUrlHost ?? string.Empty)
                .OrderByDescending(g => g.Key.Length)
                .ToDictionary(x => x.Key, x => x.AsEnumerable(), StringComparer.OrdinalIgnoreCase);

            if (unqualified.Count() == 1)
            {
                // 只有一个shell没有请求url标准
                _fallback = unqualified.Single();
            }
            else if (unqualified.Any())
            {
                // 两个或两个以上的shell没有请求标准。
                // 这在技术上是一个错误的配置 - 如果它是一个将捕获所有请求的回退到默认shell
                _fallback = unqualified.SingleOrDefault(x => x.Name == ShellSettings.DefaultName);
            }
            else
            {
                // 没有shell不合格 - 不符合shell规范的请求将不会映射到来自框架的路由
                _fallback = null;
            }

            _shellsByHostAndPrefix.Clear();
        }

        public ShellSettings Match(HttpContext httpContext)
        {
            var httpRequest = httpContext.Request;
            if (httpRequest == null)
            {
                return null;
            }

            var host = httpRequest.Headers["Host"].ToString();
            var appRelativeCurrentExecutionFilePath = httpRequest.Path.ToString();

            return Match(host ?? string.Empty, appRelativeCurrentExecutionFilePath);
        }

        public ShellSettings Match(string host, string appRelativePath)
        {
            _lock.EnterReadLock();
            try
            {
                if (_shellsByHost == null)
                {
                    return null;
                }

                // 只有一个租户（默认）时，没有配置自定义主机。
                if (!_shellsByHost.Any() && _fallback != null)
                {
                    return _fallback;
                }

                // 从主机中删除端口
                var hostLength = host.IndexOf(':');
                if (hostLength != -1)
                {
                    host = host.Substring(0, hostLength);
                }

                string hostAndPrefix = host + "/" + appRelativePath.Split('/')[1];

                return _shellsByHostAndPrefix.GetOrAdd(hostAndPrefix, key => {

                    // 根据主机过滤shell
                    IEnumerable<ShellSettings> shells;

                    if (!_shellsByHost.TryGetValue(host, out shells))
                    {
                        if (!_shellsByHost.TryGetValue("", out shells))
                        {

                            // 没有具体的匹配，那么通过*号映射寻找
                            var subHostKey = _shellsByHost.Keys.FirstOrDefault(x =>
                                x.StartsWith("*.") && host.EndsWith(x.Substring(2))
                                );

                            if (subHostKey == null)
                            {
                                return _fallback;
                            }

                            shells = _shellsByHost[subHostKey];
                        }
                    }

                    // 寻找一个请求url前缀的匹配
                    var mostQualifiedMatch = shells.FirstOrDefault(settings => {
                        if (settings.State == TenantState.Disabled)
                        {
                            return false;
                        }

                        if (String.IsNullOrWhiteSpace(settings.RequestUrlPrefix))
                        {
                            return true;
                        }

                        return key.Equals(host + "/" + settings.RequestUrlPrefix, StringComparison.OrdinalIgnoreCase);
                    });

                    return mostQualifiedMatch ?? _fallback;
                });

            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}
