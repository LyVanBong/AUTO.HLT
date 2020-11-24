namespace AUTOHLT.WEB.API.Models
{
    public class ResponseModel<T>
    {
        /// <summary>
        /// Mã thông báo
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// tin nhăn thông bao
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// dữ liệu
        /// </summary>
        public T Data { get; set; }
    }
}