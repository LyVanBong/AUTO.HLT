using AUTOHLT.WEB.API.Database;
using AUTOHLT.WEB.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AUTOHLT.WEB.API.Controllers
{
    [RoutePrefix("api/v1/notification")]
    public class NotificationController : ApiController
    {
        private bsoft_autohltEntities _entities;

        public NotificationController()
        {
            _entities = new bsoft_autohltEntities();
        }

        /// <summary>
        /// lay chi tiet mot thong bao
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("detail")]
        [HttpGet]
        public IHttpActionResult GetDetailNotification(Guid id)
        {
            var data = _entities.NotificationDetail(id);
            if (data != null)
            {
                var transfer = data.FirstOrDefault();
                if (transfer != null)
                {
                    return Ok(new ResponseModel<NotificationDetail_Result>
                    {
                        Code = 14,
                        Data = transfer,
                        Message = "Thanh cong"
                    });
                }
            }
            return Ok(new ResponseModel<string>
            {
                Code = -14,
                Data = null,
                Message = "Lỗi phát sinh trong quá trình xử lý"
            });
        }

        /// <summary>
        /// lấy danh sách thông báo cho từng tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("getnotificationforuser")]
        [HttpGet]
        public IHttpActionResult GetNotificationForUser(Guid id)
        {
            var data = _entities.GetNotificationUser(id);
            if (data != null)
            {
                var lsNoti = data.ToList();
                if (lsNoti.Any())
                {
                    return Ok(new ResponseModel<List<GetNotificationUser_Result>>
                    {
                        Code = 11,
                        Data = lsNoti,
                        Message = "Lay danh sach thong bao thanh cong"
                    });
                }
            }
            return Ok(new ResponseModel<string>
            {
                Code = -11,
                Data = null,
                Message = "Phát sinh lỗi trong qua trinh lấy dữ liệu"
            });
        }

        /// <summary>
        /// them thong bao
        /// </summary>
        /// <returns></returns>
        [Route("addnotification")]
        [HttpPost]
        public IHttpActionResult AddNotificaion([FromBody] AddNotificationModel addNotificationModel)
        {
            if (addNotificationModel != null)
            {
                var data = _entities.AddNotification(addNotificationModel.ContentNotification,
                    addNotificationModel.IdSend, addNotificationModel.IdReceive, addNotificationModel.NotificationType);
                if (data > 0)
                    return Ok(new ResponseModel<int>
                    {
                        Code = 10,
                        Data = data,
                        Message = "Thêm thông báo thành công"
                    });
            }
            return Ok(new ResponseModel<string>
            {
                Code = -10,
                Data = null,
                Message = "Thêm thông báo thành công"
            });
        }

        /// <summary>
        /// Cap lai danh thong bao thanh da doc
        /// </summary>
        /// <param name="iduser"></param>
        /// <returns></returns>
        [Route("updatenotification")]
        [HttpPut]
        public IHttpActionResult UpdateReadNotification(Guid iduser)
        {
            var data = _entities.UpdateReadNotification(iduser);
            if (data > 0)
                return Ok(new ResponseModel<int>
                {
                    Code = 9,
                    Data = data,
                    Message = $"Đã cập nhật thông báo của ID {iduser} thành công"
                });
            return Ok(new ResponseModel<int>
            {
                Code = -9,
                Data = data,
                Message = $"Đã cập nhật thông báo của ID {iduser} chưa thành công"
            });
        }
    }
}