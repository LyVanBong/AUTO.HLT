﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AUTO.MOBILE.DLL.Models.RequestProviderModel;

namespace AUTO.MOBILE.DLL.Services.RestSharp
{
    public interface IRestSharpService
    {
        Task<string> GetAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, string cookie = null);

        Task<string> PostAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, string cookie = null);

        Task<string> PutAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, string cookie = null);

        Task<string> DeleteAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, string cookie = null);
    }
}