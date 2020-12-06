using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Services.RequestProvider;

namespace AUTOHLT.MOBILE.Services.HistoryService
{
    public class HistoryService : IHistoryService
    {
        private IRequestProvider _requestProvider;

        public HistoryService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public Task<ResponseModel<string>> AddLog(string idProduct, string content, string idUser, string number, string dateCreate)
        {
            throw new System.NotImplementedException();
        }
    }
}