using System.Threading.Tasks;
using System.Web.Http;
using AUTOHLT.WEB.API.Models;
using AUTOHLT.WEB.API.Models.Version2.User;

namespace AUTOHLT.WEB.API.Controllers.Version2.User
{
    [RoutePrefix("api/version2/user")]
    public class SetRoleUserController : BaseController
    {
        /// <summary>
        /// thêm quyền cho user
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("AddRole")]
        public async Task<IHttpActionResult> SetRole(RoleModel input)
        {
            var veri = Verifying(Request);
            if (veri != null && veri.UserName != null && input != null && veri.Role == 0)
            {
                var update = DatabaseAutohlt.sp_SetRoleUser(input.IdUser, input.Role);
                if (update > 0)
                {
                    return Ok(new ResponseModel<int>()
                    {
                        Code = 241,
                        Message = "Thanh cong",
                        Data = update
                    });
                }
                else
                {
                    return Ok(new ResponseModel<string>()
                    {
                        Code = -241,
                        Message = "Loi phat sinh",
                        Data = null
                    });
                }
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
