using AUTOHLT.WEB.API.Database;
using AUTOHLT.WEB.API.Models;
using AUTOHLT.WEB.API.Models.Version2.LicenseKey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace AUTOHLT.WEB.API.Controllers.Version2.LicenseKey
{
    [RoutePrefix("api/version2/LicenseKey")]
    public class LicenseKeyController : BaseController
    {
        /// <summary>
        /// cap nhat cac dich vu da dung
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateHistoryUseProduct")]
        public async Task<IHttpActionResult> UpdateHistoryUserService(UpdateHistoryUseProductModel input)
        {
            var veri = Verifying(Request);
            if (veri != null && veri.UserName != null && input != null)
            {
                var update = DatabaseAutohlt.sp_UpdateHistoryUseProduct(input.LicenseKey, input.ContentHistory);
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

        /// <summary>
        /// xoa key ban quyen
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete")]
        public async Task<IHttpActionResult> DeleteLicenseKey(Guid licenseKey)
        {
            var veri = Verifying(Request);
            if (veri != null && veri.UserName != null && veri.Role == 0)
            {
                var infoLicense = DatabaseAutohlt.sp_DeleteLicenseKey(licenseKey);
                if (infoLicense > 0)
                {
                    return Ok(new ResponseModel<int>()
                    {
                        Code = 240,
                        Message = "Thanh cong",
                        Data = infoLicense
                    });
                }
                else
                {
                    return Ok(new ResponseModel<string>()
                    {
                        Code = -240,
                        Message = "Loi phat sinh",
                        Data = null
                    });
                }
            }
            else
                return Ok(new ResponseModel<string>()
                {
                    Code = -240,
                    Message = "Khong co quyen truy cap tai nguyen",
                    Data = null
                });
        }

        /// <summary>
        /// lay toan bo key ban quyen
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public async Task<IHttpActionResult> GetAllLicenseKey()
        {
            var veri = Verifying(Request);
            if (veri != null && veri.UserName != null && veri.Role == 0)
            {
                var infoLicense = DatabaseAutohlt.sp_AllLicenseKey()?.ToList();
                if (infoLicense != null)
                {
                    return Ok(new ResponseModel<List<sp_AllLicenseKey_Result>>()
                    {
                        Code = 239,
                        Message = "Thanh cong",
                        Data = infoLicense
                    });
                }
                else
                {
                    return Ok(new ResponseModel<string>()
                    {
                        Code = -239,
                        Message = "Loi phat sinh",
                        Data = null
                    });
                }
            }
            else
                return Ok(new ResponseModel<string>()
                {
                    Code = -239,
                    Message = "Khong co quyen truy cap tai nguyen",
                    Data = null
                });
        }

        /// <summary>
        /// lay danh sach key ban quyen theo id dai ly
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLicensekeyForAgecy")]
        public async Task<IHttpActionResult> GetLicenseKeyForAgecy()
        {
            var veri = Verifying(Request);
            if (veri != null && veri.UserName != null && veri.Role == 3)
            {
                var infoLicense = DatabaseAutohlt.sp_InfoLicenseKeyForAgecy(veri.IdUser)?.ToList();
                if (infoLicense != null)
                {
                    return Ok(new ResponseModel<List<sp_InfoLicenseKeyForAgecy_Result>>()
                    {
                        Code = 238,
                        Message = "Thanh cong",
                        Data = infoLicense
                    });
                }
                else
                {
                    return Ok(new ResponseModel<string>()
                    {
                        Code = -238,
                        Message = "Loi phat sinh",
                        Data = null
                    });
                }
            }
            else
                return Ok(new ResponseModel<string>()
                {
                    Code = -238,
                    Message = "Khong co quyen truy cap tai nguyen",
                    Data = null
                });
        }

        /// <summary>
        /// kiem tra xem tai cua cua user da duoc kich hoat key chua
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckLicenseKey")]
        public async Task<IHttpActionResult> CheckInfoLicenseKeyForUser()
        {
            var veri = Verifying(Request);
            if (veri != null && veri.UserName != null)
            {
                var infoLicense = DatabaseAutohlt.sp_InfoLicenseKeyForUser(veri.IdUser)?.ToList();
                if (infoLicense != null)
                {
                    foreach (var item in infoLicense)
                    {
                        var endDate = item.DateActive?.AddYears(1);
                        if (endDate != null && endDate >= DateTime.Now)
                        {
                            return Ok(new ResponseModel<sp_InfoLicenseKeyForUser_Result>()
                            {
                                Code = 237,
                                Message = "Tai khoan da duoc nang cap",
                                Data = item
                            });
                        }

                    }
                    return Ok(new ResponseModel<sp_InfoLicenseKeyForUser_Result>()
                    {
                        Code = -237,
                        Message = "Ban quyen da het han",
                        Data = null,
                    });
                }
                else
                {
                    return Ok(new ResponseModel<string>()
                    {
                        Code = -237,
                        Message = "Tai khoan chua duoc nang cap",
                        Data = null
                    });
                }
            }
            else
                return Ok(new ResponseModel<string>()
                {
                    Code = -237,
                    Message = "Khong co quyen truy cap tai nguyen",
                    Data = null
                });
        }

        /// <summary>
        /// Khoa key ban quyen lai
        /// </summary>
        /// <param name="LicenseKey"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("LockLicense")]
        public async Task<IHttpActionResult> LockLicenseKey(Guid LicenseKey)
        {
            var veri = Verifying(Request);
            if (veri != null && veri.UserName != null && LicenseKey != null)
            {
                if (DatabaseAutohlt.LockLicenseKey(LicenseKey) > 0)
                    return Ok(new ResponseModel<int>()
                    {
                        Code = 236,
                        Message = "Khong co quyen truy cap tai nguyen",
                        Data = 1
                    });
                else
                    return Ok(new ResponseModel<string>()
                    {
                        Code = -236,
                        Message = "Khoa ky chua thanh cong",
                        Data = null
                    });
            }
            else
                return Ok(new ResponseModel<string>()
                {
                    Code = -236,
                    Message = "Khong co quyen truy cap tai nguyen",
                    Data = null
                });
        }

        /// <summary>
        /// them khoa ban quyen
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("ActiveLicense")]
        public async Task<IHttpActionResult> UpdateLicensekey([FromBody] AddLicenseModel input)
        {
            var veri = Verifying(Request);
            if (veri != null && veri.UserName != null && input != null)
            {
                var update = DatabaseAutohlt.sp_AddLicenseKey(input.LicenseKey, veri.IdUser, DateTime.Now);
                if (update > 0)
                    return Ok(new ResponseModel<int>()
                    {
                        Code = 235,
                        Message = "Nang cap ban quyen thanh cong",
                        Data = update
                    });
                else
                    return Ok(new ResponseModel<int>()
                    {
                        Code = -235,
                        Message = "Nang cap ban quyen chua thanh cong",
                        Data = update
                    });
            }
            else
                return Ok(new ResponseModel<string>()
                {
                    Code = -235,
                    Message = "Khong co quyen truy cap tai nguyen",
                    Data = null
                });
        }

        /// <summary>
        /// Tao khoa cho app autovip
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Creates")]
        public async Task<IHttpActionResult> CreateLicenseKey([FromBody] CreatekeyModel input)
        {
            var veri = Verifying(Request);
            if (veri != null && veri.UserName != null && input != null && input.AmountKey > 0 && veri.Role == 0)
            {
                var amount = input.AmountKey;
                var amountKey = 0;
                for (int i = 0; i < amount; i++)
                    amountKey += DatabaseAutohlt.sp_CreateLicenseKey(veri.IdUser, input.IdUserAgecy, input.TypeKey);
                if (amountKey > 0)
                    return Ok(new ResponseModel<int>()
                    {
                        Code = 234,
                        Message = "Tao khao thanh cong",
                        Data = amountKey
                    });
                else
                    return Ok(new ResponseModel<string>()
                    {
                        Code = -234,
                        Message = "Tao khao phat sinh loi",
                        Data = null
                    });
            }
            else
                return Ok(new ResponseModel<string>()
                {
                    Code = -234,
                    Message = "Khong co quyen truy cap tai nguyen",
                    Data = null
                });
        }
    }
}