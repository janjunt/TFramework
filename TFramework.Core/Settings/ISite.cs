namespace TFramework.Core.Settings
{
    /// <summary>
    /// 由“settings”模型实现的接口。
    /// </summary>
    public interface ISite //: IContent
    {
        string PageTitleSeparator { get; }
        string SiteName { get; }
        string SiteSalt { get; }
        string SuperUser { get; }
        string HomePage { get; set; }
        string SiteCulture { get; set; }
        string SiteCalendar { get; set; }
        ResourceDebugMode ResourceDebugMode { get; set; }
        int PageSize { get; set; }
        int MaxPageSize { get; set; }
        string BaseUrl { get; }
        string SiteTimeZone { get; }
    }
}
