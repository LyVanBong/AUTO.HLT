﻿using FireSharp;
using FireSharp.Config;
using FireSharp.Response;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace AUTO.ALL.IN.APP.Services
{
    public static class RealtimeDatabaseService
    {
        /// <summary>
        /// Khởi tạo Realtime Database
        /// </summary>
        /// <returns></returns>
        public static FirebaseClient FirebaseClient()
        {
            var config = new FirebaseConfig
            {
                AuthSecret = "Pmjcq37N2D5PnylJdiijRK9rXyePSC5OSNs48uaM",
                BasePath = "https://autohlt-default-rtdb.firebaseio.com"
            };
            return new FirebaseClient(config);
        }

        public static async Task<T> Update<T>(string path, T data)
        {
            try
            {
                FirebaseResponse response = await FirebaseClient().PushAsync(path, data);
                return response.ResultAs<T>();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return default;
        }

        public static async Task<bool> Post<T>(string path, T data)
        {
            try
            {
                FirebaseResponse response = await FirebaseClient().SetAsync(path, data);
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return false;
        }

        public static async Task<T> Get<T>(string path)
        {
            try
            {
                FirebaseResponse response = await FirebaseClient().GetAsync(path);
                return response.ResultAs<T>();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return default;
        }
    }
}