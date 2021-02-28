using AUTOHLT.WEB.API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace AUTOHLT.WEB.API.Controllers.Version2.User
{
    [RoutePrefix("api/version2/user")]
    public class CheckExistUserController : BaseController
    {
        /// <summary>
        /// kiem tra su ton tai cua tai khoan
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckExistUserName")]
        public async Task<IHttpActionResult> CheckExistUser(string userName)
        {
            var veri = Verifying(Request);
            if (veri != null && veri.UserName != null && userName != null)
            {
                Guid? guid = new Guid();
                if (userName != null)
                {
                    var data = DatabaseAutohlt.CheckUserName(userName);
                    if (data != null)
                    {
                        guid = data.SingleOrDefault();
                        if (guid == null)
                        {
                            return Ok(new ResponseModel<string>
                            {
                                Code = 8,
                                Data = null,
                                Message = $"User {userName} chua ton tai"
                            });
                        }
                    }
                }

                return Ok(new ResponseModel<string>
                {
                    Code = -8,
                    Data = guid.ToString(),
                    Message = $"User {userName} da ton tai"
                });
            }
            else
                return Ok(new ResponseModel<string>()
                {
                    Code = -241,
                    Message = "Khong co quyen truy cap tai nguyen",
                    Data = null
                });
        }
    }
}
