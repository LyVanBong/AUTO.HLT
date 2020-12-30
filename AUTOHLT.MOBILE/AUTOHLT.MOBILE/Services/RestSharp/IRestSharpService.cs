using System.Collections.Generic;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.RequestProviderModel;

namespace AUTOHLT.MOBILE.Services.RestSharp
{
    public interface IRestSharpService
    {
        Task<string> GetAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null);

        Task<string> PostAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null);

        Task<string> PutAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null);

        Task<string> DeleteAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null);
    }
}