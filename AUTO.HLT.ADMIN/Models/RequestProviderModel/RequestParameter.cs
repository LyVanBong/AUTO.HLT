namespace AUTO.HLT.ADMIN.Models.RequestProviderModel
{
    public class RequestParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public RequestParameter(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}