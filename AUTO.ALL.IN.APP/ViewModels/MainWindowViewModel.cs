using AUTO.ALL.IN.APP.Models;
using AUTO.DLL.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AUTO.ALL.IN.APP.Services;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AUTO.ALL.IN.APP.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Công cụ hỗ trợ facebook";
        private string _dataJson;
        private UserFacebookModel _userFacebookModel = new UserFacebookModel();
        private ObservableCollection<UserFacebookModel> _dataTool = new ObservableCollection<UserFacebookModel>();

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {
            AddAccount();
        }

        #region Home

        public ObservableCollection<UserFacebookModel> DataTool
        {
            get => _dataTool;
            set => SetProperty(ref _dataTool, value);
        }

        #endregion
        #region Thêm tài khoản

        /// <summary>
        /// dữ liệu jsom để lấy thông tin tài khoản
        /// </summary>
        public string DataJson
        {
            get => _dataJson;
            set => SetProperty(ref _dataJson, value);
        }
        /// <summary>
        /// Lấy thông tin từ json nhập vào
        /// </summary>
        public ICommand GetInfoFacebookCommand { get; private set; }
        /// <summary>
        /// User facebook
        /// </summary>
        public UserFacebookModel UserFacebookModel
        {
            get => _userFacebookModel;
            set => SetProperty(ref _userFacebookModel, value);
        }

        public ICommand LoginFacebookCommand { get; private set; }
        public ICommand SaveAccountCommand { get; private set; }
        public ICommand SelectOptionStoryCommand { get; private set; }
        public ICommand SelectOptionAvatarCommand { get; private set; }
        public ICommand SelectOptionPostCommand { get; private set; }

        /// <summary>
        /// constructor của chức năng thêm tài khoản
        /// </summary>
        private void AddAccount()
        {
            GetInfoFacebookCommand = new DelegateCommand(async () => await ConvertJsonToInfo());
            LoginFacebookCommand = new DelegateCommand(async () => await LoginFacebook());
            SaveAccountCommand = new DelegateCommand<string>(async (key) => await SaveAccount(key));
            SelectOptionStoryCommand = new DelegateCommand<string>(async (story) => await SelectOptionStory(story));
            SelectOptionAvatarCommand = new DelegateCommand<string>(async (avatar) => await SelectOptionAvatar(avatar));
            SelectOptionPostCommand = new DelegateCommand<string>(async (post) => await SelectOptionPost(post));
        }

        private async Task SelectOptionPost(string post)
        {
            try
            {
                UserFacebookModel.OptionPost.IndexOptionReac = int.Parse(post);
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(SelectOptionPost));
            }
        }

        private async Task SelectOptionAvatar(string avatar)
        {
            try
            {
                UserFacebookModel.OptionAvatar.IndexOptionReac = int.Parse(avatar);
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(SelectOptionAvatar));
            }
        }

        private async Task SelectOptionStory(string story)
        {
            try
            {
                UserFacebookModel.OptionStory.IndexOptionReac = int.Parse(story);
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(SelectOptionStory));
            }
        }

        private async Task SaveAccount(string key)
        {
            try
            {
                if (key == "0")
                {
                    if (UserFacebookModel != null)
                    {
                        if (string.IsNullOrEmpty(UserFacebookModel.Id) || string.IsNullOrEmpty(UserFacebookModel.Cookie) || string.IsNullOrEmpty(UserFacebookModel.Token))
                        {
                            await ShowMessage(@"Vui lòng điền đầy đủ dữ liệu trước khi lưu !").ConfigureAwait(false);
                        }
                        else
                        {
                            if (UserFacebookModel.OptionPost.IsSelectFunction || UserFacebookModel.OPtionFriendsSuggestions.IsSelectFunction || UserFacebookModel.OptionAvatar.IsSelectFunction || UserFacebookModel.OptionStory.IsSelectFunction || UserFacebookModel.OptionMessage.IsSelectFunction)
                            {
                                var random = new Random();

                                var fields = "friends.limit(" + random.Next(4000, 5000) + "){id,name},id,name,picture,email";
                                var json = await FacebookService.GetApiFacebook(UserFacebookModel.Token, fields);
                                if (string.IsNullOrEmpty(json))
                                {
                                    if (await FacebookService.CheckTokenCookie(UserFacebookModel.Token, UserFacebookModel.Cookie))
                                    {
                                        await ShowMessage(@"Lỗi phát sinh vui lòng thử lại !").ConfigureAwait(false);
                                    }
                                    else
                                    {
                                        await ShowMessage(@"Cookie hoặc token die !").ConfigureAwait(false);
                                    }
                                }
                                else
                                {
                                    var facebook = JsonConvert.DeserializeObject<DataFacebookModel>(json);
                                    if (facebook == null)
                                    {
                                        await ShowMessage(@"Lỗi phát sinh vui lòng thử lại !").ConfigureAwait(false);
                                    }
                                    else
                                    {
                                        UserFacebookModel.DataFacebook = facebook;
                                        if (DataTool.Any(x => x.IdFacebook == UserFacebookModel.IdFacebook))
                                        {
                                            if (MessageBox.Show("Tài khoản này đã tồn tại bạn có muỗn cập nhật lại không !", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                            {
                                                var update = DataTool.FirstOrDefault(x =>
                                                    x.IdFacebook == UserFacebookModel.IdFacebook);
                                                if (update == null) return;
                                                update.DataFacebook = facebook;
                                                DataTool.Add(update);
                                                await ShowMessage(@"Cập nhật dữ liệu thành công !").ConfigureAwait(false);
                                            }
                                        }
                                        else
                                        {
                                            DataTool.Add(UserFacebookModel);
                                            await ShowMessage(@"Lưu dữ liệu thành công !").ConfigureAwait(false);
                                        }

                                        if (DataTool.Any())
                                        {
                                            await RealtimeDatabaseService.Post(nameof(UserFacebookModel), DataTool);
                                        }
                                        await ResetInput();
                                    }
                                }
                            }
                            else
                            {
                                await ShowMessage(@"Bạn chưa chọn chức năng nào để chạy công cụ !").ConfigureAwait(false);
                            }
                        }
                    }
                    else
                    {
                        await ShowMessage(@"Dữ liệu chưa được khởi tạo").ConfigureAwait(false);
                    }
                }
                else if (key == "1")
                {
                    await ResetInput().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(SaveAccount));
            }
        }

        private Task ResetInput()
        {
            DataJson = null;
            UserFacebookModel = new UserFacebookModel();
            return Task.FromResult(0);
        }

        private async Task LoginFacebook()
        {
            try
            {
                if (string.IsNullOrEmpty(UserFacebookModel.UserNameFacebook) || string.IsNullOrEmpty(UserFacebookModel.PassFacebook))
                {
                    await ShowMessage("Nhập dữ liệu chưa đủ !");
                }
                else
                {
                    var data = await FacebookService.LoginFacebook(UserFacebookModel.UserNameFacebook,
                        UserFacebookModel.PassFacebook);
                    if (data.Token != null && data.Cookie != null)
                    {
                        UserFacebookModel.Token = data.Token;
                        UserFacebookModel.Cookie = data.Cookie;
                    }
                    else
                    {
                        await ShowMessage("Đăng nhập facebook lỗi !").ConfigureAwait(false);
                    }
                }
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(LoginFacebook)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Thực hiện lấy dữ liệu từ json
        /// </summary>
        /// <returns></returns>
        private async Task ConvertJsonToInfo()
        {
            try
            {
                if (string.IsNullOrEmpty(DataJson))
                {
                    await ShowMessage("Chưa nhập dữ liệu");
                }
                else
                {
                    var data = JsonSerializer.Deserialize<JsonInfoModel>(DataJson);
                    if (data != null)
                    {
                        UserFacebookModel.Id = data.Id_Nguoi_Dung;
                        UserFacebookModel.Cookie = data.Noi_Dung_Thong_Bao.Cookie;
                        UserFacebookModel.Token = data.Noi_Dung_Thong_Bao.Token;
                        var strDate = data.Ghi_Chu.Ngay_Het_Han.Split('/');
                        UserFacebookModel.EndDate = new DateTime(int.Parse(strDate[2]), int.Parse(strDate[1]),
                            int.Parse(strDate[0]));

                        UserFacebookModel.NumberPhoneApp = data.Ghi_Chu.So_dien_thoai;
                        UserFacebookModel.NameApp = data.Ghi_Chu.Ten;
                        UserFacebookModel.UserNameApp = data.Ghi_Chu.Tai_Khoan;
                    }
                }
            }
            catch (Exception e)
            {
                await ShowMessageError(e, nameof(ConvertJsonToInfo)).ConfigureAwait(false);
            }
        }

        #endregion

        /// <summary>
        /// Hiển thị thông báo
        /// </summary>
        /// <param name="message"></param>
        private Task ShowMessage(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            return Task.FromResult(0);
        }
        /// <summary>
        /// hien thi khi co exception
        /// </summary>
        /// <param name="e"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private Task ShowMessageError(Exception e, string name)
        {
            var message = "Lỗi phát sinh tại: " + name + "\n" + "Lỗi: " + e.Message;
            MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            return Task.FromResult(0);
        }
    }
}
