using System.Threading.Tasks;

namespace AUTO.HLT.MOBILE.VIP.Services.Guide
{
    public interface IGuideService
    {
        /// <summary>
        /// lấy url hương dẫn sử dụng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> GetGuide(int id = 17);
    }
}