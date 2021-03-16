using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Product;
using AUTOHLT.MOBILE.Services.Telegram;
using Microsoft.AppCenter.Crashes;
using Prism.Mvvm;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;
using AUTOHLT.MOBILE.Models.Telegram;
using AUTOHLT.MOBILE.Services.Database;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.Controls.Dialog.UseService
{
    public class UseServiceDialogViewModel : BindableBase, IDialogAware
    {
        private string _title;
        private string _textSubmit;
        private string _maxNumber;
        private string _content;
        private string _number;
        private IProductService _productService;
        private string _idUser;
        private string _idProduct;
        private IPageDialogService _pageDialogService;
        private ITelegramService _telegramService;
        private IDatabaseService _databaseService;
        public string Number
        {
            get => _number;
            set
            {
                if (SetProperty(ref _number, value))
                {
                    if (value != null)
                    {
                        if (int.Parse(value) > int.Parse(MaxNumber))
                            Number = MaxNumber;
                    }
                }
            }
        }

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public string MaxNumber
        {
            get => _maxNumber;
            set => SetProperty(ref _maxNumber, value);
        }

        public ICommand UseServiceCommand { get; private set; }

        public string TextSubmit
        {
            get => _textSubmit;
            set => SetProperty(ref _textSubmit, value);
        }

        public ICommand CloseCommand { get; private set; }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public UseServiceDialogViewModel(IProductService productService, IPageDialogService pageDialogService, ITelegramService telegramService, IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _telegramService = telegramService;
            _pageDialogService = pageDialogService;
            _productService = productService;
            CloseCommand = new Command(CloseDialog);
            UseServiceCommand = new Command(UseService);
        }

        private async void UseService()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Content) && !string.IsNullOrWhiteSpace(Number))
                {
                    var addHistoryUse = await _productService.AddHistoryUseService(_idProduct, Content, _idUser, Number, DateTime.Now.ToString("yyy/MM/dd hh:mm:ss"));
                    if (addHistoryUse != null && addHistoryUse.Code > 0 && addHistoryUse.Data != null)
                    {
                        await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000040, "OK");
                        var user = await _databaseService.GetAccountUser();
                        var content = JsonConvert.SerializeObject(new MessageNotificationTelegramModel
                        {
                            Ten_Thong_Bao = Title,
                            So_Luong = int.Parse(Number),
                            Id_Nguoi_Dung = user?.ID,
                            Noi_Dung_Thong_Bao = new
                            {
                                Noi_Dung =Content,
                            },
                            Ghi_Chu = new
                            {
                                Ten = user?.Name,
                                Tai_Khoan = user?.UserName,
                                So_dien_thoai = user?.NumberPhone
                            }
                        }, Formatting.Indented);
                        var tele = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork,
                            content);
                        CloseDialog();
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000041, "OK");
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private void CloseDialog()
        {
            if (RequestClose != null)
                RequestClose(null);
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters != null)
            {
                _idUser = parameters.GetValue<string>("IdUser");
                _idProduct = parameters.GetValue<string>("IdProduct");
                TextSubmit = parameters.GetValue<string>("Title");
                MaxNumber = parameters.GetValue<string>("Number");
                Title = Resource._1000058 + " " + TextSubmit;
            }
        }

        public event Action<IDialogParameters> RequestClose;
    }
}