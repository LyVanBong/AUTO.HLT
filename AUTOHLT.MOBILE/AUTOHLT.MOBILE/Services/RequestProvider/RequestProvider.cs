using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AUTOHLT.MOBILE.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        private IRestClient _client;
        private IRestRequest _request;

        public RequestProvider()
        {
            _client = new RestClient();
            //_client.Timeout = 10000;
        }

        public async Task<ResponseModel<T>> GetAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null)
        {
            try
            {
                CreateClients(uri);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }

                var response = await _client.ExecuteAsync(_request);
                var data = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<ResponseModel<T>>(response.Content)
                    : default;
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateClients(string uri, Method method = Method.GET)
        {
            try
            {
                uri = AppConstants.UriApi + uri;
                _client.BaseUrl = new Uri(uri);
                _request = new RestRequest(method);
            }
            catch (Exception)
            {
            }
        }

        public async Task<ResponseModel<T>> PostAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters)
        {
            try
            {
                CreateClients(uri, Method.POST);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }

                var response = await _client.ExecuteAsync(_request);
                var data = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<ResponseModel<T>>(response.Content)
                    : default;
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ResponseModel<T>> PutAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters)
        {
            try
            {
                CreateClients(uri, Method.PUT);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }

                var response = await _client.ExecuteAsync(_request);
                var data = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<ResponseModel<T>>(response.Content)
                    : default;
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// them tham so vao api
        /// </summary>
        /// <param name="parameters"></param>
        private void AddPrarameter(IReadOnlyCollection<RequestParameter> parameters)
        {
            foreach (var item in parameters)
            {
                _request.AddParameter(item.Key, item.Value);
            }
        }

        public async Task<ResponseModel<T>> DeleteAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters)
        {
            try
            {
                CreateClients(uri, Method.DELETE);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }

                var response = await _client.ExecuteAsync(_request);
                var data = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<ResponseModel<T>>(response.Content)
                    : default;
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}