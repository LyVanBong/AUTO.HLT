using AUTOHLT.WEB.API.Models;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using AUTOHLT.WEB.API.Models.Version1;

namespace AUTOHLT.WEB.API.Controllers.Version2.User
{
    [RoutePrefix("api/version2/user")]
    public class SigUpController : BaseController
    {
        /// <summary>
        /// api dung de dang ky tai khoan user
        /// </summary>
        /// <param name="accountModel"></param>
        /// <returns></returns>
        [Route("Sigup")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] RegistrationAccountModel accountModel)
        {
            if (accountModel != null)
            {
                var userName = Regex.Match(accountModel?.UserName, @"^[a-zA-Z0-9]+(?:[_.]?[a-zA-Z0-9])*$")?.Value;
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    var data = DatabaseAutohlt.RegistrationAccount(accountModel.UserName, accountModel.Password,
                        accountModel.Name, accountModel.NumberPhone, accountModel.Email, accountModel.Sex,
                        accountModel.Age, accountModel.DateCreate);
                    if (data > 0)
                        return Ok(new ResponseModel<int>
                        {
                            Code = 22,
                            Data = data,
                            Message = "Đăng ký tài khoản thành công"
                        });
                }
            }
            return Ok(new ResponseModel<string>
            {
                Code = -22,
                Data = null,
                Message = "Lỗi phát sinh trong quá trình đăng ký tài khoản"
            });
        }
    }
}