using AUTOHLT.MOBILE.Models.RequestProviderModel;
using Microsoft.AppCenter.Crashes;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace AUTOHLT.MOBILE.Services.RestSharp
{
    public class RestSharpService : IRestSharpService
    {
        private IRestClient _client;
        private IRestRequest _request;
        public RestSharpService()
        {
            _client = new RestClient();
            _client.Timeout = -1;
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
        private void AddHeader(IReadOnlyCollection<RequestParameter> headers)
        {
            try
            {
                var cookie = headers.FirstOrDefault();
                if (cookie != null)
                {
                    var cookieContainer = new CookieContainer();
                    var data = cookie?.Value?.Split(';');
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            var ckie = item?.Split('=');
                            if (ckie != null)
                            {
                                cookieContainer.Add(new Cookie
                                {
                                    Name = ckie[0],
                                    Value = ckie[1],
                                    Domain = ".facebook.com",
                                    Path = "/"
                                });
                            }
                        }
                        _client.CookieContainer = cookieContainer;
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
        public async Task<string> GetAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, IReadOnlyCollection<RequestParameter> headers = null)
        {
            try
            {
                CreateClients(uri);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }

                if (headers != null && headers.Any())
                {
                    AddHeader(headers);
                }

                var response = await _client.ExecuteAsync(_request);

                return response.Content;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> PostAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, IReadOnlyCollection<RequestParameter> headers = null)
        {
            try
            {
                CreateClients(uri, Method.POST);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }
                if (headers != null && headers.Any())
                {
                    AddHeader(headers);
                }
                var response = await _client.ExecuteAsync(_request);
                return response.Content;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> PutAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, IReadOnlyCollection<RequestParameter> headers = null)
        {
            try
            {
                CreateClients(uri, Method.PUT);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }
                if (headers != null && headers.Any())
                {
                    AddHeader(headers);
                }
                var response = await _client.ExecuteAsync(_request);
                return response.Content;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> DeleteAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, IReadOnlyCollection<RequestParameter> headers = null)
        {
            try
            {
                CreateClients(uri, Method.DELETE);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }
                if (headers != null && headers.Any())
                {
                    AddHeader(headers);
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