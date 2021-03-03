using AUTOHLT.WEB.API.Database;
using AUTOHLT.WEB.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AUTOHLT.WEB.API.Controllers.Version1
{
    [RoutePrefix("api/v1/MoneyTransferHistory")]
    public class MoneyTransferHistoryController : ApiController
    {
        private bsoft_autohltEntities _entities;

        public MoneyTransferHistoryController()
        {
            _entities = new bsoft_autohltEntities();
        }

        /// <summary>
        /// lấy toàn bộ lịch sử chuyển tiền
        /// </summary>
        /// <returns></returns>
        [Route("all")]
        [HttpGet]
        public IHttpActionResult GetAllTransfer()
        {
            var data = _entities.GetAllTransferMoney();
            if (data != null)
            {
                var transfer = data.ToList();
                if (transfer.Any())
                {
                    return Ok(new ResponseModel<List<GetAllTransferMoney_Result>>
                    {
                        Code = 13,
                        Data = transfer,
                        Message = "Thanh cong"
                    });
                }
            }

            return Ok(new ResponseModel<string>
            {
                Code = 13,
                Data = null,
                Message = "Phát sinh lỗi trong quá trình xử lý"
            });
        }

        /// <summary>
        /// lem lich su chuyen tien moi
        /// </summary>
        /// <param name="transferModel"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        public IHttpActionResult AddTransferHistory([FromBody] AddTransferModel transferModel)
        {
            if (transferModel != null)
            {
                var data = _entities.AddTransferMoney(transferModel.Discount, transferModel.Price, transferModel.IdSend,
                    transferModel.IdReceive, transferModel.TransferType);
                if (data > 0)
                {
                    return Ok(new ResponseModel<int>
                    {
                        Code = 12,
                        Data = data,
                        Message = "Thanh cong"
                    });
                }
            }
            return Ok(new ResponseModel<string>
            {
                Code = -12,
                Data = null,
                Message = "Lỗi phát sinh trong quá trình xử lý"
            });
        }
    }
}