using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AUTOHLT.MOBILE.Views.FakeUpApp.Model
{
     public class StockCode : INotifyPropertyChanged
     {
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            private string _stock;//mã 
            private bool _isShow;// hiển thị mã
            private string _imageValue;//ảnh giá trị lên suống
            private string _stockValue;//giá trị cổ phiếu
            private string _shareRate;//tỉ lệ cổ phiếu
            private string _openingPrice;//giá mở cửa
            private string _expensive;//giá cao
            private string _lowPrice;//giá thấp 
            private string _general;//tổng HĐ
            private string _mass;//khối lượng mở


            public string Mass
            {
                get { return _mass; }
                set
                {
                    _mass = value;
                    OnPropertyChanged("Mass");
                }
            }
            public string General
            {
                get { return _general; }
                set
                {
                    _general = value;
                    OnPropertyChanged("General");
                }
            }
            public string LowPrice
            {
                get { return _lowPrice; }
                set
                {
                    _lowPrice = value;
                    OnPropertyChanged("LowPrice");
                }
            }
            public string Expensive
            {
                get { return _expensive; }
                set
                {
                    _expensive = value;
                    OnPropertyChanged("Expensive");
                }
            }
            public string OpeningPrice
            {
                get { return _openingPrice; }
                set
                {
                    _openingPrice = value;
                    OnPropertyChanged("OpeningPrice");
                }
            }
            public string ShareRate
            {
                get { return _shareRate; }
                set
                {
                    _shareRate = value;
                    OnPropertyChanged("ShareRate");
                }
            }
            public string StockValue
            {
                get { return _stockValue; }
                set
                {
                    _stockValue = value;
                    OnPropertyChanged("StockValue");
                }
            }
            public string ImageValue
            {
                get { return _imageValue; }
                set
                {
                    _imageValue = value;
                    OnPropertyChanged("ImageValue");
                }
            }
            public bool Show
            {
                get { return _isShow; }
                set
                {
                    _isShow = value;
                    OnPropertyChanged("Show");
                }
            }
            public string Stock
            {
                get { return _stock; }
                set
                {
                    _stock = value;
                    OnPropertyChanged("Stock");
                }
            }

            public string OverboughtPriceOne { get; set; }
            public string OverboughtPriceTwo { get; set; }
            public string OverboughtPriceThree { get; set; }
            public string OverboughtPriceFour { get; set; }
            public string OverboughtPriceFive { get; set; }
            public string OverboughtPriceSix { get; set; }
            public string ExcessSellingPriceOne { get; set; }
            public string ExcessSellingPriceTwo { get; set; }
            public string ExcessSellingPriceThree { get; set; }
            public string ExcessSellingPriceFour { get; set; }
            public string ExcessSellingPriceFive { get; set; }
            public string ExcessSellingPriceSix { get; set; }
     }
}

