using System.Collections.Generic;
using System.Threading.Tasks;
using AUTO.DLL.MOBILE.Models.RequestProviderModel;

namespace AUTO.DLL.MOBILE.Services.RequestProvider
{
    public interface IRequestProvider
    {
        Task<ResponseModel<T>> GetAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null);

        Task<ResponseModel<T>> PostAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null);

        Task<ResponseModel<T>> PutAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null);

        Task<ResponseModel<T>> DeleteAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null);
    }
}