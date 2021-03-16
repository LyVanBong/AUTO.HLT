using AUTOHLT.WEB.API.Models;
using AUTOHLT.WEB.API.Models.Version1;
using System.Threading.Tasks;
using System.Web.Http;

namespace AUTOHLT.WEB.API.Controllers.Version2.Facebook
{
    [RoutePrefix("api/version2/Facebook")]
    public class FacebookController : BaseController
    {
        /// <summary>
        /// lưu thông tin facebook
        /// </summary>
        /// <returns></returns>
        [Route("Update")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveFacebook(UserFacebookModel facebook)
        {
            var veri =await Verifying(Request);
            if (veri != null && veri.UserName != null && veri.Role == 0)
            {
                if (facebook != null && facebook.UID != null)
                {
                    var data = DatabaseAutohlt.UpdateUserFacebook(facebook.Id_User, facebook.UID, facebook.F_Cookie,
                        facebook.F_Token, facebook.Note);
                    if (data > 0)
                    {
                        return Ok(new ResponseModel<int>
                        {
                            Code = 12346,
                            Message = "Cập nhật thành công",
                            Data = data,
                        });
                    }
                }
                return Ok(new ResponseModel<string>
                {
                    Code = -12346,
                    Message = "Cập nhật lỗi",
                    Data = null,
                });
            }
            else
                return Ok(new ResponseModel<string>()
                {
                    Code = -12346,
                    Message = "Khong co quyen truy cap tai nguyen",
                    Data = null
                });
        }
    }
}
