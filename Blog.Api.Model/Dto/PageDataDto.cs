using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Model.Dto
{
    /// <summary>
    /// 分页数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageDataDto<T>
    {
        public List<T> PageData { get; set; }=new List<T>();

        /// <summary>
        /// 数据总条数
        /// </summary>
        public long TotalCount { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        /// <summary>
        /// 总页面数
        /// </summary>
        public long TotalPages { get; set; }
    }
}
