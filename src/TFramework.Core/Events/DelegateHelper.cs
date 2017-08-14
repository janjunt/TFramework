using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TFramework.Events
{
    /// <summary>
    /// 委托帮助类
    /// </summary>
    public static class DelegateHelper
    {
        /// <summary>
        /// 创建一个强类型的动态委托，表示指定目标类型上的指定方法
        /// </summary>
        /// <typeparam name="T">委托目标的类型</typeparam>
        /// <param name="method">委托指定的方法</param>
        /// <returns>表示指定方法的强类型委托</returns>
        public static Func<T, object[], object> CreateDelegate<T>(MethodInfo method)
        {
            return CreateDelegate<T>(typeof(T), method);
        }

        /// <summary>
        /// 创建一个强类型的动态委托，表示指定目标类型上的指定方法
        /// </summary>
        /// <remarks>
        /// 提供的方法必须是目标类型上有效的方法
        /// </remarks>
        /// <typeparam name="T">委托目标的类型</typeparam>
        /// <param name="targetType">委托实际目标类型</param>
        /// <param name="method">委托指定的方法</param>
        /// <returns>表示指定方法的强类型委托</returns>
        public static Func<T, object[], object> CreateDelegate<T>(Type targetType, MethodInfo method)
        {
            var parameters = method.ReturnType == typeof(void)
                ? new[] { targetType }.Concat(method.GetParameters().Select(p => p.ParameterType)).ToArray()
                : new[] { targetType }.Concat(method.GetParameters().Select(p => p.ParameterType)).Concat(new[] { method.ReturnType }).ToArray();

            MethodInfo genericHelper = method.ReturnType == typeof(void)
                ? typeof(DelegateHelper).GetMethods(BindingFlags.Static | BindingFlags.NonPublic).First(m => m.Name == "BuildAction" && m.GetGenericArguments().Count() == parameters.Length)
                : typeof(DelegateHelper).GetMethods(BindingFlags.Static | BindingFlags.NonPublic).First(m => m.Name == "BuildFunc" && m.GetGenericArguments().Count() == parameters.Length);

            MethodInfo constructedHelper = genericHelper.MakeGenericMethod(parameters);

            object ret = constructedHelper.Invoke(null, new object[] { method });
            return (Func<T, object[], object>)ret;
        }

        /// <summary>
        /// 生成一个强类型的动态委托，表示指定目标类型上的指定方法
        /// </summary>
        /// <typeparam name="T">委托目标类型</typeparam>
        /// <param name="method">委托指定的方法</param>
        /// <returns>表示指定方法的强类型委托</returns>        
        static Delegate BuildDynamicDelegate<T>(MethodInfo method)
        {
            var parameter = Expression.Parameter(typeof(T));
            var caller = Expression.Call(parameter, method, method.GetParameters().Select(p => Expression.Parameter(p.ParameterType)));
            var e = Expression.Lambda(caller, parameter);

            return e.Compile();
        }

        static Func<object, object[], object> BuildFunc<T, TRet>(MethodInfo method)
        {
            var func = (Func<T, TRet>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => func((T)target);
            return ret;
        }

        static Func<object, object[], object> BuildFunc<T, T1, TRet>(MethodInfo method)
        {
            var func = (Func<T, T1, TRet>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => func((T)target, (T1)p[0]);
            return ret;
        }

        static Func<object, object[], object> BuildFunc<T, T1, T2, TRet>(MethodInfo method)
        {
            var func = (Func<T, T1, T2, TRet>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => func((T)target, (T1)p[0], (T2)p[1]);
            return ret;
        }

        static Func<object, object[], object> BuildFunc<T, T1, T2, T3, TRet>(MethodInfo method)
        {
            var func = (Func<T, T1, T2, T3, TRet>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => func((T)target, (T1)p[0], (T2)p[1], (T3)p[2]);
            return ret;
        }

        static Func<object, object[], object> BuildFunc<T, T1, T2, T3, T4, TRet>(MethodInfo method)
        {
            var func = (Func<T, T1, T2, T3, T4, TRet>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => func((T)target, (T1)p[0], (T2)p[1], (T3)p[2], (T4)p[3]);
            return ret;
        }

        static Func<object, object[], object> BuildFunc<T, T1, T2, T3, T4, T5, TRet>(MethodInfo method)
        {
            var func = (Func<T, T1, T2, T3, T4, T5, TRet>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => func((T)target, (T1)p[0], (T2)p[1], (T3)p[2], (T4)p[3], (T5)p[4]);
            return ret;
        }

        static Func<object, object[], object> BuildFunc<T, T1, T2, T3, T4, T5, T6, TRet>(MethodInfo method)
        {
            var func = (Func<T, T1, T2, T3, T4, T5, T6, TRet>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => func((T)target, (T1)p[0], (T2)p[1], (T3)p[2], (T4)p[3], (T5)p[4], (T6)p[5]);
            return ret;
        }

        static Func<object, object[], object> BuildFunc<T, T1, T2, T3, T4, T5, T6, T7, TRet>(MethodInfo method)
        {
            var func = (Func<T, T1, T2, T3, T4, T5, T6, T7, TRet>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => func((T)target, (T1)p[0], (T2)p[1], (T3)p[2], (T4)p[3], (T5)p[4], (T6)p[5], (T7)p[6]);
            return ret;
        }

        static Func<object, object[], object> BuildFunc<T, T1, T2, T3, T4, T5, T6, T7, T8, TRet>(MethodInfo method)
        {
            var func = (Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TRet>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => func((T)target, (T1)p[0], (T2)p[1], (T3)p[2], (T4)p[3], (T5)p[4], (T6)p[5], (T7)p[6], (T8)p[7]);
            return ret;
        }

        static Func<object, object[], object> BuildFunc<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>(MethodInfo method)
        {
            var func = (Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => func((T)target, (T1)p[0], (T2)p[1], (T3)p[2], (T4)p[3], (T5)p[4], (T6)p[5], (T7)p[6], (T8)p[7], (T9)p[8]);
            return ret;
        }

        static Func<object, object[], object> BuildFunc<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>(MethodInfo method)
        {
            var func = (Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => func((T)target, (T1)p[0], (T2)p[1], (T3)p[2], (T4)p[3], (T5)p[4], (T6)p[5], (T7)p[6], (T8)p[7], (T9)p[8], (T10)p[9]);
            return ret;
        }

        static Func<object, object[], object> BuildAction<T>(MethodInfo method)
        {
            var func = (Action<T>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => { func((T)target); return null; };
            return ret;
        }

        static Func<object, object[], object> BuildAction<T, T1>(MethodInfo method)
        {
            var func = (Action<T, T1>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => { func((T)target, (T1)p[0]); return null; };
            return ret;
        }

        static Func<object, object[], object> BuildAction<T, T1, T2>(MethodInfo method)
        {
            var func = (Action<T, T1, T2>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => { func((T)target, (T1)p[0], (T2)p[1]); return null; };
            return ret;
        }

        static Func<object, object[], object> BuildAction<T, T1, T2, T3>(MethodInfo method)
        {
            var func = (Action<T, T1, T2, T3>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => { func((T)target, (T1)p[0], (T2)p[1], (T3)p[2]); return null; };
            return ret;
        }

        static Func<object, object[], object> BuildAction<T, T1, T2, T3, T4>(MethodInfo method)
        {
            var func = (Action<T, T1, T2, T3, T4>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => { func((T)target, (T1)p[0], (T2)p[1], (T3)p[2], (T4)p[3]); return null; };
            return ret;
        }

        static Func<object, object[], object> BuildAction<T, T1, T2, T3, T4, T5>(MethodInfo method)
        {
            var func = (Action<T, T1, T2, T3, T4, T5>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => { func((T)target, (T1)p[0], (T2)p[1], (T3)p[2], (T4)p[3], (T5)p[4]); return null; };
            return ret;
        }

        static Func<object, object[], object> BuildAction<T, T1, T2, T3, T4, T5, T6>(MethodInfo method)
        {
            var func = (Action<T, T1, T2, T3, T4, T5, T6>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => { func((T)target, (T1)p[0], (T2)p[1], (T3)p[2], (T4)p[3], (T5)p[4], (T6)p[5]); return null; };
            return ret;
        }

        static Func<object, object[], object> BuildAction<T, T1, T2, T3, T4, T5, T6, T7>(MethodInfo method)
        {
            var func = (Action<T, T1, T2, T3, T4, T5, T6, T7>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => { func((T)target, (T1)p[0], (T2)p[1], (T3)p[2], (T4)p[3], (T5)p[4], (T6)p[5], (T7)p[6]); return null; };
            return ret;
        }

        static Func<object, object[], object> BuildAction<T, T1, T2, T3, T4, T5, T6, T7, T8>(MethodInfo method)
        {
            var func = (Action<T, T1, T2, T3, T4, T5, T6, T7, T8>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => { func((T)target, (T1)p[0], (T2)p[1], (T3)p[2], (T4)p[3], (T5)p[4], (T6)p[5], (T7)p[6], (T8)p[7]); return null; };
            return ret;
        }

        static Func<object, object[], object> BuildAction<T, T1, T2, T3, T4, T5, T6, T7, T8, T9>(MethodInfo method)
        {
            var func = (Action<T, T1, T2, T3, T4, T5, T6, T7, T8, T9>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => { func((T)target, (T1)p[0], (T2)p[1], (T3)p[2], (T4)p[3], (T5)p[4], (T6)p[5], (T7)p[6], (T8)p[7], (T9)p[8]); return null; };
            return ret;
        }

        static Func<object, object[], object> BuildAction<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(MethodInfo method)
        {
            var func = (Action<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)BuildDynamicDelegate<T>(method);
            Func<object, object[], object> ret = (target, p) => { func((T)target, (T1)p[0], (T2)p[1], (T3)p[2], (T4)p[3], (T5)p[4], (T6)p[5], (T7)p[6], (T8)p[7], (T9)p[8], (T10)p[9]); return null; };
            return ret;
        }
    }
}
