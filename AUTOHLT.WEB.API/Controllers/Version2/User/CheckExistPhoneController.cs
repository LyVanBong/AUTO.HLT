using AUTOHLT.WEB.API.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace AUTOHLT.WEB.API.Controllers.Version2.User
{
    [RoutePrefix("api/version2/user")]
    public class CheckExistPhoneController : BaseController
    {
        /// <summary>
        /// kiem tra su ton tai cua so dien thoai
        /// </summary>
        /// <param name="numberPhone"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckExistPhone")]
        public async Task<IHttpActionResult> CheckExistPhoneNumber(string numberPhone)
        {
            if (numberPhone != null)
            {
                var data = DatabaseAutohlt.CheckNumberPhone(numberPhone)?.FirstOrDefault();
                if (data != null && data.Any())
                {
                    return Ok(new ResponseModel<string>()
                    {
                        Code = 8877,
                        Message = "thanh cong",
                        Data = data
                    });
                }

                return Ok(new ResponseModel<string>()
                {
                    Code = -8877,
                    Message = "loi phat sinh",
                    Data = "",
                });
            }
            else
                return Ok(new ResponseModel<string>()
                {
                    Code = -9871,
                    Message = "Khong co quyen truy cap tai nguyen",
                    Data = null
                });
        }
    }
}