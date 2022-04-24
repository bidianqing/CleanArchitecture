using Newtonsoft.Json;

namespace Domain.SeedWork
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public ResultModel(bool success, string message, T data)
        {
            this.Success = success;
            this.Message = message;
            this.Data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ResultModel
    {
        /// <summary>
        /// 成功返回
        /// </summary>
        /// <typeparam name="T">具体类型</typeparam>
        /// <param name="data">有效负载</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public static ResultModel<T> Success<T>(T data = default, string message = "操作成功")
        {
            return new ResultModel<T>(true, message, data);
        }

        /// <summary>
        /// 失败返回
        /// </summary>
        /// <typeparam name="T">具体类型</typeparam>
        /// <param name="data">有效负载</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public static ResultModel<T> Fail<T>(T data = default, string message = "操作失败")
        {
            return new ResultModel<T>(false, message, data);
        }
    }

}
