using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.Guide;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Services.RequestProvider;

namespace AUTOHLT.MOBILE.Services.Guide
{
    public class GuideService : IGuideService
    {
        private IRequestProvider _requestProvider;
        private static List<GuideModel> _lsGuideModels;
        public GuideService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<GuideModel> GetGuide(int id)
        {
            try
            {
                if (_lsGuideModels==null)
                {
                    var data =await _requestProvider.GetAsync<List<GuideModel>>("HDSD/Get");
                    if (data?.Code>0&&data?.Data!=null && data.Data.Any())
                    {
                        _lsGuideModels = new List<GuideModel>(data.Data);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return _lsGuideModels.FirstOrDefault(x => x.ID == id);
                }
            }
            catch (Exception )
            {
                return null;
            }

            return null;
        }
    }
}