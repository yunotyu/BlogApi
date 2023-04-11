using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Common
{
    /// <summary>
    /// 获取appsettings.json
    /// </summary>
    public class AppConfigs
    {
        private static  IConfiguration _configuration;

        /// <summary>
        /// base64加密key
        /// </summary>
        public static string Base64Key = "";

        public AppConfigs(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 可以读取多层节点，节点中使用:分割
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string ReadAppConfig(params string[] keys)
        {
            if (keys.Any())
            {
                return _configuration[string.Join(":", keys)];
            }
            else
            {
                return "";
            }
        }
    }
}
