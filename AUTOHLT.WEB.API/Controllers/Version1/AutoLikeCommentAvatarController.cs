using AUTOHLT.WEB.API.Database;
using AUTOHLT.WEB.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AUTOHLT.WEB.API.Controllers.Version1
{
    [RoutePrefix("api/v1/AutoLikeCommentAvatar")]
    public class AutoLikeCommentAvatarController : ApiController
    {
        private bsoft_autohltEntities _dbAutohltEntities;

        public AutoLikeCommentAvatarController()
        {
            _dbAutohltEntities = new bsoft_autohltEntities();
        }

        [Route("GetAllHistoryAutoLikeCommentAvatar")]
        [HttpGet]
        public IHttpActionResult GetAllHistoryAutoLikeComment()
        {
            var data = _dbAutohltEntities.GetAllHistoryAutoLikeCommentAvatar()?.ToList();
            if (data != null && data.Any())
            {
                return Ok(new ResponseModel<List<GetAllHistoryAutoLikeCommentAvatar_Result>>()
                {
                    Code = 34234,
                    Message = "Thanh cong",
                    Data = data,
                });
            }

            return Ok(new ResponseModel<string>()
            {
                Code = 34234,
                Message = "Loi",
                Data = null
            });
        }

        [Route("AddHistory")]
        [HttpPost]
        public IHttpActionResult AddHistoryAutoLikeComment(GetAllHistoryAutoLikeCommentAvatar_Result model)
        {
            var data = _dbAutohltEntities.AddHistoryAutoLikeComment(model.ID, model.UId, model.Name, model.Avatar, model.TypeAuto, model.UId_Friend, model.Name_Friend, model.Avatar_Friend);
            return Ok(new ResponseModel<int>()
            {
                Code = 8876,
                Message = "thanh cong",
                Data = data
            });
        }

        [Route("GetAllFUId")]
        [HttpGet]
        public IHttpActionResult GetAllFUidAuto(string id)
        {
            var data = _dbAutohltEntities.GetAllFUIdFriendAutoLikeComment(id)?.ToList();
            if (data != null && data.Any())
            {
                return Ok(new ResponseModel<List<GetAllFUIdFriendAutoLikeComment_Result>>()
                {
                    Code = 37463,
                    Message = "thanh cong",
                    Data = data,
                });
            }
            return Ok(new ResponseModel<string>()
            {
                Code = -37463,
                Message = "Loi",
                Data = null
            });
        }

        [Route("DeleteFUIdAuto")]
        [HttpDelete]
        public IHttpActionResult DeleteFUidAutoLikeCommentAvatar(string id)
        {
            var data = _dbAutohltEntities.DeleteFUIdFriendAutoLikeComment(id);
            return Ok(new ResponseModel<int>()
            {
                Code = 8876,
                Message = "thanh cong",
                Data = data
            });
        }

        [Route("AddFUidAutoLikeComment")]
        [HttpPost]
        public IHttpActionResult AddFUIdAutoLikeCommentAvatar(AddFUIdAutoLikeCommentModel model)
        {
            var data = _dbAutohltEntities.AddFUIdFriendAutoLikeComment(model.Id, model.UId);
            return Ok(new ResponseModel<int>()
            {
                Code = 8876,
                Message = "thanh cong",
                Data = data
            });
        }

        [Route("GetAll")]
        [HttpGet]
        public IHttpActionResult GetAllAutoLikeCommentAvataResult()
        {
            var data = _dbAutohltEntities.GetAllUserAutoLikeComment()?.ToList();
            if (data != null && data.Any())
            {
                return Ok(new ResponseModel<List<GetAllUserAutoLikeComment_Result>>()
                {
                    Code = 9877,
                    Message = "thanh cong",
                    Data = data,
                });
            }
            return Ok(new ResponseModel<string>()
            {
                Code = -9877,
                Message = "Loi",
                Data = null,
            });
        }

        /// <summary>
        /// xoa thong tin user auto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("DeleteUserAutoLikeCommetAvatar")]
        [HttpDelete]
        public IHttpActionResult DeleteUserAutoLikeCommentAvatar(string id)
        {
            var data = _dbAutohltEntities.DeleteAutoLikeCmtAvatar(id);
            return Ok(new ResponseModel<int>()
            {
                Code = 8876,
                Message = "thanh cong",
                Data = data
            });
        }

        /// <summary>
        /// Cập lại Thông tin user auto like comment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateUserAuto")]
        [HttpPut]
        public IHttpActionResult UpdateAutoLikeCommentAvatar(UpdateUserAutoLikeCommentModel model)
        {
            var data = _dbAutohltEntities.UpdateAutoLikeCmtAvatarInfoFace(model.Id, model.UId, model.Name,
                model.Picture, model.IsRunWork);
            return Ok(new ResponseModel<int>()
            {
                Code = 8876,
                Message = "thanh cong",
                Data = data
            });
        }

        /// <summary>
        /// Thêm tài khoản chạy auto
        /// </summary>
        /// <returns></returns>
        [Route("AddUserAuto")]
        [HttpPost]
        public IHttpActionResult AddUser(AdduserAutoLikeCommentModel model)
        {
            var data = _dbAutohltEntities.UpdateUserAutoLikeComment(model.Id, model.RegistrationDate, model.ExpiredTime,
                model.F_Cookie, model.F_Token);
            return Ok(new ResponseModel<int>()
            {
                Code = 8876,
                Message = "thanh cong",
                Data = data
            });
        }
    }
}