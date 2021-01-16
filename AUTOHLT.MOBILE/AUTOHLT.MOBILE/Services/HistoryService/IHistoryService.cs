using AUTOHLT.MOBILE.Models.RequestProviderModel;
using System.Threading.Tasks;

namespace AUTOHLT.MOBILE.Services.HistoryService
{
    public interface IHistoryService
    {
        /// <summary>
        /// ghi lai lich su dung dich cua khach hang
        /// </summary>
        /// <param name="idProduct"></param>
        /// <param name="content"></param>
        /// <param name="idUser"></param>
        /// <param name="number"></param>
        /// <param name="dateCreate"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> AddLog(string idProduct, string content, string idUser, string number,
            string dateCreate);
    }
}