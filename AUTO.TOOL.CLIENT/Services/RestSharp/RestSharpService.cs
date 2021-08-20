﻿using AUTO.TOOL.CLIENT.Models.RequestProviderModel;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AUTO.TOOL.CLIENT.Services.RestSharp
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
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e);
            }
        }

        /// <summary>
        /// them du lieu chuyen vao request
        /// </summary>
        /// <param name="parameters"></param>
        private void AddPrarameter(IReadOnlyCollection<RequestParameter> parameters)
        {
            try
            {
                foreach (var item in parameters)
                {
                    _request.AddParameter(item.Key, item.Value);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e);
            }
        }

        /// <summary>
        /// them cookie
        /// </summary>
        /// <param name="cookie"></param>
        private void AddHeader(string cookie)
        {
            try
            {
                if (cookie != null)
                {
                    var cookieContainer = new CookieContainer();
                    var data = cookie.Split(';');
                    if (data.Any())
                    {
                        foreach (var item in data)
                        {
                            var ckie = item?.Split('=');
                            if (ckie.Any())
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
                Debug.WriteLine("Error: " + ex);
            }
        }

        public async Task<string> GetAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, string cookie = null)
        {
            try
            {
                CreateClients(uri);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }

                if (!string.IsNullOrWhiteSpace(cookie))
                {
                    AddHeader(cookie);
                }

                var response = await _client.ExecuteAsync(_request);

                return response.Content;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e);
                return null;
            }
        }

        public async Task<string> PostAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, string cookie = null)
        {
            try
            {
                CreateClients(uri, Method.POST);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }
                if (!string.IsNullOrWhiteSpace(cookie))
                {
                    AddHeader(cookie);
                }
                var response = await _client.ExecuteAsync(_request);
                return response.Content;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e);
                return null;
            }
        }

        public async Task<string> PutAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, string cookie = null)
        {
            try
            {
                CreateClients(uri, Method.PUT);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }
                if (!string.IsNullOrWhiteSpace(cookie))
                {
                    AddHeader(cookie);
                }
                var response = await _client.ExecuteAsync(_request);
                return response.Content;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e);
                return null;
            }
        }

        public async Task<string> DeleteAsync(string uri, IReadOnlyCollection<RequestParameter> parameters = null, string cookie = null)
        {
            try
            {
                CreateClients(uri, Method.DELETE);
                if (parameters != null && parameters.Any())
                {
                    AddPrarameter(parameters);
                }
                if (!string.IsNullOrWhiteSpace(cookie))
                {
                    AddHeader(cookie);
                }
                var response = await _client.ExecuteAsync(_request);
                return response.Content;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e);
                return null;
            }
        }
    }
}