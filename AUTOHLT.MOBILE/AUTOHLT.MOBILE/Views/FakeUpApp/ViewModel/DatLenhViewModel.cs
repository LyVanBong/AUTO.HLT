using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AUTOHLT.MOBILE.Views.FakeUpApp.Model;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.Views.FakeUpApp.ViewModel
{
    public class DatLenhViewModel : INotifyPropertyChanged
    {
        public Command<object> ClickNormalOrderCommand { get; set; }
        public Command<object> ClickSampleOrderCommand { get; set; }
        public Command<object> ClickConditionalOrderCommand { get; set; }
        public Command<object> ClickImageExpanderCommand { get; set; }
        public Command<object> ClickBuyCommand { get; set; }
        public Command<object> ClickSellCommand { get; set; }
        public Command<object> ShowStockCommand { get; set; }
        public Command<object> StockCodeCommand { get; set; }
        public Command<object> ClickMaxCommand { get; set; }
        public Command<object> ClickOverBoughtCommand { get; set; }
        public Command<object> ClickExcessSaleCommand { get; set; }
        public Command<object> CongEntryKhoiLuongCommand { get; set; }
        public Command<object> TruEntryKhoiLuongCommand { get; set; }
        public Command<object> ShowLenhCommand { get; set; }
        public Command<object> ClickLenhUPCommand { get; set; }
        public Command<object> ClickLenhDOWNCommand { get; set; }
        public Command<object> ClickLenhOCOCommand { get; set; }
        public Command<object> ClickLenhBBCommand { get; set; }
        //lệnh Up
        public Command<object> TruGiaDieuKienCommand { get; set; }
        public Command<object> CongGiaDieuKienCommand { get; set; }
        public Command<object> TruBientruotCommand { get; set; }
        public Command<object> CongBientruotCommand { get; set; }
        //lệnh down
        public Command<object> TruGiaDieuKienDownCommand { get; set; }
        public Command<object> CongGiaDieuKienDownCommand { get; set; }
        public Command<object> TruBientruotDownCommand { get; set; }
        public Command<object> CongBientruotDownCommand { get; set; }
        //pass
        public Command<object> ShowPassCommand { get; set; }
        //lệnh OCO
        public Command<object> GiaTranCommand { get; set; }
        public Command<object> GiaThamChieuCommand { get; set; }
        public Command<object> GiaSanCommand { get; set; }
        public Command<object> GiamGiaCommand { get; set; }
        public Command<object> TangGiaCommand { get; set; }
        public Command<object> TruGiaDieuKienOCOCommand { get; set; }
        public Command<object> CongGiaDieuKienOCOCommand { get; set; }
        public Command<object> TruGiaBientruotOCOCommand { get; set; }
        public Command<object> CongGiaBientruotOCOCommand { get; set; }
        //lệnh BB
        public Command<object> TangKhoangLaiCommand { get; set; }
        public Command<object> GiamKhoangLaiCommand { get; set; }
        public Command<object> GiamKhoangLoCommand { get; set; }
        public Command<object> TangKhoangLoCommand { get; set; }
        public Command<object> GiamBienTruotBBCommand { get; set; }
        public Command<object> TangBienTruotBBCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //lệnh BB
        private string _textKhoangLaiBB;
        private string _textGiaChotLaiBB;
        private string _textKhoangLoBB;
        private string _textGiaDieuKienBB;
        private string _textGiaDieuChinhBB;
        private double GiaKhoangLo;
        private double GiaKhoangLai;
        private string _textBienTruotBB;

        public string TextBienTruotBB
        {
            get { return _textBienTruotBB; }
            set
            {
                _textBienTruotBB = value;
                OnPropertyChanged("TextBienTruotBB");
            }
        }
        public string TextGiaDieuChinhBB
        {
            get { return _textGiaDieuChinhBB; }
            set
            {
                _textGiaDieuChinhBB = value;
                OnPropertyChanged("TextGiaDieuChinhBB");
            }
        }
        public string TextGiaDieuKienBB
        {
            get { return _textGiaDieuKienBB; }
            set
            {
                _textGiaDieuKienBB = value;
                OnPropertyChanged("TextGiaDieuKienBB");
            }
        }
        public string TextKhoangLoBB
        {
            get { return _textKhoangLoBB; }
            set
            {
                _textKhoangLoBB = value;
                OnPropertyChanged("TextKhoangLoBB");
            }
        }
        public string TextGiaChotLaiBB
        {
            get { return _textGiaChotLaiBB; }
            set
            {
                _textGiaChotLaiBB = value;
                OnPropertyChanged("TextGiaChotLaiBB");
            }
        }
        public string TextKhoangLaiBB
        {
            get { return _textKhoangLaiBB; }
            set
            {
                _textKhoangLaiBB = value;
                OnPropertyChanged("TextKhoangLaiBB");
            }
        }

        //xử lý button xác nhận
        private string _textBtnXacNhan;
        private string _btnBackgroundPass;
        private bool _isPassword;

        public string BtnBackgroundPass
        {
            get { return _btnBackgroundPass; }
            set
            {
                _btnBackgroundPass = value;
                OnPropertyChanged("BtnBackgroundPass");
            }
        }
        public bool IsPassword
        {
            get { return _isPassword; }
            set
            {
                _isPassword = value;
                OnPropertyChanged("IsPassword");
            }
        }
        public string TextBtnXacNhan
        {
            get { return _textBtnXacNhan; }
            set
            {
                _textBtnXacNhan = value;
                OnPropertyChanged("TextBtnXacNhan");
            }
        }

        //xử lý giao diện lệnh OCO
        private string _textpriceAdjustmentOCO;//entry giá điều kiện
        private double _textSlipOCO;//entry biên trượt
        private string _textSetPriceOCO;//label giá điều chỉnh

        public string TextSetPriceOCO
        {
            get { return _textSetPriceOCO; }
            set
            {
                _textSetPriceOCO = value;
                OnPropertyChanged("TextSetPriceOCO");
            }
        }
        public double TextSlipOCO
        {
            get { return _textSlipOCO; }
            set
            {
                _textSlipOCO = value;
                OnPropertyChanged("TextSlipOCO");
            }
        }
        public string TextpriceAdjustmentOCO
        {
            get { return _textpriceAdjustmentOCO; }
            set
            {
                _textpriceAdjustmentOCO = value;
                OnPropertyChanged("TextpriceAdjustmentOCO");
            }
        }

        //xử lý entry giá
        private double GiaCoPhieu;
        private bool _isEntryColorGia;
        private bool _isShowEntryGia;
        private string _textEntryGia;
        private string _textGiaChotLai;
        public string TextGiaChotLai
        {
            get { return _textGiaChotLai; }
            set
            {
                _textGiaChotLai = value;
                OnPropertyChanged("TextGiaChotLai");
            }
        }
        public string TextEntryGia
        {
            get { return _textEntryGia; }
            set
            {
                _textEntryGia = value;
                OnPropertyChanged("TextEntryGia");
            }
        }
        public bool IsEntryColorGia
        {
            get { return _isEntryColorGia; }
            set
            {
                _isEntryColorGia = value;
                OnPropertyChanged("IsEntryColorGia");
            }
        }
        public bool IsShowEntryGia
        {
            get { return _isShowEntryGia; }
            set
            {
                _isShowEntryGia = value;
                OnPropertyChanged("IsShowEntryGia");
            }
        }

        // xử lý giao điện lệnh UP
        private string _textpriceAdjustment;//entry giá điều kiện
        private double GiaDieuKien;
        private double _textSlip;//entry biên trượt
        private string _textSetPrice;//label giá điều chỉnh
        private double GiaDieuChinh;

        public string TextSetPrice
        {
            get { return _textSetPrice; }
            set
            {
                _textSetPrice = value;
                OnPropertyChanged("TextSetPrice");
            }
        }
        public double TextSlip
        {
            get { return _textSlip; }
            set
            {
                _textSlip = value;
                OnPropertyChanged("TextSlip");
            }
        }
        public string TextpriceAdjustment
        {
            get { return _textpriceAdjustment; }
            set
            {
                _textpriceAdjustment = value;
                OnPropertyChanged("TextpriceAdjustment");
            }
        }

        //xử lý giao diện lệnh DOWN
        private string _textpriceAdjustmentDown;//entry giá điều kiện
        private double _textSlipDown;//entry biên trượt
        private string _textSetPriceDown;//label giá điều chỉnh

        public string TextSetPriceDown
        {
            get { return _textSetPriceDown; }
            set
            {
                _textSetPriceDown = value;
                OnPropertyChanged("TextSetPriceDown");
            }
        }
        public double TextSlipDown
        {
            get { return _textSlipDown; }
            set
            {
                _textSlipDown = value;
                OnPropertyChanged("TextSlipDown");
            }
        }
        public string TextpriceAdjustmentDown
        {
            get { return _textpriceAdjustmentDown; }
            set
            {
                _textpriceAdjustmentDown = value;
                OnPropertyChanged("TextpriceAdjustmentDown");
            }
        }

        //dieu kien
        private string _entryKhoiLuong;
        private bool _isShowLenh;
        private bool _isConditionUP; 
        private bool _isConditionOCO; 
        private bool _isConditionBB;
        private bool _isConditionDOWN;
        private string _showTextLenh;
        private string _backGroundLenhUP;
        private string _backGroundLenhDOWN; 
        private string _backGroundLenhOCO; 
        private string _backGroundLenhBB; 
        private string _showBBText; 
        private int KhoiLuong;

        public bool IsConditionBB
        {
            get { return _isConditionBB; }
            set
            {
                _isConditionBB = value;
                OnPropertyChanged("IsConditionBB");
            }
        }
        public bool IsConditionOCO
        {
            get { return _isConditionOCO; }
            set
            {
                _isConditionOCO = value;
                OnPropertyChanged("IsConditionOCO");
            }
        }
        public bool IsConditionDOWN
        {
            get { return _isConditionDOWN; }
            set
            {
                _isConditionDOWN = value;
                OnPropertyChanged("IsConditionDOWN");
            }
        }
        public bool IsConditionUP
        {
            get { return _isConditionUP; }
            set
            {
                _isConditionUP = value;
                OnPropertyChanged("IsConditionUP");
            }
        }
        public string ShowBBText
        {
            get { return _showBBText; }
            set
            {
                _showBBText = value;
                OnPropertyChanged("ShowBBText");
            }
        }
        public string BackGroundLenhOCO
        {
            get { return _backGroundLenhOCO; }
            set
            {
                _backGroundLenhOCO = value;
                OnPropertyChanged("BackGroundLenhOCO");
            }
        }
        public string BackGroundLenhBB
        {
            get { return _backGroundLenhBB; }
            set
            {
                _backGroundLenhBB = value;
                OnPropertyChanged("BackGroundLenhBB");
            }
        }
        public string BackGroundLenhDOWN
        {
            get { return _backGroundLenhDOWN; }
            set
            {
                _backGroundLenhDOWN = value;
                OnPropertyChanged("BackGroundLenhDOWN");
            }
        }
        public string BackGroundLenhUP
        {
            get { return _backGroundLenhUP; }
            set
            {
                _backGroundLenhUP = value;
                OnPropertyChanged("BackGroundLenhUP");
            }
        }
        public string ShowTextLenh
        {
            get { return _showTextLenh; }
            set
            {
                _showTextLenh = value;
                OnPropertyChanged("ShowTextLenh");
            }
        }
        public bool IsShowLenh
        {
            get { return _isShowLenh; }
            set
            {
                _isShowLenh = value;
                OnPropertyChanged("IsShowLenh");
            }
        }
        public string EntryKhoiLuong
        {
            get { return _entryKhoiLuong; }
            set
            {
                _entryKhoiLuong = value;
                OnPropertyChanged("EntryKhoiLuong");
            }
        }

        //list mã stock
        public ObservableCollection<StockCode> ListStockCode { get; set; }
        private string _stockShow;//mã 
        private string _imageValueShow;//ảnh giá trị lên suống
        private string _stockValueShow;//giá trị cổ phiếu
        private string _shareRateShow;//tỉ lệ cổ phiếu
        private string _openingPriceShow;//giá mở cửa
        private string _expensiveShow;//giá cao
        private string _lowPriceShow;//giá thấp 
        private string _generalShow;//tổng HĐ
        private string _massShow;//khối lượng mở
        private bool _isShow;

        public bool IsShow
        {
            get { return _isShow; }
            set
            {
                _isShow = value;
                OnPropertyChanged("IsShow");
            }
        }
        public string MassShow
        {
            get { return _massShow; }
            set
            {
                _massShow = value;
                OnPropertyChanged("MassShow");
            }
        }
        public string GeneralShow
        {
            get { return _generalShow; }
            set
            {
                _generalShow = value;
                OnPropertyChanged("GeneralShow");
            }
        }
        public string LowPriceShow
        {
            get { return _lowPriceShow; }
            set
            {
                _lowPriceShow = value;
                OnPropertyChanged("LowPriceShow");
            }
        }
        public string ExpensiveShow
        {
            get { return _expensiveShow; }
            set
            {
                _expensiveShow = value;
                OnPropertyChanged("ExpensiveShow");
            }
        }
        public string OpeningPriceShow
        {
            get { return _openingPriceShow; }
            set
            {
                _openingPriceShow = value;
                OnPropertyChanged("OpeningPriceShow");
            }
        }
        public string ShareRateShow
        {
            get { return _shareRateShow; }
            set
            {
                _shareRateShow = value;
                OnPropertyChanged("ShareRateShow");
            }
        }
        public string StockValueShow
        {
            get { return _stockValueShow; }
            set
            {
                _stockValueShow = value;
                OnPropertyChanged("StockValueShow");
            }
        }
        public string ImageValueShow
        {
            get { return _imageValueShow; }
            set
            {
                _imageValueShow = value;
                OnPropertyChanged("ImageValueShow");
            }
        }
        public string StockShow
        {
            get { return _stockShow; }
            set
            {
                _stockShow = value;
                OnPropertyChanged("StockShow");
            }
        }
        //lable Mua bán
        private int _textSizeBuy;
        private int _textSizeSell;
        private string _colorTextBuy;
        private string _colorTextSell;
        private string _backGroundColorBuy;
        private string _backGroundColorSell;

        public string BackGroundColorSell
        {
            get { return _backGroundColorSell; }
            set
            {
                _backGroundColorSell = value;
                OnPropertyChanged("BackGroundColorSell");
            }
        }
        public string BackGroundColorBuy
        {
            get { return _backGroundColorBuy; }
            set
            {
                _backGroundColorBuy = value;
                OnPropertyChanged("BackGroundColorBuy");
            }
        }
        public string ColorTextSell
        {
            get { return _colorTextSell; }
            set
            {
                _colorTextSell = value;
                OnPropertyChanged("ColorTextSell");
            }
        }
        public string ColorTextBuy
        {
            get { return _colorTextBuy; }
            set
            {
                _colorTextBuy = value;
                OnPropertyChanged("ColorTextBuy");
            }
        }
        public int TextSizeSell
        {
            get { return _textSizeSell; }
            set
            {
                _textSizeSell = value;
                OnPropertyChanged("TextSizeSell");
            }
        }
        public int TextSizeBuy
        {
            get { return _textSizeBuy; }
            set
            {
                _textSizeBuy = value;
                OnPropertyChanged("TextSizeBuy");
            }
        }

        //expander
        private bool _isExpanded;
        private string _imageExpander; 

        public string ImageExpander
        {
            get { return _imageExpander; }
            set
            {
                _imageExpander = value;
                OnPropertyChanged("ImageExpander");
            }
        }
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        //lable lệnh
        private string _colorNormalOrder;
        private string _backGroundColorNormalOrder;
        private string _colorConditionalOrder;
        private string _backGroundColorConditionalOrder;
        private string _colorSampleOrder;
        private string _backGroundColorSampleOrder;

        public string BackGroundColorSampleOrder
        {
            get { return _backGroundColorSampleOrder; }
            set
            {
                _backGroundColorSampleOrder = value;
                OnPropertyChanged("BackGroundColorSampleOrder");
            }
        }
        public string ColorSampleOrder
        {
            get { return _colorSampleOrder; }
            set
            {
                _colorSampleOrder = value;
                OnPropertyChanged("ColorSampleOrder");
            }
        }
        public string BackGroundColorConditionalOrder
        {
            get { return _backGroundColorConditionalOrder; }
            set
            {
                _backGroundColorConditionalOrder = value;
                OnPropertyChanged("BackGroundColorConditionalOrder");
            }
        }
        public string ColorConditionalOrder
        {
            get { return _colorConditionalOrder; }
            set
            {
                _colorConditionalOrder = value;
                OnPropertyChanged("ColorConditionalOrder");
            }
        }
        public string BackGroundColorNormalOrder
        {
            get { return _backGroundColorNormalOrder; }
            set
            {
                _backGroundColorNormalOrder = value;
                OnPropertyChanged("BackGroundColorNormalOrder");
            }
        }
        public string ColorNormalOrder
        {
            get { return _colorNormalOrder; }
            set
            {
                _colorNormalOrder = value;
                OnPropertyChanged("ColorNormalOrder");
            }
        }
        public DatLenhViewModel()
        {
            ListStockCode = new ObservableCollection<StockCode>();
            ColorNormalOrder = "#868992";
            BackGroundColorNormalOrder = "#c9c9c9";
            BackGroundColorConditionalOrder = "#5eb358";
            ColorConditionalOrder = "#5eb358";
            BackGroundColorSampleOrder = "#c9c9c9";
            ColorSampleOrder = "#868992";
            IsExpanded = false;
            ImageExpander = "suong.png";
            BackGroundColorSell = "#f3f3f3";
            ColorTextSell = "Red";
            TextSizeSell = 18;
            BackGroundColorBuy = "Green";
            ColorTextBuy = "Green";
            TextBtnXacNhan = "Xác Nhận Mua";
            IsPassword = true;
            TextSizeBuy = 22;
            IsShow = false;
            IsShowLenh = false;
            ShowBBText = "B&B";
            ShowTextLenh = "Lệnh UP";
            BackGroundLenhUP = "#c3c3c3";
            BackGroundLenhDOWN = "White";
            BackGroundLenhOCO= "White";
            BackGroundLenhBB = "White";
            IsConditionUP = true;
            IsConditionDOWN = false;
            IsConditionOCO = false;
            IsConditionBB = false;
            BtnBackgroundPass = "#b3b3b3";
            IsShowEntryGia = false;
            IsEntryColorGia = true;

            StockShow = "VN30F2103";
            ImageValueShow = "reddown.png";
            StockValueShow = "1,188.8";
            ShareRateShow= "2 (0.2%)";
            OpeningPriceShow= "1,12";
            ExpensiveShow= "1,13";
            LowPriceShow = "1,11";
            GeneralShow= "40,815";
            MassShow = "27,303";

            //chuyển đổi tag
            ClickNormalOrderCommand = new Command<object>(ClickNormalOrder);
            ClickSampleOrderCommand = new Command<object>(ClickSampleOrder);
            ClickConditionalOrderCommand = new Command<object>(ClickConditionalOrder);
            //expander
            ClickImageExpanderCommand = new Command<object>(ClickImageExpander);
            ClickBuyCommand = new Command<object>(ClickBuy);
            ClickSellCommand = new Command<object>(ClickSell);
            //chọn mã chứng khoán 
            ShowStockCommand = new Command<object>(ShowStock);
            StockCodeCommand = new Command<object>(StockCodeshow);
            //xử lý khối lượng
            ClickOverBoughtCommand = new Command<object>(ClickOverBought);
            ClickExcessSaleCommand = new Command<object>(ClickExcessSale);
            ClickMaxCommand = new Command<object>(ClickMax);
            CongEntryKhoiLuongCommand = new Command<object>(CongEntryKhoiLuong);
            TruEntryKhoiLuongCommand = new Command<object>(TruEntryKhoiLuong);
            ShowLenhCommand = new Command<object>(ShowLenh);
            //chọn lênh
            ClickLenhUPCommand = new Command<object>(ClickLenhUP);
            ClickLenhDOWNCommand = new Command<object>(ClickLenhDOWN);
            ClickLenhOCOCommand = new Command<object>(ClickLenhOCO);
            ClickLenhBBCommand = new Command<object>(ClickLenhBB);
            //lệnh UP
            TruGiaDieuKienCommand = new Command<object>(TruGiaDieuKien);
            CongGiaDieuKienCommand = new Command<object>(CongGiaDieuKien);
            TruBientruotCommand = new Command<object>(TruBientruot);
            CongBientruotCommand = new Command<object>(CongBientruot);
            //lệnh down
            TruGiaDieuKienDownCommand = new Command<object>(TruGiaDieuKienDown);
            CongGiaDieuKienDownCommand = new Command<object>(CongGiaDieuKienDown);
            TruBientruotDownCommand = new Command<object>(TruBientruotDown);
            CongBientruotDownCommand = new Command<object>(CongBientruotDown);
            //pass
            ShowPassCommand = new Command<object>(ShowPass);
            //lệnh OCO
            GiaTranCommand = new Command<object>(GiaTran);
            GiaThamChieuCommand = new Command<object>(GiaThamChieu);
            GiaSanCommand = new Command<object>(GiaSan);
            GiamGiaCommand = new Command<object>(GiamGia);
            TangGiaCommand = new Command<object>(TangGia);
            TruGiaDieuKienOCOCommand = new Command<object>(TruGiaDieuKienOCO);
            CongGiaDieuKienOCOCommand = new Command<object>(CongGiaDieuKienOCO);
            TruGiaBientruotOCOCommand = new Command<object>(TruGiaBientruotOCO);
            CongGiaBientruotOCOCommand = new Command<object>(CongGiaBientruotOCO);
            //lệnh BB
            TangKhoangLaiCommand = new Command<object>(TangKhoangLai);
            GiamKhoangLaiCommand = new Command<object>(GiamKhoangLai);
            GiamKhoangLoCommand = new Command<object>(GiamKhoangLo);
            TangKhoangLoCommand = new Command<object>(TangKhoangLo);
            GiamBienTruotBBCommand = new Command<object>(GiamBienTruotBB);
            TangBienTruotBBCommand = new Command<object>(TangBienTruotBB);

            ListStockCode.Add(new StockCode { Show=false ,Stock = "VN30F2103", StockValue = "1,188.8", ShareRate = "2 (0.2%)", ImageValue = "reddown.png", OpeningPrice = "1,12", Expensive = "1,13", LowPrice = "1,11", General = "40,815", Mass = "27,303", OverboughtPriceOne = "1,188.7", OverboughtPriceTwo = "1,262.3", OverboughtPriceFive = "1,848.4", OverboughtPriceFour = "1,151.3", OverboughtPriceSix = "1,152.6", OverboughtPriceThree = "1,125.6", ExcessSellingPriceFive = "9", ExcessSellingPriceFour = "15", ExcessSellingPriceOne = "226", ExcessSellingPriceSix = "12", ExcessSellingPriceThree = "152", ExcessSellingPriceTwo = "16" });
            ListStockCode.Add(new StockCode { Show = false, Stock = "VN30F2104", StockValue = "1,155.1", ShareRate = "0 (0.0%)", ImageValue = "square.png", OpeningPrice = "1,85", Expensive = "1,87", LowPrice = "1.885.2", General = "30,785", Mass = "21.663", OverboughtPriceOne = "1,188.7", OverboughtPriceTwo = "1,262.3", OverboughtPriceFive = "1,848.4", OverboughtPriceFour = "1,151.3", OverboughtPriceSix = "1,152.6", OverboughtPriceThree = "1,125.6", ExcessSellingPriceFive = "9", ExcessSellingPriceFour = "15", ExcessSellingPriceOne = "226", ExcessSellingPriceSix = "12", ExcessSellingPriceThree = "152", ExcessSellingPriceTwo = "16" });
            ListStockCode.Add(new StockCode { Show = false, Stock = "VN30F2105", StockValue = "1,115.5", ShareRate = "1 (0.8%)", ImageValue = "greenup.png", OpeningPrice = "1,191", Expensive = "1,192", LowPrice = "1127.2", General = "20,631", Mass = "15,903", OverboughtPriceOne = "1,188.7", OverboughtPriceTwo = "1,262.3", OverboughtPriceFive = "1,848.4", OverboughtPriceFour = "1,151.3", OverboughtPriceSix = "1,152.6", OverboughtPriceThree = "1,125.6", ExcessSellingPriceFive = "9", ExcessSellingPriceFour = "15", ExcessSellingPriceOne = "226", ExcessSellingPriceSix = "12", ExcessSellingPriceThree = "152", ExcessSellingPriceTwo = "16" });
            ListStockCode.Add(new StockCode { Show = false, Stock = "VN30F2106", StockValue = "1,148.9", ShareRate = "6 (0.3%)", ImageValue = "reddown.png", OpeningPrice = "1,114", Expensive = "1,115", LowPrice = "1645.2", General = "10,215", Mass = "6,303", OverboughtPriceOne = "1,188.7", OverboughtPriceTwo = "1,262.3", OverboughtPriceFive = "1,848.4", OverboughtPriceFour = "1,151.3", OverboughtPriceSix = "1,152.6", OverboughtPriceThree = "1,125.6", ExcessSellingPriceFive = "9", ExcessSellingPriceFour = "15", ExcessSellingPriceOne = "226", ExcessSellingPriceSix = "12", ExcessSellingPriceThree = "152", ExcessSellingPriceTwo = "16" });
        }

        private void TangBienTruotBB(object obj)
        {
            if (!string.IsNullOrEmpty(TextEntryGia) && !string.IsNullOrEmpty(TextKhoangLoBB) && !string.IsNullOrEmpty(TextBienTruotBB))
            {
                GiaCoPhieu = double.Parse(TextGiaDieuKienBB);
                GiaDieuChinh = double.Parse(TextGiaDieuChinhBB);
                GiaDieuKien = double.Parse(TextEntryGia);
                GiaKhoangLo = double.Parse(TextKhoangLoBB);
                GiaKhoangLai = double.Parse(TextBienTruotBB);
                if (GiaCoPhieu - GiaKhoangLai > 1.188)
                {
                    GiaKhoangLai = GiaKhoangLai + 0.001;
                }
                GiaCoPhieu = GiaKhoangLo + GiaDieuKien;
                GiaDieuChinh = GiaCoPhieu - GiaKhoangLai;
                TextBienTruotBB = GiaKhoangLai.ToString();
                TextGiaDieuKienBB = GiaCoPhieu.ToString();
                TextGiaDieuChinhBB = GiaDieuChinh.ToString();
            }
            if (!string.IsNullOrEmpty(TextEntryGia) && string.IsNullOrEmpty(TextGiaDieuKienBB) && string.IsNullOrEmpty(TextKhoangLoBB) && string.IsNullOrEmpty(TextGiaDieuChinhBB) && string.IsNullOrEmpty(TextBienTruotBB))
            {
                TextBienTruotBB = "0";
                GiaKhoangLai = double.Parse(TextBienTruotBB);
                if (GiaCoPhieu - GiaKhoangLai > 1.188)
                {
                    GiaKhoangLai = GiaKhoangLai + 0.001;
                }
                TextBienTruotBB = GiaKhoangLai.ToString();
                
            }
            if (!string.IsNullOrEmpty(TextEntryGia) && !string.IsNullOrEmpty(TextBienTruotBB) && string.IsNullOrEmpty(TextKhoangLoBB))
            {
                TextBienTruotBB = "0";
                GiaKhoangLai = double.Parse(TextBienTruotBB);
                if (GiaCoPhieu - GiaKhoangLai > 1.188)
                {
                    GiaKhoangLai = GiaKhoangLai + 0.001;
                }
                TextBienTruotBB = GiaKhoangLai.ToString();
            }
            if (string.IsNullOrEmpty(TextEntryGia) && !string.IsNullOrEmpty(TextBienTruotBB))
            {
                GiaKhoangLo = double.Parse(TextBienTruotBB);
                GiaKhoangLo = GiaKhoangLo + 0.001;
                TextBienTruotBB = GiaKhoangLo.ToString();
            }
            if (string.IsNullOrEmpty(TextEntryGia) && string.IsNullOrEmpty(TextBienTruotBB))
            {
                TextBienTruotBB = "0";
                GiaKhoangLo = double.Parse(TextBienTruotBB);
                GiaKhoangLai = GiaKhoangLai + 0.001;
                TextBienTruotBB = GiaKhoangLai.ToString();
            }
            
        }

        private void GiamBienTruotBB(object obj)
        {
            if (!string.IsNullOrEmpty(TextEntryGia) && !string.IsNullOrEmpty(TextKhoangLoBB) && !string.IsNullOrEmpty(TextBienTruotBB))
            {
                GiaCoPhieu = double.Parse(TextGiaDieuKienBB);
                GiaDieuChinh = double.Parse(TextGiaDieuChinhBB);
                GiaDieuKien = double.Parse(TextEntryGia);
                GiaKhoangLo = double.Parse(TextKhoangLoBB);
                GiaKhoangLai = double.Parse(TextBienTruotBB);
                if (GiaKhoangLai > 0)
                {
                    GiaKhoangLai = GiaKhoangLai - 0.001;
                }
                GiaCoPhieu = GiaKhoangLo + GiaDieuKien;
                GiaDieuChinh = GiaCoPhieu - GiaKhoangLai;
                TextBienTruotBB = GiaKhoangLai.ToString();
                TextGiaDieuKienBB = GiaCoPhieu.ToString();
                TextGiaDieuChinhBB = GiaDieuChinh.ToString();
            }
            if (string.IsNullOrEmpty(TextEntryGia) && !string.IsNullOrEmpty(TextBienTruotBB))
            {
                GiaKhoangLo = double.Parse(TextBienTruotBB);
                if (GiaKhoangLo > 0)
                {
                    GiaKhoangLo = GiaKhoangLo - 0.001;
                    TextBienTruotBB = GiaKhoangLo.ToString();
                }
            }
        }

        private void TangKhoangLo(object obj)
        {
            if (!string.IsNullOrEmpty(TextEntryGia)  && !string.IsNullOrEmpty(TextKhoangLoBB) && !string.IsNullOrEmpty(TextBienTruotBB))
            {
                TextGiaDieuKienBB = "0";
                TextGiaDieuChinhBB = "0";
                GiaCoPhieu = double.Parse(TextGiaDieuKienBB);
                GiaDieuChinh = double.Parse(TextGiaDieuChinhBB);
                GiaDieuKien = double.Parse(TextEntryGia);
                GiaKhoangLo = double.Parse(TextKhoangLoBB);
                GiaKhoangLai = double.Parse(TextBienTruotBB);
                if (GiaKhoangLo + GiaDieuKien < 1.272)
                {
                    GiaKhoangLo = GiaKhoangLo + 0.001;
                }
                GiaCoPhieu = GiaKhoangLo + GiaDieuKien;
                GiaDieuChinh = GiaCoPhieu - GiaKhoangLai;
                TextKhoangLoBB = GiaKhoangLo.ToString();
                TextGiaDieuKienBB = GiaCoPhieu.ToString();
                TextGiaDieuChinhBB = GiaDieuChinh.ToString();
            }
            if (!string.IsNullOrEmpty(TextEntryGia) && string.IsNullOrEmpty(TextKhoangLoBB) )
            {
                TextGiaDieuKienBB = "0";
                TextGiaDieuChinhBB = "0";
                TextBienTruotBB = "0";
                TextKhoangLoBB = "0";
                GiaCoPhieu = double.Parse(TextGiaDieuKienBB);
                GiaDieuChinh = double.Parse(TextGiaDieuChinhBB);
                GiaDieuKien = double.Parse(TextEntryGia);
                GiaKhoangLo = double.Parse(TextKhoangLoBB);
                GiaKhoangLai = double.Parse(TextBienTruotBB);
                if (GiaKhoangLo + GiaDieuKien < 1.272)
                {
                    GiaKhoangLo = GiaKhoangLo + 0.001;
                }
                GiaCoPhieu = GiaKhoangLo + GiaDieuKien;
                GiaDieuChinh = GiaCoPhieu - GiaKhoangLai;
                TextKhoangLoBB = GiaKhoangLo.ToString();
                TextGiaDieuKienBB = GiaCoPhieu.ToString();
                TextGiaDieuChinhBB = GiaDieuChinh.ToString();
            }
            if (!string.IsNullOrEmpty(TextEntryGia) && !string.IsNullOrEmpty(TextKhoangLoBB) && string.IsNullOrEmpty(TextBienTruotBB))
            {
                TextGiaDieuKienBB = "0";
                TextGiaDieuChinhBB = "0";
                TextBienTruotBB = "0";
                GiaCoPhieu = double.Parse(TextGiaDieuKienBB);
                GiaDieuChinh = double.Parse(TextGiaDieuChinhBB);
                GiaDieuKien = double.Parse(TextEntryGia);
                GiaKhoangLo = double.Parse(TextKhoangLoBB);
                GiaKhoangLai = double.Parse(TextBienTruotBB);
                if (GiaKhoangLo + GiaDieuKien < 1.272)
                {
                    GiaKhoangLo = GiaKhoangLo + 0.001;
                }
                GiaCoPhieu = GiaKhoangLo + GiaDieuKien;
                GiaDieuChinh = GiaCoPhieu - GiaKhoangLai;
                TextKhoangLoBB = GiaKhoangLo.ToString();
                TextGiaDieuKienBB = GiaCoPhieu.ToString();
                TextGiaDieuChinhBB = GiaDieuChinh.ToString();
            }
            if (string.IsNullOrEmpty(TextEntryGia) && !string.IsNullOrEmpty(TextKhoangLoBB))
            {
                GiaKhoangLo = double.Parse(TextKhoangLoBB);
                GiaKhoangLo = GiaKhoangLo + 0.001;
                TextKhoangLoBB = GiaKhoangLo.ToString();
            }
            if (string.IsNullOrEmpty(TextEntryGia) && string.IsNullOrEmpty(TextKhoangLoBB))
            {
                TextBienTruotBB = "0";
                GiaKhoangLo = double.Parse(TextBienTruotBB);
                if (GiaKhoangLo + GiaDieuKien < 1.272)
                {
                    GiaKhoangLo = GiaKhoangLo + 0.001;
                }
                TextKhoangLoBB = GiaKhoangLo.ToString();
            }
        }

        private void GiamKhoangLo(object obj)
        {
            if (!string.IsNullOrEmpty(TextEntryGia) && !string.IsNullOrEmpty(TextGiaDieuKienBB) && !string.IsNullOrEmpty(TextKhoangLoBB) && !string.IsNullOrEmpty(TextGiaDieuChinhBB) && !string.IsNullOrEmpty(TextBienTruotBB))
            {
                GiaCoPhieu = double.Parse(TextGiaDieuKienBB);
                GiaDieuChinh = double.Parse(TextGiaDieuChinhBB);
                GiaDieuKien = double.Parse(TextEntryGia);
                GiaKhoangLo = double.Parse(TextKhoangLoBB);
                GiaKhoangLai = double.Parse(TextBienTruotBB);
                if (GiaKhoangLo > 0)
                {
                    GiaKhoangLo = GiaKhoangLo - 0.001;
                }
                GiaCoPhieu = GiaKhoangLo + GiaDieuKien;
                GiaDieuChinh = GiaCoPhieu - GiaKhoangLai;
                TextKhoangLoBB = GiaKhoangLo.ToString();
                TextGiaDieuKienBB = GiaCoPhieu.ToString();
                TextGiaDieuChinhBB = GiaDieuChinh.ToString();
            }
            
            if (string.IsNullOrEmpty(TextEntryGia) && !string.IsNullOrEmpty(TextKhoangLoBB))
            {
                GiaKhoangLo = double.Parse(TextKhoangLoBB);
                if (GiaKhoangLo > 0)
                {
                    GiaKhoangLo = GiaKhoangLo - 0.001;
                    TextKhoangLoBB = GiaKhoangLo.ToString();
                }
            }
            if(string.IsNullOrEmpty(TextKhoangLoBB) || TextKhoangLoBB == "0")
            {
                TextKhoangLoBB = "0";
                TextGiaDieuKienBB = "";
                TextGiaDieuChinhBB = "";
            }
        }

        private void GiamKhoangLai(object obj)
        {
            if (!string.IsNullOrEmpty(TextEntryGia) && !string.IsNullOrEmpty(TextGiaChotLaiBB) && !string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                GiaCoPhieu = double.Parse(TextGiaChotLaiBB);
                GiaDieuChinh = double.Parse(TextEntryGia);
                GiaDieuKien = double.Parse(TextKhoangLaiBB);
                if (GiaDieuKien > 0)
                {
                    GiaDieuKien = GiaDieuKien - 0.001;
                }
                GiaCoPhieu = GiaDieuChinh + GiaDieuKien;
                TextKhoangLaiBB = GiaDieuKien.ToString();
                TextGiaChotLaiBB = GiaCoPhieu.ToString();
            }
            if (!string.IsNullOrEmpty(TextEntryGia) && string.IsNullOrEmpty(TextGiaChotLaiBB) && string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                GiaDieuChinh = double.Parse(TextEntryGia);
                TextKhoangLaiBB = "0";
                GiaDieuKien = double.Parse(TextKhoangLaiBB);
                if (GiaDieuKien > 0)
                {
                    GiaDieuKien = GiaDieuKien - 0.001;
                    TextKhoangLaiBB = GiaDieuKien.ToString();
                }
                GiaCoPhieu = GiaDieuChinh + GiaDieuKien;
                TextGiaChotLaiBB = GiaCoPhieu.ToString();
            }
            if (string.IsNullOrEmpty(TextEntryGia) && string.IsNullOrEmpty(TextGiaChotLaiBB) && string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                TextKhoangLaiBB = "";
            }
            if (!string.IsNullOrEmpty(TextKhoangLaiBB) && string.IsNullOrEmpty(TextEntryGia) && string.IsNullOrEmpty(TextGiaChotLaiBB))
            {
                GiaDieuKien = double.Parse(TextKhoangLaiBB);
                if (GiaDieuKien > 0)
                {
                    GiaDieuKien = GiaDieuKien - 0.001;
                    TextKhoangLaiBB = GiaDieuKien.ToString();
                }
            }
            if (TextKhoangLaiBB == "0" && string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                TextGiaChotLaiBB = "";
                TextKhoangLaiBB = "";
            }
        }

        private void TangKhoangLai(object obj)
        {
            if (!string.IsNullOrEmpty(TextEntryGia) && !string.IsNullOrEmpty(TextGiaChotLaiBB) && !string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                GiaCoPhieu = double.Parse(TextGiaChotLaiBB);
                GiaDieuChinh = double.Parse(TextEntryGia);
                GiaDieuKien = double.Parse(TextKhoangLaiBB);
                GiaDieuKien = GiaDieuKien + 0.001;
                GiaCoPhieu = GiaDieuChinh + GiaDieuKien;
                TextKhoangLaiBB = GiaDieuKien.ToString();
                TextGiaChotLaiBB = GiaCoPhieu.ToString();
            }
            if (!string.IsNullOrEmpty(TextEntryGia) && string.IsNullOrEmpty(TextGiaChotLaiBB) && string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                GiaDieuChinh = double.Parse(TextEntryGia);
                TextKhoangLaiBB = "0";
                GiaDieuKien = double.Parse(TextKhoangLaiBB);
                GiaDieuKien = GiaDieuKien + 0.001;
                GiaCoPhieu = GiaDieuChinh + GiaDieuKien;
                TextKhoangLaiBB = GiaDieuKien.ToString();
                TextGiaChotLaiBB = GiaCoPhieu.ToString();
            }
            if (string.IsNullOrEmpty(TextEntryGia) && string.IsNullOrEmpty(TextGiaChotLaiBB) && string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                TextKhoangLaiBB = "0";
                GiaDieuKien = double.Parse(TextKhoangLaiBB);
                GiaDieuKien = GiaDieuKien + 0.001;
                TextKhoangLaiBB = GiaDieuKien.ToString();
            }
        }

        private void CongGiaBientruotOCO(object obj)
        {
            if (!string.IsNullOrEmpty(TextpriceAdjustmentOCO))
            {
                GiaDieuChinh = double.Parse(TextSetPriceOCO);
                GiaDieuKien = double.Parse(TextpriceAdjustmentOCO);
                TextSlipOCO = TextSlipOCO + 0.001;
                GiaDieuChinh = TextSlipOCO + GiaDieuKien;
                TextSetPriceOCO = GiaDieuChinh.ToString();
            }
            else
                TextSlipOCO = TextSlipOCO + 0.001;
        }

        private void TruGiaBientruotOCO(object obj)
        {
            if (!string.IsNullOrEmpty(TextpriceAdjustmentOCO))
            {
                if (TextSlipOCO > 0)
                {
                    GiaDieuChinh = double.Parse(TextSetPriceOCO);
                    GiaDieuKien = double.Parse(TextpriceAdjustmentOCO);
                    TextSlipOCO = TextSlipOCO - 0.001;
                    GiaDieuChinh = TextSlipOCO + GiaDieuKien;
                    TextSetPriceOCO = GiaDieuChinh.ToString();
                }
            }
            else
            {
                if (TextSlipOCO > 0)
                {
                    TextSlipOCO = TextSlipOCO - 0.001;
                }
            }
        }

        private void CongGiaDieuKienOCO(object obj)
        {
            if (!string.IsNullOrEmpty(TextpriceAdjustmentOCO))
            {
                GiaDieuKien = double.Parse(TextpriceAdjustmentOCO);
                if (GiaDieuKien >= 1.188 && GiaDieuKien < 1.272)
                {
                    GiaDieuKien = GiaDieuKien + 0.001;
                    GiaDieuChinh = double.Parse(TextSetPriceOCO);
                    GiaDieuChinh = TextSlipOCO + GiaDieuKien;
                    TextpriceAdjustmentOCO = GiaDieuKien.ToString();
                    TextSetPriceOCO = GiaDieuChinh.ToString();
                }
            }
            else
            {
                TextSetPriceOCO = "0";
                TextpriceAdjustmentOCO = "1.188";
                GiaDieuChinh = double.Parse(TextSetPriceOCO);
                GiaDieuKien = double.Parse(TextpriceAdjustmentOCO);
                GiaDieuChinh = TextSlipOCO + GiaDieuKien;
                TextSetPriceOCO = GiaDieuChinh.ToString();
            }
        }

        private void TruGiaDieuKienOCO(object obj)
        {
            if (!string.IsNullOrEmpty(TextpriceAdjustmentOCO))
            {
                GiaDieuKien = double.Parse(TextpriceAdjustmentOCO);
                if (GiaDieuKien > 1.188 && GiaDieuKien <= 1.272)
                {
                    GiaDieuKien = GiaDieuKien - 0.001;
                    GiaDieuChinh = double.Parse(TextSetPriceOCO);
                    GiaDieuChinh = GiaDieuKien + TextSlipOCO;
                    TextpriceAdjustmentOCO = GiaDieuKien.ToString();
                    TextSetPriceOCO = GiaDieuChinh.ToString();
                }
            }
            else
            {
                TextSetPriceOCO = "0";
                TextpriceAdjustmentOCO = "1.272";
                GiaDieuChinh = double.Parse(TextSetPriceOCO);
                GiaDieuKien = double.Parse(TextpriceAdjustmentOCO);
                GiaDieuChinh = GiaDieuKien + TextSlipOCO;
                TextSetPriceOCO = GiaDieuChinh.ToString();
            }
        }

        private void TangGia(object obj)
        {
            if (!string.IsNullOrEmpty(TextEntryGia))
            {
                GiaCoPhieu = double.Parse(TextEntryGia);
                if (GiaCoPhieu > 1.188 && GiaCoPhieu < 1.272)
                {
                    GiaCoPhieu = GiaCoPhieu + 0.001;
                    TextEntryGia = GiaCoPhieu.ToString();
                    TextGiaChotLai = TextEntryGia;
                }
            }
            else
            {
                TextEntryGia = "1.188";
                TextGiaChotLai = TextEntryGia;
            }
        }

        private void GiamGia(object obj)
        {
            if (!string.IsNullOrEmpty(TextEntryGia))
            {
                GiaCoPhieu = double.Parse(TextEntryGia);
                if (GiaCoPhieu > 1.188 && GiaCoPhieu <= 1.272)
                {
                    GiaCoPhieu = GiaCoPhieu - 0.001;
                    TextEntryGia = GiaCoPhieu.ToString();
                    TextGiaChotLai = TextEntryGia;
                }
            }
            else
            {
                TextEntryGia = "1.272";
                TextGiaChotLai = TextEntryGia;
            }
        }

        private void GiaSan(object obj)
        {
            if (IsConditionOCO == true)
            {
                TextEntryGia = "1.188";
                TextGiaChotLai = "1.188";
            }
            if (IsConditionBB == true && !string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                TextEntryGia = "1.188";
                GiaDieuKien = double.Parse(TextEntryGia);
                GiaDieuChinh = double.Parse(TextKhoangLaiBB);
                if ((GiaDieuKien + GiaDieuChinh) < 1.272)
                {
                    TextGiaChotLaiBB = (GiaDieuKien + GiaDieuChinh).ToString();
                }
                if ((GiaDieuKien + GiaDieuChinh) > 1.272)
                {
                    TextEntryGia = "";
                }
            }
            if (IsConditionBB == true && string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                TextEntryGia = "1.188";
            }
        }

        private void GiaThamChieu(object obj)
        {
            if (IsConditionOCO == true)
            {
                TextEntryGia = "1.192";
                TextGiaChotLai = "1.192";
            }
            if (IsConditionBB == true && !string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                TextEntryGia = "1.192";
                GiaDieuKien = double.Parse(TextEntryGia);
                GiaDieuChinh = double.Parse(TextKhoangLaiBB);
                if((GiaDieuKien + GiaDieuChinh) < 1.272)
                {
                    TextGiaChotLaiBB = (GiaDieuKien + GiaDieuChinh).ToString();
                }
                if ((GiaDieuKien + GiaDieuChinh) > 1.272)
                {
                    TextEntryGia = "";
                }
            }
            if (IsConditionBB == true && string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                TextEntryGia = "1.192";
            }
        }

        private void GiaTran(object obj)
        {
            if (IsConditionOCO == true)
            {
                TextEntryGia = "1.272";
                TextGiaChotLai = "1.272";
            }
            if (IsConditionBB == true && !string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                TextEntryGia = "1.272";
                GiaDieuKien = double.Parse(TextEntryGia);
                GiaDieuChinh = double.Parse(TextKhoangLaiBB);
                if ((GiaDieuKien + GiaDieuChinh) < 1.272)
                {
                    TextGiaChotLaiBB = (GiaDieuKien + GiaDieuChinh).ToString();
                }
                if ((GiaDieuKien + GiaDieuChinh) > 1.272)
                {
                    TextEntryGia = "";
                }
            }
            if (IsConditionBB == true && string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                TextEntryGia = "1.272";
            }
        }

        private void CongBientruotDown(object obj)
        {
            if (!string.IsNullOrEmpty(TextpriceAdjustmentDown) && !string.IsNullOrEmpty(TextSetPriceDown))
            {
                GiaDieuChinh = double.Parse(TextSetPriceDown);
                GiaDieuKien = double.Parse(TextpriceAdjustmentDown);
                TextSlipDown = TextSlipDown + 0.001;
                GiaDieuChinh =  GiaDieuKien- TextSlipDown ;
                TextSetPriceDown = GiaDieuChinh.ToString();
            }
            else
                TextSlipDown = TextSlipDown + 0.001;
        }

        private void TruBientruotDown(object obj)
        {
            if (!string.IsNullOrEmpty(TextpriceAdjustmentDown) && !string.IsNullOrEmpty(TextSetPriceDown))
            {
                if (TextSlipDown > 0)
                {
                    GiaDieuChinh = double.Parse(TextSetPriceDown);
                    GiaDieuKien = double.Parse(TextpriceAdjustmentDown);
                    TextSlipDown = TextSlipDown - 0.001;
                    GiaDieuChinh =  GiaDieuKien-TextSlipDown;
                    TextSetPriceDown = GiaDieuChinh.ToString();
                }
            }
            else
            {
                if (TextSlipDown > 0)
                {
                    TextSlipDown = TextSlipDown - 0.001;
                }
            }
        }

        private void CongGiaDieuKienDown(object obj)
        {
            if (!string.IsNullOrEmpty(TextpriceAdjustmentDown))
            {
                GiaDieuKien = double.Parse(TextpriceAdjustmentDown);
                if (GiaDieuKien >= 1.188 && GiaDieuKien < 1.272)
                {
                    GiaDieuKien = GiaDieuKien + 0.001;
                    GiaDieuChinh = double.Parse(TextSetPriceDown);
                    GiaDieuChinh =  GiaDieuKien - TextSlipDown ;
                    TextpriceAdjustmentDown = GiaDieuKien.ToString();
                    TextSetPriceDown = GiaDieuChinh.ToString();
                }
            }
            else
            {
                TextSetPriceDown = "0";
                TextpriceAdjustmentDown = "1.188";
                GiaDieuChinh = double.Parse(TextSetPriceDown);
                GiaDieuKien = double.Parse(TextpriceAdjustmentDown);
                GiaDieuChinh =   GiaDieuKien - TextSlipDown;
                TextSetPriceDown = GiaDieuChinh.ToString();
            }
        }

        private void TruGiaDieuKienDown(object obj)
        {
            if(!string.IsNullOrEmpty(TextpriceAdjustmentDown))
            {
                GiaDieuKien = double.Parse(TextpriceAdjustmentDown);
                if (GiaDieuKien > 1.188 && GiaDieuKien <= 1.272)
                {
                    GiaDieuKien = GiaDieuKien - 0.001;
                    GiaDieuChinh = double.Parse(TextSetPriceDown);
                    GiaDieuChinh = GiaDieuKien- TextSlipDown;
                    TextpriceAdjustmentDown = GiaDieuKien.ToString();
                    TextSetPriceDown = GiaDieuChinh.ToString();
                }
            }
            else
            {
                TextSetPriceDown = "0";
                TextpriceAdjustmentDown = "1.272";
                GiaDieuChinh = double.Parse(TextSetPriceDown);
                GiaDieuKien = double.Parse(TextpriceAdjustmentDown);
                GiaDieuChinh =  GiaDieuKien - TextSlipDown;
                TextSetPriceDown = GiaDieuChinh.ToString();
            }
        }

        private void ShowPass(object obj)
        {
            IsPassword = !IsPassword;
        }

        private void CongBientruot(object obj)
        {
            if (TextpriceAdjustment != "" && TextpriceAdjustment != null && TextSetPrice != "" && TextSetPrice != null)
            {
                GiaDieuChinh = double.Parse(TextSetPrice);
                GiaDieuKien = double.Parse(TextpriceAdjustment);
                TextSlip = TextSlip + 0.001;
                GiaDieuChinh = TextSlip + GiaDieuKien;
                TextSetPrice = GiaDieuChinh.ToString();
            }
            else
                TextSlip = TextSlip + 0.001;
        }

        private void TruBientruot(object obj)
        {
            if (TextpriceAdjustment != "" && TextpriceAdjustment != null && TextSetPrice != "" && TextSetPrice != null)
            {
                if (TextSlip > 0)
                {
                    GiaDieuChinh = double.Parse(TextSetPrice);
                    GiaDieuKien = double.Parse(TextpriceAdjustment);
                    TextSlip = TextSlip - 0.001;
                    GiaDieuChinh = TextSlip + GiaDieuKien;
                    TextSetPrice = GiaDieuChinh.ToString();
                }
            }
            else
            {
                if (TextSlip > 0)
                {
                    TextSlip = TextSlip - 0.001;
                }
            }   
        }

        private void CongGiaDieuKien(object obj)
        {
            if (TextpriceAdjustment != "" && TextpriceAdjustment != null)
            {
                GiaDieuKien = double.Parse(TextpriceAdjustment);
                if (GiaDieuKien >= 1.188 && GiaDieuKien < 1.272)
                {
                    GiaDieuKien = GiaDieuKien + 0.001;
                    GiaDieuChinh = double.Parse(TextSetPrice);
                    GiaDieuChinh = TextSlip + GiaDieuKien;
                    TextpriceAdjustment = GiaDieuKien.ToString();
                    TextSetPrice = GiaDieuChinh.ToString();
                }
            }
            else
            {
                TextSetPrice = "0";
                TextpriceAdjustment = "1.188";
                GiaDieuChinh = double.Parse(TextSetPrice);
                GiaDieuKien = double.Parse(TextpriceAdjustment);
                GiaDieuChinh = TextSlip + GiaDieuKien;
                TextSetPrice = GiaDieuChinh.ToString();
            }
        }

        private void TruGiaDieuKien(object obj)
        {
            if (TextpriceAdjustment != "" && TextpriceAdjustment != null)
            {
                GiaDieuKien = double.Parse(TextpriceAdjustment);
                if (GiaDieuKien > 1.188 && GiaDieuKien <= 1.272)
                {
                    GiaDieuKien = GiaDieuKien - 0.001;
                    GiaDieuChinh = double.Parse(TextSetPrice);
                    GiaDieuChinh = TextSlip + GiaDieuKien;
                    TextpriceAdjustment = GiaDieuKien.ToString();
                    TextSetPrice = GiaDieuChinh.ToString();
                }
            }
            else
            {
                TextSetPrice = "0";
                TextpriceAdjustment = "1.272";
                GiaDieuChinh = double.Parse(TextSetPrice);
                GiaDieuKien = double.Parse(TextpriceAdjustment);
                GiaDieuChinh = TextSlip + GiaDieuKien;
                TextSetPrice = GiaDieuChinh.ToString();
            }
        }

        private void ClickLenhBB(object obj)
        {
            ShowTextLenh = "Lệnh B&B";
            BackGroundLenhUP = "White";
            BackGroundLenhDOWN = "White";
            BackGroundLenhOCO = "White";
            BackGroundLenhBB = "#c3c3c3";
            IsConditionUP = false;
            IsConditionDOWN = false;
            IsConditionOCO = false;
            IsConditionBB = true;
            IsShowEntryGia = true;
            IsEntryColorGia = false;
            TextEntryGia = "";
            BtnBackgroundPass = "#b3b3b3";
        }

        private void ClickLenhOCO(object obj)
        {
            ShowTextLenh = "Lệnh OCO";
            BackGroundLenhUP = "White";
            BackGroundLenhDOWN = "White";
            BackGroundLenhOCO = "#c3c3c3";
            BackGroundLenhBB = "White";
            IsConditionUP = false;
            IsConditionDOWN = false;
            IsConditionOCO = true;
            IsConditionBB = false;
            IsShowEntryGia = true;
            IsEntryColorGia = false;
            TextEntryGia = "";
            TextSetPriceOCO = "";
            TextSlipOCO = 0;
            TextpriceAdjustmentOCO = "";
            BtnBackgroundPass = "#b3b3b3";
        }

        private void ClickLenhDOWN(object obj)
        {
            ShowTextLenh = "Lệnh DOWN";
            BackGroundLenhUP = "White";
            BackGroundLenhDOWN = "#c3c3c3";
            BackGroundLenhOCO = "White";
            BackGroundLenhBB = "White";
            IsConditionUP = false;
            IsConditionDOWN = true;
            IsConditionOCO = false;
            IsConditionBB = false;
            IsShowEntryGia = false;
            IsEntryColorGia = true;
            TextSetPriceDown = "";
            TextSlipDown = 0;
            TextpriceAdjustmentDown = "";
            BtnBackgroundPass = "#b3b3b3";
        }

        private void ClickLenhUP(object obj)
        {
            ShowTextLenh = "Lệnh UP";
            BackGroundLenhUP = "#c3c3c3";
            BackGroundLenhDOWN = "White";
            BackGroundLenhOCO = "White";
            BackGroundLenhBB = "White";
            IsConditionUP = true;
            IsConditionDOWN = false;
            IsConditionOCO = false;
            IsConditionBB = false;
            IsShowEntryGia = false;
            IsEntryColorGia = true;
            TextSetPrice = "";
            TextSlip = 0;
            TextpriceAdjustment = "";
            BtnBackgroundPass = "#b3b3b3";
        }

        private void ShowLenh(object obj)
        {
            IsShowLenh = !IsShowLenh;
        }

        private void TruEntryKhoiLuong(object obj)
        {
            if (EntryKhoiLuong != "" && EntryKhoiLuong != null)
            {
                KhoiLuong = int.Parse(EntryKhoiLuong);
                if (KhoiLuong > 0)
                {
                    KhoiLuong--;
                }
                EntryKhoiLuong = KhoiLuong.ToString();
            }
        }

        private void CongEntryKhoiLuong(object obj)
        {
            if(EntryKhoiLuong != "" && EntryKhoiLuong != null)
            {
                KhoiLuong = int.Parse(EntryKhoiLuong);
                if (KhoiLuong < 999)
                {
                    KhoiLuong++;
                }
                EntryKhoiLuong = KhoiLuong.ToString();
            }
        }

        private void ClickMax(object obj)
        {
            EntryKhoiLuong = "0";

        }

        private void ClickExcessSale(object obj)
        {
            if (IsConditionUP == true && TextSizeSell == 22)
            {
                TextSetPrice = "";
                TextSlip = 0;
                TextpriceAdjustment = "";
                BtnBackgroundPass = "#b3b3b3";
            }
            if (IsConditionDOWN == true && TextSizeSell == 22)
            {
                TextSetPriceDown = "";
                TextSlipDown = 0;
                TextpriceAdjustmentDown = "";
                BtnBackgroundPass = "#b3b3b3";
            }
            if (IsConditionOCO == true && TextSizeSell == 22)
            {
                TextEntryGia = "1.191";
                TextGiaChotLai = "1.191";
            }
            if (IsConditionBB == true && !string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                TextEntryGia = "1.191";
                GiaDieuKien = double.Parse(TextEntryGia);
                GiaDieuChinh = double.Parse(TextKhoangLaiBB);
                TextGiaChotLaiBB = (GiaDieuKien + GiaDieuChinh).ToString();
            }
            if (IsConditionBB == true && string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                TextEntryGia = "1.191";
            }
            BackGroundColorSell = "#f3f3f3";
            ColorTextSell = "Red";
            TextSizeSell = 18;
            BackGroundColorBuy = "Green";
            ColorTextBuy = "Green";
            TextSizeBuy = 22;
            TextBtnXacNhan = "Xác Nhận Mua";
        }

        private void ClickOverBought(object obj)
        {
            if (IsConditionUP == true && TextSizeBuy == 22)
            {
                TextSetPrice = "";
                TextSlip = 0;
                TextpriceAdjustment = "";
                BtnBackgroundPass = "#b3b3b3";
            }
            if (IsConditionDOWN == true && TextSizeBuy == 22)
            {
                TextSetPriceDown = "";
                TextSlipDown = 0;
                TextpriceAdjustmentDown = "";
                BtnBackgroundPass = "#b3b3b3";
            }
            if (IsConditionOCO == true )
            {
                TextEntryGia = "1.191";
                TextGiaChotLai = "1.191";
            }
            if (IsConditionBB == true && !string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                TextEntryGia = "1.191";
                GiaDieuKien = double.Parse(TextEntryGia);
                GiaDieuChinh = double.Parse(TextKhoangLaiBB);
                TextGiaChotLaiBB = (GiaDieuKien + GiaDieuChinh).ToString();
            }
            if (IsConditionBB == true && string.IsNullOrEmpty(TextKhoangLaiBB))
            {
                TextEntryGia = "1.191";
            }

            BackGroundColorSell = "Red";
            ColorTextSell = "Red";
            TextSizeSell = 22;
            BackGroundColorBuy = "#c9c9c9";
            ColorTextBuy = "Green";
            TextSizeBuy = 18;
            TextBtnXacNhan = "Xác Nhận Bán";
        }

        private void StockCodeshow(object obj)
        {
            IsShow = false;
            var mn = obj as StockCode;
            mn.Show = true;
            foreach(var iteam in ListStockCode)
            {
                if (iteam.Show == true)
                {
                    iteam.Show = false;
                    StockShow = iteam.Stock;
                    ImageValueShow = iteam.ImageValue;
                    StockValueShow = iteam.StockValue;
                    ShareRateShow = iteam.ShareRate;
                    OpeningPriceShow =iteam.OpeningPrice;
                    ExpensiveShow = iteam.Expensive;
                    LowPriceShow = iteam.LowPrice;
                    GeneralShow = iteam.General;
                    MassShow = iteam.Mass;
                    break;
                }
            }
        }

        private void ShowStock(object obj)
        {
            IsShow = !IsShow;
        }

        private void ClickSell(object obj)
        {
            if (IsConditionUP == true && TextSizeBuy == 22)
            {
                TextSetPrice = "";
                TextSlip = 0;
                TextpriceAdjustment = "";
                BtnBackgroundPass = "#b3b3b3";
            }
            if (IsConditionDOWN == true && TextSizeBuy == 22)
            {
                TextSetPriceDown = "";
                TextSlipDown = 0;
                TextpriceAdjustmentDown = "";
                BtnBackgroundPass = "#b3b3b3";
            }
            if (IsConditionOCO == true && TextSizeBuy == 22)
            {
                TextSetPriceOCO = "";
                TextSlipOCO = 0;
                TextpriceAdjustmentOCO = "";
                BtnBackgroundPass = "#b3b3b3";
            }
            BackGroundColorSell = "Red";
            ColorTextSell = "Red";
            TextSizeSell = 22;
            BackGroundColorBuy = "#c9c9c9";
            ColorTextBuy = "Green";
            TextSizeBuy = 18;
            TextBtnXacNhan = "Xác Nhận Bán";
        }

        private void ClickBuy(object obj)
        {
            if (IsConditionUP == true && TextSizeSell == 22)
            {
                TextSetPrice = "";
                TextSlip = 0;
                TextpriceAdjustment = "";
                BtnBackgroundPass = "#b3b3b3";
            }
            if (IsConditionDOWN == true && TextSizeSell == 22)
            {
                TextSetPriceDown = "";
                TextSlipDown = 0;
                TextpriceAdjustmentDown = "";
                BtnBackgroundPass = "#b3b3b3";
            }
            if (IsConditionOCO == true && TextSizeSell == 22)
            {
                TextSetPriceOCO = "";
                TextSlipOCO = 0;
                TextpriceAdjustmentOCO = "";
                BtnBackgroundPass = "#b3b3b3";
            }
            BackGroundColorSell = "#f3f3f3";
            ColorTextSell = "Red";
            TextSizeSell = 18;
            BackGroundColorBuy = "Green";
            ColorTextBuy = "Green";
            TextSizeBuy = 22;
            TextBtnXacNhan = "Xác Nhận Mua";
            
    
        }

        private void ClickImageExpander(object obj)
        {
            IsExpanded = !IsExpanded;
            if(IsExpanded == false)
            {
                ImageExpander = "suong.png";
            }
            else
                ImageExpander = "len.png";
        }

        private void ClickConditionalOrder(object obj)
        {
            ColorNormalOrder = "#868992";
            BackGroundColorNormalOrder = "#c9c9c9";
            BackGroundColorConditionalOrder = "#5eb358";
            ColorConditionalOrder = "#5eb358";
            BackGroundColorSampleOrder = "#c9c9c9";
            ColorSampleOrder = "#868992";
        }

        private void ClickSampleOrder(object obj)
        {
            ColorNormalOrder = "#868992";
            BackGroundColorNormalOrder = "#c9c9c9";
            BackGroundColorConditionalOrder = "#c9c9c9";
            ColorConditionalOrder = "#868992";
            BackGroundColorSampleOrder = "#5eb358";
            ColorSampleOrder = "#5eb358";
        }

        private void ClickNormalOrder(object obj)
        {
            
            BackGroundColorNormalOrder = "#5eb358";
            BackGroundColorConditionalOrder = "#c9c9c9";
            ColorConditionalOrder = "#868992";
            BackGroundColorSampleOrder = "#c9c9c9";
            ColorSampleOrder = "#868992";
            ColorNormalOrder = "#5eb358";
        }
    }
}
