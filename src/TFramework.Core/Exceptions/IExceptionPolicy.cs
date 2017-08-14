using System;

namespace TFramework.Core.Exceptions
{
    public interface IExceptionPolicy : ISingletonDependency
    {
        /// <summary>
        /// 如果异常应该被调用者重新抛出，返回false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        bool HandleException(object sender, Exception exception);
    }
}
