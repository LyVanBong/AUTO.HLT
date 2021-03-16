using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AUTOHLT.WEB.API.Database;
using AUTOHLT.WEB.API.Models;
using AUTOHLT.WEB.API.Models.Version2.VersionApplication;

namespace AUTOHLT.WEB.API.Controllers.Version2.VersionApp
{
    [RoutePrefix("api/version2/VersionApplication")]
    public class VersionApplicationController : BaseController
    {
        /// <summary>
        /// kiểm tra version
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckVersion")]
        public async Task<IHttpActionResult> UpdateVersionApplication(string id)
        {
            var veri =await Verifying(Request);
            if (veri != null && veri.UserName != null && id != null)
            {
                var update = DatabaseAutohlt.sp_GetVersionApp(int.Parse(id))?.FirstOrDefault();
                if (update != null)
                {
                    return Ok(new ResponseModel<sp_GetVersionApp_Result>()
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
        /// <summary>
        /// yêu cầu user cập nhật phiên bản mới nhất
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<IHttpActionResult> UpdateVersionApplication(VersionModel input)
        {
            var veri = await Verifying(Request);
            if (veri != null && veri.UserName != null && input != null && input.VersionApp != null)
            {
                var update = DatabaseAutohlt.sp_UpdateVersionApplication(input.IdApp, input.VersionApp, input.NoteApp);
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
