﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AUTO.HLT.MOBILE.VIP.Models.LicenseKey;
using AUTO.HLT.MOBILE.VIP.Models.RequestProviderModel;
using AUTO.HLT.MOBILE.VIP.Services.RequestProvider;
using Microsoft.AppCenter.Crashes;

namespace AUTO.HLT.MOBILE.VIP.Services.LicenseKey
{
    public class LicenseKeyService : ILicenseKeyService
    {
        private IRequestProvider _requestProvider;
        public LicenseKeyService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<string>> CreateLicense(string idUser, string amountKey, string numDateUseKey)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("IdUserAgecy",idUser),
                    new RequestParameter("TypeKey",numDateUseKey),
                    new RequestParameter("AmountKey",amountKey),
                };
                var data = await _requestProvider.PostAsync<string>("LicenseKey/Creates", para);
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            return null;
        }

        public async Task<ResponseModel<List<AgecyLicenseModel>>> GetLicenseForAgecy()
        {
            try
            {
                var data = await _requestProvider.GetAsync<List<AgecyLicenseModel>>("LicenseKey/GetLicensekeyForAgecy");
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            return null;
        }

        public async Task<ResponseModel<string>> UpdateHistory(string key, string content)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("LicenseKey",key),
                    new RequestParameter("ContentHistory",content),
                };
                var data = await _requestProvider.PutAsync<string>("LicenseKey/UpdateHistoryUseProduct", para);
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }

        public async Task<bool> ActiveLiceseKey(string key)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("LicenseKey",key),
                    new RequestParameter("DateActiveLicense",DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")),
                };
                var data = await _requestProvider.PutAsync<string>("LicenseKey/ActiveLicense", para);
                if (data != null && data.Code > 0 && data.Data != null)
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return false;
            }
        }

        public async Task<LicenseKeyModel> CheckLicenseForUser()
        {
            try
            {
                var data = await _requestProvider.GetAsync<LicenseKeyModel>("LicenseKey/CheckLicenseKey");
                if (data != null && data.Code > 0 && data.Data != null)
                    return data.Data;
                return null;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }
    }
}