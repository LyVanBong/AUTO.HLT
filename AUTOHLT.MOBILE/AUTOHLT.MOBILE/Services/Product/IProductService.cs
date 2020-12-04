using System.Collections.Generic;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models;
using AUTOHLT.MOBILE.Models.RequestProviderModel;

namespace AUTOHLT.MOBILE.Services.Product
{
    public interface IProductService
    {
        /// <summary>
        /// Cập nhật lại sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="endDate"></param>
        /// <param name="content"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> UpdateProduct(string id, string name, string price, string endDate, string content, string number);
        /// <summary>
        /// Xoa san pham
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> DeleteProduct(string id);
        /// <summary>
        /// Thêm loại sản phẩm mới
        /// </summary>
        /// <param name="nameProduct"></param>
        /// <param name="price"></param>
        /// <param name="endDate"></param>
        /// <param name="content"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> AddProduct(string nameProduct, string price, string endDate, string content, string number);
        /// <summary>
        /// Lấy toàn bộ danh sách sản phẩm
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<ProductModel>>> GetAllProduct();
    }
}