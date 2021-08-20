using AUTOHLT.MOBILE.Models.Guide;
using System.Threading.Tasks;

namespace AUTOHLT.MOBILE.Services.Guide
{
    public interface IGuideService
    {
        /// <summary>
        /// lấy hdsd
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GuideModel> GetGuide(int id);
    }
}