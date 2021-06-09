using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTO.HLT.MOBILE.VIP.Models.RequestProviderModel;
using AUTO.HLT.MOBILE.VIP.Services.RestSharp;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;

namespace AUTO.HLT.MOBILE.VIP.Services.Guide
{
    public class GuideService : IGuideService
    {
        private IRestSharpService _restSharpService;
        public GuideService(IRestSharpService restSharpService)
        {
            _restSharpService = restSharpService;
        }
        public async Task<string> GetGuide(int id = 17)
        {
            string url = null;
            try
            {
                var json = await _restSharpService.GetAsync("https://api.autohlt.vn/api/v1/HDSD/Get");
                var data = JsonConvert.DeserializeObject<ResponseModel<List<GuideModel>>>(json);

                url = data?.Data?.FirstOrDefault(x => x.ID == id)?.Url;

            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            return url;
        }
    }

    public class GuideModel
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string Note { get; set; }
    }

}