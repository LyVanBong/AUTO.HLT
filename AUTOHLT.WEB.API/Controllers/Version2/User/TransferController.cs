using System;
using System.Web.Http;
using AUTOHLT.WEB.API.Models;

namespace AUTOHLT.WEB.API.Controllers.Version2.User
{
    [RoutePrefix("api/version2/user")]
    public class TransferController : BaseController
    {
        /// <summary>
        /// chuyen tien cho user
        /// </summary>
        /// <param name="transfer"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("transfermoney")]
        public IHttpActionResult TransferMoney([FromBody] TransferMoneyModel transfer)
        {
            var veri = Verifying(Request);
            if (veri != null && veri.UserName != null)
            {
                var data = DatabaseAutohlt.UserTransfers(veri.IdUser, transfer.IdReceive, transfer.Price);
                if (data >0)
                {
                    return Ok(new ResponseModel<int>
                    {
                        Code = 23,
                        Message = "Thanh cong",
                        Data = data,
                    });
                }
            }
            return Ok(new ResponseModel<int>
            {
                Code = -23,
                Message = "Loi",
                Data = 0,
            });
        }
    }
    public class TransferMoneyModel
    {
        public Guid IdReceive { get; set; }
        public int Price { get; set; }
    }
}