using AUTOHLT.WEB.API.Database;
using AUTOHLT.WEB.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AUTOHLT.WEB.API.Controllers
{
    [RoutePrefix("api/v1/servicessehistory")]
    public class ServiceUseHistoryController : ApiController
    {
        private bsoft_autohltEntities _entities;

        public ServiceUseHistoryController()
        {
            _entities = new bsoft_autohltEntities();
        }

        /// <summary>
        /// them lich su su dung dich vu
        /// </summary>
        /// <param name="serviceUseHistoryModel"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        public IHttpActionResult AddService([FromBody] ServiceUseHistoryModel serviceUseHistoryModel)
        {
            if (serviceUseHistoryModel != null)
            {
                var data = _entities.AddServiceUseHistory(serviceUseHistoryModel.IdProductType, serviceUseHistoryModel.Content, serviceUseHistoryModel.IdUser, serviceUseHistoryModel.Number);
                if (data > 0)
                {
                    return Ok(new ResponseModel<int>
                    {
                        Code = 24,
                        Data = data,
                        Message = "Thanh cong"
                    });
                }
            }
            return Ok(new ResponseModel<string>
            {
                Code = -24,
                Data = null,
                Message = "Loi phat sinh trong qua trinh xu ly"
            });
        }

        /// <summary>
        /// lay toan bo lich su su dung dich vu
        /// </summary>
        /// <returns></returns>
        [Route("all")]
        [HttpGet]
        public IHttpActionResult GetAllService()
        {
            var data = _entities.GetAllHistoryUserSevice();
            if (data != null)
            {
                var allService = data.ToList();
                if (allService.Any())
                {
                    return Ok(new ResponseModel<List<GetAllHistoryUserSevice_Result>>
                    {
                        Code = 23,
                        Data = allService,
                        Message = "thanh cong"
                    });
                }
            }
            return Ok(new ResponseModel<string>
            {
                Code = -23,
                Data = null,
                Message = "Loi phat sinh trong qua trinh xu ly"
            });
        }

        /// <summary>
        /// lay lich su dung dich vu cho tung usser
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("servicehistory")]
        [HttpGet]
        public IHttpActionResult GetHistorySevice(Guid id)
        {
            var data = _entities.GetHistoryService(id);
            if (data != null)
            {
                var service = data.ToList();
                if (service.Any())
                {
                    return Ok(new ResponseModel<List<GetHistoryService_Result>>
                    {
                        Code = 22,
                        Data = service,
                        Message = "Thanh cong"
                    });
                }
            }
            return Ok(new ResponseModel<string>
            {
                Code = -22,
                Data = null,
                Message = "Loi phat sinh trong qua trinh xu ly"
            });
        }
    }
}