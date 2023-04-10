using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Common.Json
{
    public class JsonHelper
    {

        /// <summary>
        /// 序列化为一个json字符串
        /// </summary>
        /// <returns></returns>
        public static String Serialize(object obj)
        {
            if(obj!=null)
            {
                return JsonConvert.SerializeObject(obj);
            }
            return "";
        }

        /// <summary>
        /// 反序列化一个json字符串为一个T对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
