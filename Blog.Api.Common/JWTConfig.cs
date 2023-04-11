using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Common
{
    public class JWTConfig
    {
        /// <summary>
        /// 秘钥
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuser { get; set; }

        /// <summary>
        /// 过期时间，单位s
        /// </summary>
        public int Expires { get; set; }

        /// <summary>
        /// 订阅人
        /// </summary>
        public string Audience { get; set; }
    }
}
