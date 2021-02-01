using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AUTOHLT.WEB.API.Database;
using AUTOHLT.WEB.API.Models;

namespace AUTOHLT.WEB.API.Controllers
{
    [RoutePrefix("api/v1/HDSD")]
    public class HuongDanSuDungController : ApiController
    {
        private bsoft_autohltEntities _entities;
        public HuongDanSuDungController()
        {
            _entities = new bsoft_autohltEntities();
        }

        [Route("Delete")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var data = _entities.DeleteHDSD(id);
            return Ok(new ResponseModel<int>()
            {
                Code = 123123,
                Message = "Thanh Cong",
                Data = data,
            });
        }

        [Route("Get")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            var data = _entities.GetDataHDSD()?.ToList();
            if (data != null && data.Any())
            {
                return Ok(new ResponseModel<List<GetDataHDSD_Result>>()
                {
                    Code = 45634,
                    Message = "Thanh Cong",
                    Data = data,
                });
            }
            return Ok(new ResponseModel<string>()
            {
                Code = -45634,
                Message = "Loi",
                Data = null
            });
        }

        [Route("Add")]
        [HttpPost]
        public IHttpActionResult Add(HuongDanSuDungModel model)
        {
            var data = _entities.AddHDSD(model.Url,model.Note);
            return Ok(new ResponseModel<int>()
            {
                Code = 4534,
                Message = "Thanh cong",
                Data = data,
            });
        }
    }
}
