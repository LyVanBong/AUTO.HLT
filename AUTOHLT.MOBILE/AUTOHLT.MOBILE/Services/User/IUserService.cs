﻿using AUTOHLT.MOBILE.Models.RequestProviderModel;
using System.Threading.Tasks;

namespace AUTOHLT.MOBILE.Services.User
{
    public interface IUserService
    {
        /// <summary>
        /// service them nguoi gioi thieu
        /// </summary>
        /// <param name="userGioiThieu"></param>
        /// <param name="userDuocGioiThieu"></param>
        /// <param name="discount"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> ThemGioiThieu(string userGioiThieu, string userDuocGioiThieu, int discount, string note);

        /// <summary>
        /// Kiem su ton tai cua sdt
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> CheckExistNumberPhone(string number);

        /// <summary>
        /// gui ma otp
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> SendOtp(string number);

        /// <summary>
        /// Nap tien cho user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> SetMoneyUser(string userName, string price);

        /// <summary>
        /// Ghi lai lich su set tien cho user
        /// </summary>
        /// <param name="discount"></param>
        /// <param name="price"></param>
        /// <param name="idSend"></param>
        /// <param name="idReceive"></param>
        /// <param name="transferType"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> HistorySetMoneyForUser(string discount, string price, string idSend, string idReceive, string transferType);

        /// <summary>
        /// Cập lại user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> UpdateUser(string userName, string name, string pass, string email, string number, string sex, string role, string isActive, string age, string price, string idDevice);

        /// <summary>
        /// Chuyển tiền từ user sang user
        /// </summary>
        /// <param name="idSend"></param>
        /// <param name="idReceive"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> TransferMoney(string idSend, string idReceive, string price);

        /// <summary>
        /// Lấy số dư hiện tại của user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> GetMoneyUser(string userName);

        /// <summary>
        /// Kiểm tra sự tồn tại của username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> CheckExistAccount(string userName);
    }
}