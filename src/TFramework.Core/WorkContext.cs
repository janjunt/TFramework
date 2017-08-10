using Microsoft.AspNetCore.Http;
using System;
using TFramework.Core.Environment.Extensions.Models;
using TFramework.Core.Security;
using TFramework.Core.Settings;

namespace TFramework.Core
{
    /// <summary>
    /// 用于工作上下文作用域的工作上下文
    /// </summary>
    public abstract class WorkContext
    {
        /// <summary>
        /// 解析注册的依赖类型
        /// </summary>
        /// <typeparam name="T">依赖的类型</typeparam>
        /// <returns>如果可以解析，返回一个依赖的实例</returns>
        public abstract T Resolve<T>();

        /// <summary>
        /// 尝试解析注册的依赖类型
        /// </summary>
        /// <typeparam name="T">依赖的类型</typeparam>
        /// <param name="service">如果可以解析，返回一个依赖的实例</param>
        /// <returns>如果可以解析依赖类型，则为true，否则为false</returns>
        public abstract bool TryResolve<T>(out T service);

        public abstract T GetState<T>(string name);
        public abstract void SetState<T>(string name, T value);

        /// <summary>
        /// 对应于工作上下文的http上下文
        /// </summary>
        public HttpContext HttpContext
        {
            get { return GetState<HttpContext>("HttpContext"); }
            set { SetState("HttpContext", value); }
        }

        /// <summary>
        /// 对应于工作上下文的布局形状
        /// </summary>
        public dynamic Layout
        {
            get { return GetState<dynamic>("Layout"); }
            set { SetState("Layout", value); }
        }

        /// <summary>
        /// 对应于工作环境的站点设置
        /// </summary>
        public ISite CurrentSite
        {
            get { return GetState<ISite>("CurrentSite"); }
            set { SetState("CurrentSite", value); }
        }

        /// <summary>
        /// 对应于工作上下文的任意用户
        /// </summary>
        public IUser CurrentUser
        {
            get { return GetState<IUser>("CurrentUser"); }
            set { SetState("CurrentUser", value); }
        }

        /// <summary>
        /// 在工作环境中使用的主题
        /// </summary>
        public ExtensionDescriptor CurrentTheme
        {
            get { return GetState<ExtensionDescriptor>("CurrentTheme"); }
            set { SetState("CurrentTheme", value); }
        }

        /// <summary>
        /// 工作上下文的活动语言文化
        /// </summary>
        public string CurrentCulture
        {
            get { return GetState<string>("CurrentCulture"); }
            set { SetState("CurrentCulture", value); }
        }

        /// <summary>
        /// 工作上下文的活动日历
        /// </summary>
        public string CurrentCalendar
        {
            get { return GetState<string>("CurrentCalendar"); }
            set { SetState("CurrentCalendar", value); }
        }

        /// <summary>
        /// 工作上下文的时区
        /// </summary>
        public TimeZoneInfo CurrentTimeZone
        {
            get { return GetState<TimeZoneInfo>("CurrentTimeZone"); }
            set { SetState("CurrentTimeZone", value); }
        }
    }
}
