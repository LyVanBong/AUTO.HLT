namespace AUTO.DLL.Models
{
    public class ResponseModel<T>
    {
        public long Code { get; set; }
        
        public string Message { get; set; }
        
        public T Data { get; set; }
    }
}