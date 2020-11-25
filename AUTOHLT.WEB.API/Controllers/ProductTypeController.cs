using AUTOHLT.WEB.API.Database;
using AUTOHLT.WEB.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AUTOHLT.WEB.API.Controllers
{
    [RoutePrefix("api/v1/producttype")]
    public class ProductTypeController : ApiController
    {
        private bsoft_autohltEntities _entities;

        public ProductTypeController()
        {
            _entities = new bsoft_autohltEntities();
        }

        /// <summary>
        /// lay toan bo san pham
        /// </summary>
        /// <returns></returns>
        [Route("all")]
        [HttpGet]
        public IHttpActionResult GetAllProductType()
        {
            var data = _entities.GetAllProductType();
            if (data != null)
            {
                var productType = data.ToList();
                if (productType.Any())
                    return Ok(new ResponseModel<List<GetAllProductType_Result>>
                    {
                        Code = 19,
                        Data = productType,
                        Message = "Thanh cong"
                    });
            }
            return Ok(new ResponseModel<string>
            {
                Code = -19,
                Data = null,
                Message = "Loi phat sinh trong qua trinh xu ly"
            });
        }

        /// <summary>
        /// Cap lai san pham
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPut]
        public IHttpActionResult UpdateProductType([FromBody] UpdateProductModel productModel)
        {
            if (productModel != null)
            {
                var data = _entities.UpdateProductType(productModel.ID, productModel.Name, productModel.Price,
                    productModel.EndDate, productModel.Content, productModel.Number);
                if (data > 0)
                    return Ok(new ResponseModel<int>
                    {
                        Code = 17,
                        Data = data,
                        Message = "thanh cong"
                    });
            }

            return Ok(new ResponseModel<string>
            {
                Code = -17,
                Data = null,
                Message = "Loi phat sinh trong qua trinh xu ly"
            });
        }

        /// <summary>
        /// api dùng để xóa sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpDelete]
        public IHttpActionResult DeleteProductType(Guid id)
        {
            var data = _entities.DeleteProductType(id);
            if (data > 0)
                return Ok(new ResponseModel<int>
                {
                    Code = 16,
                    Data = data,
                    Message = "Thanh cong"
                });
            return Ok(new ResponseModel<string>
            {
                Code = -16,
                Data = null,
                Message = "Lỗi phát sinh trong quá trình xử lý"
            });
        }

        /// <summary>
        /// api thêm sản phẩm mới
        /// </summary>
        /// <param name="productTypeModel"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        public IHttpActionResult AddProductType([FromBody] ProductTypeModel productTypeModel)
        {
            if (productTypeModel != null)
            {
                var data = _entities.AddProductType(productTypeModel.Name, productTypeModel.Price, productTypeModel.EndDate, productTypeModel.Content, productTypeModel.Number);
                if (data > 0)
                    return Ok(new ResponseModel<int>
                    {
                        Code = 15,
                        Data = data,
                        Message = "Thanh cong"
                    });
            }

            return Ok(new ResponseModel<string>
            {
                Code = -15,
                Data = null,
                Message = "Xẩy ra lỗi trong quá trình xử lý"
            });
        }
    }
}