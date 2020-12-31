using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using Newtonsoft.Json;
using RestSharp;

namespace AUTOHLT.MOBILE.Services.RestSharp
{
    public class RestSharpService : IRestSharpService
    {
        private IRestClient _client;
        private IRestRequest _request;
        public RestSharpService()
        {
            _client = new RestClient();
            //_client.Timeout = 10000;
        }
        private void CreateClients(string uri, Method method = Method.GET)
        {
            try
            {
                _client.BaseUrl = new Uri(uri);
                _request = new RestRequest(method);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void AddPrarameter(IReadOnlyCollection<RequestParameter> parameters)
        {
            try
            {
                foreach (var item in parameters)
                {
                    _request.AddParameter(item.Key, item.Value);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<string> GetAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null)
        {
            try
            {
                CreateClients(uri);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }

                var response = await _client.ExecuteAsync(_request);

                return response.Content;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> PostAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null)
        {
            try
            {
                CreateClients(uri, Method.POST);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }

                var response = await _client.ExecuteAsync(_request);
                return response.Content;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> PutAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null)
        {
            try
            {
                CreateClients(uri, Method.PUT);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }

                var response = await _client.ExecuteAsync(_request);
                return response.Content;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> DeleteAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null)
        {
            try
            {
                CreateClients(uri, Method.DELETE);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }

                var response = await _client.ExecuteAsync(_request);
                return response.Content;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}