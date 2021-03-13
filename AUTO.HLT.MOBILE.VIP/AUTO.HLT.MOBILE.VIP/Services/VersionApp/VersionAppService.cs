using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AUTO.HLT.MOBILE.VIP.Models.RequestProviderModel;
using AUTO.HLT.MOBILE.VIP.Models.VersionApp;
using AUTO.HLT.MOBILE.VIP.Services.RequestProvider;
using Microsoft.AppCenter.Crashes;

namespace AUTO.HLT.MOBILE.VIP.Services.VersionApp
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
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("id","2"),
                };
                var data = await _requestProvider.GetAsync<VersionModel>("VersionApplication/CheckVersion", parameters);
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