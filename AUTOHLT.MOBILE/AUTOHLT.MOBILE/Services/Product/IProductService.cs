using AUTOHLT.MOBILE.Models.Product;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.HistoryService;

namespace AUTOHLT.MOBILE.Services.Product
{
    public interface IProductService
    {
        /// <summary>
        /// api ghi log lich sử sử dụng dịch vụ
        /// </summary>
        /// <param name="idProduct"></param>
        /// <param name="content"></param>
        /// <param name="idUser"></param>
        /// <param name="number"></param>
        /// <param name="dateCreate"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> AddHistoryUseService(string idProduct, string content, string idUser, string number, string dateCreate);
        /// <summary>
        /// lấy danh sách sử dụng dịch vụ trong ngày
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<HistoryUserServiceModel>>> GetHistoryUseServiceForUser(string id);
        /// <summary>
        /// đăng ký sản phảm
        /// </summary>
        /// <param name="idProduct"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> RegisterProduct(string idProduct, string idUser);
        /// <summary>
        /// Lấy toàn bộ danh sách user đã đăng ký sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<ListRegisterProductModel>>> GetListRegisterProductForUser(string id);
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