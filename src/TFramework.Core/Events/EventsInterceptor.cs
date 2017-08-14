using Castle.DynamicProxy;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TFramework.Core.Events
{
    public class EventsInterceptor : IInterceptor
    {
        private readonly IEventBus _eventBus;
        private static readonly ConcurrentDictionary<Type, MethodInfo> _enumerableOfTypeTDictionary = new ConcurrentDictionary<Type, MethodInfo>();

        public EventsInterceptor(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Intercept(IInvocation invocation)
        {
            var interfaceName = invocation.Method.DeclaringType.Name;
            var methodName = invocation.Method.Name;

            var data = invocation.Method.GetParameters()
                .Select((parameter, index) => new { parameter.Name, Value = invocation.Arguments[index] })
                .ToDictionary(kv => kv.Name, kv => kv.Value);

            var results = _eventBus.Notify(interfaceName + "." + methodName, data);

            invocation.ReturnValue = Adjust(results, invocation.Method.ReturnType);
        }

        public static object Adjust(IEnumerable results, Type returnType)
        {
            if (returnType == typeof(void) ||
                results == null ||
                results.GetType() == returnType)
            {
                return results;
            }

            // 获取方法：
            // static IEnumerable<T> IEnumerable.OfType<T>(this IEnumerable source)
            // 其中T是来自returnType的IEnumerable <T>
            var enumerableOfTypeT = _enumerableOfTypeTDictionary.GetOrAdd(returnType, type => typeof(Enumerable).GetGenericMethod("OfType", type.GetGenericArguments(), new[] { typeof(IEnumerable) }, typeof(IEnumerable<>)));
            return enumerableOfTypeT.Invoke(null, new[] { results });
        }
    }

    public static class Extensions
    {
        public static MethodInfo GetGenericMethod(this Type t, string name, Type[] genericArgTypes, Type[] argTypes, Type returnType)
        {
            return (from m in t.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    where m.Name == name &&
                    m.GetGenericArguments().Length == genericArgTypes.Length &&
                    m.GetParameters().Select(pi => pi.ParameterType).SequenceEqual(argTypes) &&
                    (m.ReturnType.GetTypeInfo().IsGenericType && !m.ReturnType.GetTypeInfo().IsGenericTypeDefinition ? returnType.GetGenericTypeDefinition() : m.ReturnType) == returnType
                    select m).Single().MakeGenericMethod(genericArgTypes);

        }
    }
}
