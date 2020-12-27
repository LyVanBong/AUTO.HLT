using AUTOHLT.WEB.API.Database;
using AUTOHLT.WEB.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AUTOHLT.WEB.API.Controllers
{
    [RoutePrefix("api/v1/user")]
    public class UserController : ApiController
    {
        private bsoft_autohltEntities _entities;

        public UserController()
        {
            _entities = new bsoft_autohltEntities();
        }

        /// <summary>
        /// set số dư cho user
        /// </summary>
        /// <param name="moneyModel"></param>
        /// <returns></returns>
        [Route("SetMoney")]
        [HttpPut]
        public IHttpActionResult SetMoneyUser(MoneyModel moneyModel)
        {
            var data = _entities.SetMoneyUser(moneyModel.UserName, moneyModel.Price);
            if (data > 0)
            {
                return Ok(new ResponseModel<int>
                {
                    Code = 987,
                    Message = "Thanh cong",
                    Data = data
                });
            }
            else
            {
                return Ok(new ResponseModel<int>
                {
                    Code = -987,
                    Message = "Loi phat sinh",
                    Data = data
                });
            }
        }
        /// <summary>
        /// kiem tra tai khoan user da ton tai hay chua
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [Route("checkusername")]
        [HttpGet]
        public IHttpActionResult CheckUserName(string userName)
        {
            Guid? guid = new Guid();
            if (userName != null)
            {
                var data = _entities.CheckUserName(userName);
                if (data != null)
                {
                    guid = data.SingleOrDefault();
                    if (guid == null)
                    {
                        return Ok(new ResponseModel<string>
                        {
                            Code = 8,
                            Data = null,
                            Message = $"User {userName} chua ton tai"
                        });
                    }
                }
            }

            return Ok(new ResponseModel<string>
            {
                Code = -8,
                Data = guid.ToString(),
                Message = $"User {userName} da ton tai"
            });
        }

        /// <summary>
        /// api xóa tài khoản user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [Route("deleteuser")]
        [HttpDelete]
        public IHttpActionResult DeleteUser(string userName)
        {
            if (userName != null)
            {
                var data = _entities.DeleteUser(userName);
                if (data > 0)
                    return Ok(new ResponseModel<int>
                    {
                        Code = 7,
                        Data = data,
                        Message = $"Xoa Tai khoan {userName} thanh cong"
                    });
            }
            return Ok(new ResponseModel<string>
            {
                Code = -7,
                Data = null,
                Message = "Lỗi phát sinh trong qua trình xóa tài khoản"
            });
        }

        /// <summary>
        /// api lấy toàn bộ danh sách user
        /// </summary>
        /// <returns></returns>
        [Route("getalluser")]
        [HttpGet]
        public IHttpActionResult GetAllUser()
        {
            var data = _entities.GetAllUser();
            if (data != null)
            {
                return Ok(new ResponseModel<List<GetAllUser_Result>>
                {
                    Code = 6,
                    Data = data.ToList(),
                    Message = "Lấy toàn bộ user"
                });
            }
            return Ok(new ResponseModel<string>
            {
                Code = -6,
                Data = null,
                Message = "Lỗi không lấy được danh sách user"
            });
        }

        /// <summary>
        /// api lay ra so tien hien tai cua user
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [Route("getpriceuser")]
        [HttpGet]
        public IHttpActionResult Get(string UserName)
        {
            if (UserName != null)
            {
                var data = _entities.GetPriceUser(userName: UserName);
                if (data != null)
                {
                    var price = data.SingleOrDefault() + "";
                    return Ok(new ResponseModel<string>
                    {
                        Code = 5,
                        Data = price,
                        Message = $"Số tiền hiện tại của user {price}"
                    });
                }
            }
            return Ok(new ResponseModel<string>
            {
                Code = -5,
                Data = null,
                Message = "Ban vui long nap them tien vao tai khoan"
            });
        }

        /// <summary>
        /// api cap nhat user
        /// </summary>
        /// <param name="updateUserModel"></param>
        /// <returns></returns>
        [Route("updateuser")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] UpdateUserModel updateUserModel)
        {
            if (updateUserModel != null)
            {
                var data = _entities.UpdateUser(updateUserModel.UserName, updateUserModel.Password,
                    updateUserModel.Name, updateUserModel.NumberPhone, updateUserModel.Email, updateUserModel.Sex,
                    updateUserModel.Role, updateUserModel.IsActive, updateUserModel.Age, updateUserModel.Price,
                    updateUserModel.IdDevice);
                if (data > 0)
                    return Ok(new ResponseModel<int>
                    {
                        Code = 4,
                        Data = data,
                        Message = "Cập nhật user thành công"
                    });
            }
            return Ok(new ResponseModel<string>
            {
                Code = -4,
                Data = null,
                Message = "Lỗi phát sinh trong quá trinh cập nhật lại tài khoản"
            });
        }

        /// <summary>
        /// api chuyen tien
        /// </summary>
        /// <param name="transferModel"></param>
        /// <returns></returns>
        [Route("transfermoney")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] TransferModel transferModel)
        {
            if (transferModel != null)
            {
                var data = _entities.UserTransfers(transferModel.IdSend, transferModel.IdReceive, transferModel.Price);
                if (data > 0)
                    return Ok(new ResponseModel<int>
                    {
                        Code = 3,
                        Data = data,
                        Message = "Truyển tiền thành công"
                    });
            }

            return Ok(new ResponseModel<string>
            {
                Code = -3,
                Data = null,
                Message = "Lỗi phát sinh trong qua trinh chuyển tiền"
            });
        }

        /// <summary>
        /// api dung de login app
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        public IHttpActionResult Post([FromBody] LoginModel loginModel)
        {
            if (loginModel != null)
            {
                var data = _entities.UserInformation(loginModel.UserName, loginModel.PassWord);
                if (data != null)
                    return Ok(new ResponseModel<UserInformation_Result>
                    {
                        Code = 2,
                        Data = data.FirstOrDefault(),
                        Message = "Đăng nhập thành công"
                    });
            }

            return Ok(new ResponseModel<string>
            {
                Code = -2,
                Data = null,
                Message = "Lỗi phát sinh trong quá trình đăng nhập"
            });
        }

        /// <summary>
        /// api dung de dang ky tai khoan user
        /// </summary>
        /// <param name="accountModel"></param>
        /// <returns></returns>
        [Route("registrationcccount")]
        [HttpPost]
        public IHttpActionResult Post([FromBody] RegistrationAccountModel accountModel)
        {
            if (accountModel != null)
            {
                var data = _entities.RegistrationAccount(accountModel.UserName, accountModel.Password,
                    accountModel.Name, accountModel.NumberPhone, accountModel.Email, accountModel.Sex,
                    accountModel.Age, accountModel.DateCreate);
                if (data > 0)
                    return Ok(new ResponseModel<int>
                    {
                        Code = 1,
                        Data = data,
                        Message = "Đăng ký tài khoản thành công"
                    });
            }
            return Ok(new ResponseModel<string>
            {
                Code = -1,
                Data = null,
                Message = "Lỗi phát sinh trong quá trình đăng ký tài khoản"
            });
        }
    }
}