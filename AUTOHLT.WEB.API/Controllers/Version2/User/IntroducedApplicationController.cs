using AUTOHLT.WEB.API.Database;
using AUTOHLT.WEB.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AUTOHLT.WEB.API.Controllers.Version2.User
{
    [RoutePrefix("api/version2/user")]
    public class IntroducedApplicationController : BaseController
    {
        [Route("LayNguoiGioiThieu")]
        [HttpGet]
        public IHttpActionResult LayNguoiGioiThieu(string user)
        {
            var veri = Verifying(Request);
            if (veri != null && veri.UserName != null && user != null)
            {
                var data = DatabaseAutohlt.GetAllNguoiGioiThieu(user)?.ToList();
                if (data != null && data.Any())
                {
                    return Ok(new ResponseModel<List<GetAllNguoiGioiThieu_Result>>()
                    {
                        Code = 8876,
                        Message = "thanh cong",
                        Data = data
                    });
                }

                return Ok(new ResponseModel<string>()
                {
                    Code = -8876,
                    Message = "loi phat sinh",
                    Data = "",
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
        /// <summary>
        /// Thêm người giới thiệu cho đại lý
        /// </summary>
        /// <param name="gioiThieu"></param>
        /// <returns></returns>
        [Route("ThemNguoiGioiThieu")]
        [HttpPost]
        public IHttpActionResult ThemNguoiGioiThieu(NguoiGioiThieuModel input)
        {
            var veri = Verifying(Request);
            if (veri != null && veri.UserName != null && input != null && input != null && input.UserGioiThieu != null)
            {
                var update = DatabaseAutohlt.ThemNguoiGioiThieu(input.UserGioiThieu, input.UserDuocGioiThieu,
                    input.Discount, input.Note);
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
