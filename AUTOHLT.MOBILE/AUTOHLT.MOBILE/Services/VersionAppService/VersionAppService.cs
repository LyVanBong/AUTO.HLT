using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Models.VersionApp;
using AUTOHLT.MOBILE.Services.RequestProvider;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace AUTOHLT.MOBILE.Services.VersionAppService
{
    public class VersionAppService : IVersionAppService
    {
        private IRequestProvider _requestProvider;

        public VersionAppService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<VersionModel>> CheckVersionApp()
        {
            try
            {
                var client = new RestClient("https://api.autohlt.vn/api/version2/VersionApplication/CheckVersion?id=1");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                var response = await client.ExecuteAsync(request);
                var data = JsonConvert.DeserializeObject<ResponseModel<VersionModel>>(response?.Content);
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }
    }
}