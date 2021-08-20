using AUTO.TOOL.CLIENT.Models.RequestProviderModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTO.TOOL.CLIENT.Services.RequestProvider
{
    public interface IRequestProvider
    {
        Task<ResponseModel<T>> GetAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null);

        Task<ResponseModel<T>> PostAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null);

        Task<ResponseModel<T>> PutAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null);

        Task<ResponseModel<T>> DeleteAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null);
    }
}