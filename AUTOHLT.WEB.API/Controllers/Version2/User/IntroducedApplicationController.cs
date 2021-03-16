using AUTOHLT.WEB.API.Database;
using AUTOHLT.WEB.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AUTOHLT.WEB.API.Models.Version1;

namespace AUTOHLT.WEB.API.Controllers.Version2.User
{
    [RoutePrefix("api/version2/user")]
    public class IntroducedApplicationController : BaseController
    {
        /// <summary>
        /// lay thong tin nguoi gioi thieu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ThongTinNguoiGioiThieu")]
        public async Task<IHttpActionResult> ThongTinNguoiGioiThieu()
        {
            var veri = await Verifying(Request);
            if (veri != null && veri.UserName != null)
            {
                var data = DatabaseAutohlt.sp_LayNguoiGioiThieuChoKhach(veri.UserName)?.FirstOrDefault();
                if (data != null)
                {
                    return Ok(new ResponseModel<sp_LayNguoiGioiThieuChoKhach_Result>()
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
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("LayNguoiGioiThieuAll")]
        [HttpGet]
        public async Task<IHttpActionResult> LayNguoiGioiThieu(string user, int admin)
        {
            var veri = await Verifying(Request);
            if (veri != null && veri.UserName != null && user != null)
            {
                var data = DatabaseAutohlt.sp_LayTatCaGioiThieu(admin, user)?.ToList();
                if (data != null && data.Any())
                {
                    return Ok(new ResponseModel<List<sp_LayTatCaGioiThieu_Result>>()
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
        public async Task<IHttpActionResult> ThemNguoiGioiThieu(NguoiGioiThieuModel input)
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
    }
}