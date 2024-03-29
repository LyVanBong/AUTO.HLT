﻿using Newtonsoft.Json;

namespace AUTO.TOOL.CLIENT.Models.RequestProviderModel
{
    public class ResponseModel<T>
    {
        [JsonProperty("Code")]
        public long Code { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("Data")]
        public T Data { get; set; }
    }
}