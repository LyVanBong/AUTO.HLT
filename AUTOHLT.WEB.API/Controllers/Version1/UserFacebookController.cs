using AUTOHLT.WEB.API.Database;
using AUTOHLT.WEB.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AUTOHLT.WEB.API.Models.Version1;

namespace AUTOHLT.WEB.API.Controllers.Version1
{
    [RoutePrefix("api/v1/Facebook")]
    public class UserFacebookController : ApiController
    {
        private bsoft_autohltEntities _entities;

        public UserFacebookController()
        {
            _entities = new bsoft_autohltEntities();
        }

        [Route("AllUidTypeFacebook")]
        [HttpGet]
        public IHttpActionResult GetAllUidTypeFacebook()
        {
            var data = _entities.GetAllIDTypeFacebook();
            if (data != null)
            {
                return Ok(new ResponseModel<List<GetAllIDTypeFacebook_Result>>()
                {
                    Code = 12349,
                    Message = "thanh cong",
                    Data = data.ToList(),
                });
            }

            return Ok(new ResponseModel<string>()
            {
                Code = -12349,
                Message = "Loi phat sinh",
                Data = null,
            });
        }

        [Route("DeleteTypeUidFacebook")]
        [HttpDelete]
        public IHttpActionResult DeleteUidFacebook(string uid)
        {
            var id = uid.Split(',');
            var total = 0;
            foreach (var item in id)
            {
                var delete = _entities.DeleteIDTypeFacebook(item);
                if (delete > 0)
                    total++;
            }
            if (total > 0)
            {
                return Ok(new ResponseModel<int>()
                {
                    Code = 12347,
                    Message = $"Đã xóa {total} user facebook khong con hoat dong",
                    Data = total
                });
            }
            return Ok(new ResponseModel<string>()
            {
                Code = -12347,
                Message = "Lỗi phát sinh",
                Data = null,
            });
        }

        [Route("AddTypeUidFacebook")]
        [HttpPost]
        public IHttpActionResult UpdateUidTypeFacebook(GetAllIDTypeFacebook_Result facebook)
        {
            var data = _entities.AddUIDTypeFacebook(facebook.UID, facebook.UIDType);
            return Ok(new ResponseModel<string>()
            {
                Code = 12348,
                Message = "Thanh cong",
                Data = data + ""
            });
        }

        /// <summary>
        /// xoa danh sach user facebook khong con hoat dong nua
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Route("DeleteFacebook")]
        [HttpDelete]
        public IHttpActionResult DeleteUserFacebook(string uid)
        {
            if (uid != null)
            {
                var id = uid.Split(',');
                var total = 0;
                foreach (var item in id)
                {
                    var delete = _entities.DeleteUserFacebook(item);
                    if (delete > 0)
                        total++;
                }

                if (total > 0)
                {
                    return Ok(new ResponseModel<int>()
                    {
                        Code = 12347,
                        Message = $"Đã xóa {total} user facebook khong con hoat dong",
                        Data = total
                    });
                }
            }
            return Ok(new ResponseModel<string>()
            {
                Code = -12347,
                Message = "Lỗi phát sinh",
                Data = null,
            });
        }

        /// <summary>
        /// cập bảng user facebook
        /// </summary>
        /// <param name="facebook"></param>
        /// <returns></returns>
        [Route("Update")]
        [HttpPost]
        public IHttpActionResult UpdateUserFacebook(UserFacebookModel facebook)
        {
            if (facebook != null && facebook.UID != null)
            {
                var data = _entities.UpdateUserFacebook(facebook.Id_User, facebook.UID, facebook.F_Cookie,
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

        /// <summary>
        /// Lấy toàn bộ user facebook sử dụng dịch free
        /// </summary>
        /// <returns></returns>
        [Route("AllUserFacebook")]
        [HttpGet]
        public IHttpActionResult GetAllUserFacebook()
        {
            var data = _entities.GetAllUserFacebook();
            if (data != null)
            {
                return Ok(new ResponseModel<List<GetAllUserFacebook_Result>>
                {
                    Code = 12345,
                    Message = "Lấy toàn bộ User facebook",
                    Data = data.ToList(),
                });
            }
            return Ok(new ResponseModel<string>
            {
                Code = -12345,
                Message = "không có dữ liệu nào",
                Data = null,
            });
        }
    }
}