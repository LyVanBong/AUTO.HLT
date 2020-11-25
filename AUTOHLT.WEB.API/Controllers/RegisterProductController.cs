using AUTOHLT.WEB.API.Database;
using AUTOHLT.WEB.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AUTOHLT.WEB.API.Controllers
{
    [RoutePrefix("api/v1/registerproduct")]
    public class RegisterProductController : ApiController
    {
        private bsoft_autohltEntities _entities;

        public RegisterProductController()
        {
            _entities = new bsoft_autohltEntities();
        }

        /// <summary>
        /// xoa nhung san pham khach hang da het han
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            var data = _entities.DeleteRegisterProduct(id);
            if (data > 0)
                return Ok(new ResponseModel<int>
                {
                    Code = 21,
                    Data = data,
                    Message = "Thanh cong"
                });
            return Ok(new ResponseModel<string>
            {
                Code = -21,
                Data = null,
                Message = "Loi phat sinh trong qua trinh xu ly"
            });
        }

        /// <summary>
        /// lay danh sach san pham
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("listregisterproduct")]
        [HttpGet]
        public IHttpActionResult ListRegisterProduct(Guid id)
        {
            var data = _entities.ListRegisterProduct(id);
            if (data != null)
            {
                var lsRegisterProduct = data.ToList();
                if (lsRegisterProduct.Any())
                {
                    return Ok(new ResponseModel<List<ListRegisterProduct_Result>>
                    {
                        Code = 20,
                        Data = lsRegisterProduct,
                        Message = "thanh cong"
                    });
                }
            }
            return Ok(new ResponseModel<string>
            {
                Code = -20,
                Data = null,
                Message = "Loi trong qua trinh xu ly"
            });
        }

        /// <summary>
        /// dang ky san pham
        /// </summary>
        /// <param name="registerProductModel"></param>
        /// <returns></returns>
        [Route("registerproduct")]
        [HttpPost]
        public IHttpActionResult Register([FromBody] RegisterProductModel registerProductModel)
        {
            if (registerProductModel != null)
            {
                var data = _entities.RegisterProduct(registerProductModel.IdProduct, registerProductModel.IdUser);
                if (data > 0)
                    return Ok(new ResponseModel<int>
                    {
                        Code = 18,
                        Data = data,
                        Message = "thanh cong"
                    });
            }

            return Ok(new ResponseModel<string>
            {
                Code = -18,
                Data = null,
                Message = "Loi phat sinh trong qua trinh xu ly"
            });
        }
    }
}