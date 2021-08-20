using AUTOHLT.MOBILE.Models.RequestProviderModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOHLT.MOBILE.Services.RestSharp
{
    public interface IRestSharpService
    {
        Task<string> GetAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, string cookie = null);

        Task<string> PostAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, string cookie = null);

        Task<string> PutAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, string cookie = null);

        Task<string> DeleteAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, string cookie = null);
    }
}