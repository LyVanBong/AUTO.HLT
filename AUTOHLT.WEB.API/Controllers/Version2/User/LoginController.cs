using AUTOHLT.WEB.API.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AUTOHLT.WEB.API.Models.Version1;

namespace AUTOHLT.WEB.API.Controllers.Version2.User
{
    [RoutePrefix("api/version2/user")]
    public class LoginController : BaseController
    {
        [Route("Login")]
        [HttpPost]
        public async Task<IHttpActionResult> Login([FromBody] LoginModel login)
        {
            if (login != null && login.UserName != null && login.PassWord != null)
            {
                var data = DatabaseAutohlt.UserInformation(login.UserName, login.PassWord);
                if (data != null)
                {
                    var user = data?.FirstOrDefault();
                    if (user != null && user.UserName != null)
                    {
                        return Ok(new ResponseModel<object>
                        {
                            Code = 21,
                            Message = "Đăng nhập thành công",
                            Data = new
                            {
                                ID = user.ID,
                                UserName = user.UserName,
                                Password = user.Password,
                                Name = user.Name,
                                NumberPhone = user.NumberPhone,
                                Email = user.Email,
                                Sex = user.Sex,
                                Role = user.Role,
                                IsActive = user.IsActive,
                                Age = user.Age,
                                DateCreate = user.DateCreate,
                                Price = user.Price,
                                IdDevice = user.IdDevice,
                                Jwt =await Signing(user.UserName, user.ID, user.Role),
                            },
                        });
                    }
                }
            }
            return Ok(new ResponseModel<string>
            {
                Code = -21,
                Message = "Đăng nhập lỗi",
                Data = null,
            });
        }
    }
}