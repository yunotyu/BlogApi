namespace Blog.Api.Common
{
    /// <summary>
    /// 返回给前端的消息
    /// </summary>
    public class ResultMsg<T>
    {
        /// <summary>
        /// 消息代码，0代表失败，1代表成功
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 具体的内容，失败返回报错信息，成功返回对应的json
        /// </summary>
        public T Data { get; set; }

        public string Msg { get; set; } = "";
    }
}
