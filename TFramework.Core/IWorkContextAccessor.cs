using Microsoft.AspNetCore.Http;
using System;

namespace TFramework.Core
{
    public interface IWorkContextAccessor
    {
        WorkContext GetContext(HttpContext httpContext);
        IWorkContextScope CreateWorkContextScope(HttpContext httpContext);

        WorkContext GetContext();
        IWorkContextScope CreateWorkContextScope();
    }

    public interface IWorkContextStateProvider : IDependency
    {
        Func<WorkContext, T> Get<T>(string name);
    }

    public interface IWorkContextScope : IDisposable
    {
        WorkContext WorkContext { get; }
        TService Resolve<TService>();
        bool TryResolve<TService>(out TService service);
    }
}
