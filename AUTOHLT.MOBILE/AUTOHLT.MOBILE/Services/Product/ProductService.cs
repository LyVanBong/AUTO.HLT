using AUTOHLT.MOBILE.Models.HistoryService;
using AUTOHLT.MOBILE.Models.Product;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOHLT.MOBILE.Services.Product
{
    public class ProductService : IProductService
    {
        private IRequestProvider _requestProvider;

        public ProductService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<string>> AddHistoryUseService(string idProduct, string content, string idUser, string number, string dateCreate)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("IdProductType",idProduct),
                    new RequestParameter("Content",content),
                    new RequestParameter("IdUser",idUser),
                    new RequestParameter("Number",number),
                    new RequestParameter("DateCreate",dateCreate),
                };
                var data = await _requestProvider.PostAsync<string>("servicessehistory/add", para);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<IEnumerable<HistoryUserServiceModel>>> GetHistoryUseServiceForUser(string id)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("id",id),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<HistoryUserServiceModel>>("servicessehistory/servicehistory", para);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<string>> RegisterProduct(string idProduct, string idUser)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("IdProduct",idProduct),
                    new RequestParameter("IdUser",idUser),
                };
                var data = await _requestProvider.PostAsync<string>("registerproduct/registerproduct", para);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<IEnumerable<ListRegisterProductModel>>> GetListRegisterProductForUser(string id)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("ID",id),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<ListRegisterProductModel>>("registerproduct/listregisterproduct", para);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<string>> UpdateProduct(string id, string name, string price, string endDate, string content, string number)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("ID",id),
                    new RequestParameter("Name",name),
                    new RequestParameter("Price",price),
                    new RequestParameter("EndDate",endDate),
                    new RequestParameter("Content",content),
                    new RequestParameter("Number",number),
                };
                var data = await _requestProvider.PutAsync<string>("producttype/update", para);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<string>> DeleteProduct(string id)
        {
            try
            {
                var para = new List<RequestParameter> { new RequestParameter("id", id) };
                var data = await _requestProvider.DeleteAsync<string>("producttype/delete", para);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<string>> AddProduct(string nameProduct, string price, string endDate, string content, string number)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("Name",nameProduct),
                    new RequestParameter("Price",price),
                    new RequestParameter("EndDate",endDate),
                    new RequestParameter("Content",content),
                    new RequestParameter("Number",number),
                };
                var data = await _requestProvider.PostAsync<string>("producttype/add", para);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<IEnumerable<ProductModel>>> GetAllProduct()
        {
            try
            {
                var data = await _requestProvider.GetAsync<IEnumerable<ProductModel>>("producttype/all");
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}