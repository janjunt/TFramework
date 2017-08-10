namespace TFramework.Core.Security
{
    /// <summary>
    /// 由“用户”模型实现的接口。
    /// </summary>
    public interface IUser //: IContent
    {
        string UserName { get; }
        string Email { get; }
    }
}
