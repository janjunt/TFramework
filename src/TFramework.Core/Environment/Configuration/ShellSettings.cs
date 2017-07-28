using System;
using System.Collections.Generic;
using System.Linq;

namespace TFramework.Core.Environment.Configuration
{
    /// <summary>
    /// 表示用于每个租户字段存储的简单集合。
    /// 该模型从IShellSettingsManager获取，该文件默认从App_Data目录下的settings.txt文件中读取。
    /// </summary>
    public class ShellSettings
    {
        public const string DefaultName = "Default";
        private TenantState _tenantState;
        private string[] _themes;
        private string[] _modules;
        private readonly IDictionary<string, string> _values;

        public ShellSettings()
        {
            _values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            State = TenantState.Invalid;
            Themes = new string[0];
            Modules = new string[0];
        }

        public ShellSettings(ShellSettings settings)
        {
            _values = new Dictionary<string, string>(settings._values, StringComparer.OrdinalIgnoreCase);

            Name = settings.Name;
            DataProvider = settings.DataProvider;
            DataConnectionString = settings.DataConnectionString;
            DataTablePrefix = settings.DataTablePrefix;
            RequestUrlHost = settings.RequestUrlHost;
            RequestUrlPrefix = settings.RequestUrlPrefix;
            EncryptionAlgorithm = settings.EncryptionAlgorithm;
            EncryptionKey = settings.EncryptionKey;
            HashAlgorithm = settings.HashAlgorithm;
            HashKey = settings.HashKey;
            State = settings.State;
            Themes = settings.Themes;
            Modules = settings.Modules;
        }

        public string this[string key]
        {
            get
            {
                string retVal;
                return _values.TryGetValue(key, out retVal) ? retVal : null;
            }
            set { _values[key] = value; }
        }

        /// <summary>
        /// 获取此shell设置的所有键。
        /// </summary>
        public IEnumerable<string> Keys { get { return _values.Keys; } }

        /// <summary>
        /// 租房名称
        /// </summary>
        public string Name
        {
            get { return this["Name"] ?? ""; }
            set { this["Name"] = value; }
        }

        /// <summary>
        /// 租户的数据提供者
        /// </summary>
        public string DataProvider
        {
            get { return this["DataProvider"] ?? ""; }
            set { this["DataProvider"] = value; }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string DataConnectionString
        {
            get { return this["DataConnectionString"]; }
            set { this["DataConnectionString"] = value; }
        }

        /// <summary>
        /// 租户的数据表名称前缀
        /// </summary>
        public string DataTablePrefix
        {
            get { return this["DataTablePrefix"]; }
            set { this["DataTablePrefix"] = value; }
        }

        /// <summary>
        /// 租户的主机名
        /// </summary>
        public string RequestUrlHost
        {
            get { return this["RequestUrlHost"]; }
            set { this["RequestUrlHost"] = value; }
        }

        /// <summary>
        /// 租户请求Url前缀
        /// </summary>
        public string RequestUrlPrefix
        {
            get { return this["RequestUrlPrefix"]; }
            set { _values["RequestUrlPrefix"] = value; }
        }

        /// <summary>
        /// 用于加密服务的加密算法
        /// </summary>
        public string EncryptionAlgorithm
        {
            get { return this["EncryptionAlgorithm"]; }
            set { this["EncryptionAlgorithm"] = value; }
        }

        /// <summary>
        /// 用于加密服务的加密密钥
        /// </summary>
        public string EncryptionKey
        {
            get { return this["EncryptionKey"]; }
            set { _values["EncryptionKey"] = value; }
        }

        /// <summary>
        /// 用于加密服务的哈希算法
        /// </summary>
        public string HashAlgorithm
        {
            get { return this["HashAlgorithm"]; }
            set { this["HashAlgorithm"] = value; }
        }

        /// <summary>
        /// 用于加密服务的哈希密钥
        /// </summary>
        public string HashKey
        {
            get { return this["HashKey"]; }
            set { this["HashKey"] = value; }
        }

        /// <summary>
        /// 租房可用主题列表
        /// </summary>
        public string[] Themes
        {
            get
            {
                return _themes ?? (Themes = (_values["Themes"] ?? "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                                                                     .Select(t => t.Trim())
                                                                     .ToArray();
            }
            set
            {
                _themes = value;
                this["Themes"] = string.Join(";", value);
            }
        }

        /// <summary>
        /// 租户可用模块列表
        /// </summary>
        public string[] Modules
        {
            get
            {
                return _modules ?? (Modules = (_values["Modules"] ?? "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                                                                     .Select(t => t.Trim())
                                                                     .ToArray();
            }
            set
            {
                _modules = value;
                this["Modules"] = string.Join(";", value);
            }
        }

        /// <summary>
        /// 租户状态
        /// </summary>
        public TenantState State
        {
            get { return _tenantState; }
            set
            {
                _tenantState = value;
                this["State"] = value.ToString();
            }
        }
    }
}
