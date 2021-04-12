using System;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Views.FakeUpApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AUTOHLT.MOBILE.Views.FakeUpApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatLenhPageDetail : Xamarin.Forms.ContentPage
    {
        private double GiaDieuKien;
        private double GiaBienTruot;
        public DatLenhPageDetail()
        {
            InitializeComponent();
            BindingContext = new DatLenhViewModel();
        }

        private void ShowStock(object sender, EventArgs e)
        {
            EntryStock.Focus();
        }

        private void TurnOffShowStock(object sender, FocusEventArgs e)
        {
            StockCode.IsVisible = false;
        }

        private async void ClickOverBoughtOne(object sender, EventArgs e)
        {
            OverBoughtOne.BackgroundColor = Color.FromHex("#c9c9c9");
            ColorEntryGia.BackgroundColor = Color.FromHex("#c9c9c9");
            ColorSell.BackgroundColor = Color.FromHex("#c9c9c9");
            await Task.Delay(300);
            OverBoughtOne.BackgroundColor = Color.FromHex("#b9e3b8");
            ColorSell.BackgroundColor = Color.FromHex("#f3f3f3");
            ColorEntryGia.BackgroundColor = Color.White;
        }

        private async void ClickOverBoughtTwo(object sender, EventArgs e)
        {
            OverBoughtTwo.BackgroundColor = Color.FromHex("#c9c9c9");
            ColorEntryGia.BackgroundColor = Color.FromHex("#c9c9c9");
            ColorSell.BackgroundColor = Color.FromHex("#c9c9c9");
            await Task.Delay(300);
            OverBoughtTwo.BackgroundColor = Color.FromHex("#b9e3b8");
            ColorSell.BackgroundColor = Color.FromHex("#f3f3f3");
            ColorEntryGia.BackgroundColor = Color.White;
        }

        private async void ClickOverBoughtThree(object sender, EventArgs e)
        {
            OverBoughtThree.BackgroundColor = Color.FromHex("#c9c9c9");
            ColorEntryGia.BackgroundColor = Color.FromHex("#c9c9c9");
            ColorSell.BackgroundColor = Color.FromHex("#c9c9c9");
            await Task.Delay(300);
            OverBoughtThree.BackgroundColor = Color.FromHex("#b9e3b8");
            ColorSell.BackgroundColor = Color.FromHex("#f3f3f3");
            ColorEntryGia.BackgroundColor = Color.White;
        }

        private async void ClickExcessSaleThree(object sender, EventArgs e)
        {
            ExcessSaleThree.BackgroundColor = Color.FromHex("#c9c9c9");
            ColorEntryGia.BackgroundColor = Color.FromHex("#c9c9c9");
            ColorBuy.BackgroundColor = Color.FromHex("#c9c9c9");
            await Task.Delay(300);
            ExcessSaleThree.BackgroundColor = Color.FromHex("#fde8e9");
            ColorBuy.BackgroundColor = Color.FromHex("#f3f3f3");
            ColorEntryGia.BackgroundColor = Color.White;
        }

        private async void ClickExcessSaleOne(object sender, EventArgs e)
        {
            ExcessSaleOne.BackgroundColor = Color.FromHex("#c9c9c9");
            ColorEntryGia.BackgroundColor = Color.FromHex("#c9c9c9");
            ColorBuy.BackgroundColor = Color.FromHex("#c9c9c9");
            await Task.Delay(300);
            ExcessSaleOne.BackgroundColor = Color.FromHex("#fde8e9");
            ColorBuy.BackgroundColor = Color.FromHex("#f3f3f3");
            ColorEntryGia.BackgroundColor = Color.White;
        }

        private async void ClickExcessSaleTwo(object sender, EventArgs e)
        {
            ExcessSaleTwo.BackgroundColor = Color.FromHex("#c9c9c9");
            ColorEntryGia.BackgroundColor = Color.FromHex("#c9c9c9");
            ColorBuy.BackgroundColor = Color.FromHex("#c9c9c9");
            await Task.Delay(300);
            ExcessSaleTwo.BackgroundColor = Color.FromHex("#fde8e9");
            ColorBuy.BackgroundColor = Color.FromHex("#f3f3f3");
            ColorEntryGia.BackgroundColor = Color.White;
        }

        private async void ClickMax(object sender, EventArgs e)
        {
            Lbl_Max.IsVisible = false;
            ColorEntryKhoiLuong.BackgroundColor = Color.FromHex("#c9c9c9");
            await Task.Delay(200);
            Lbl_Max.IsVisible = true;
            ColorEntryKhoiLuong.BackgroundColor = Color.White;
        }

        private void TurnOffShowLenh(object sender, FocusEventArgs e)
        {
            ShowLenh.IsVisible = false;
        }

        private void ClickShowLenh(object sender, EventArgs e)
        {
            EntryShowLenh.Focus();
        }
        //điều kiện UP
        private async void UnfocusGiaDieuKien(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(EntryGiaDieuKien.Text))
            {
                GiaDieuKien = double.Parse(EntryGiaDieuKien.Text);
                GiaBienTruot = double.Parse(EntryBienTruot.Text);
                if (GiaDieuKien > 1.272 || GiaDieuKien < 1.118)
                {
                    await DisplayAlert("Thông báo", "Giá điều kiện không được nằm ngoài biên độ trần sàn.", "ok");
                    EntryGiaDieuKien.Text = "";
                }
                if (GiaDieuKien < 1.272 && GiaDieuKien > 1.118 && !string.IsNullOrEmpty(EntryBienTruot.Text) && (GiaDieuKien + GiaBienTruot) < 1.272)
                {
                    Lbl_GiaDieuChinh.Text = (GiaDieuKien + GiaBienTruot).ToString();
                }
                if (GiaDieuKien < 1.272 && GiaDieuKien > 1.118 && !string.IsNullOrEmpty(EntryBienTruot.Text) && (GiaDieuKien + GiaBienTruot) > 1.272)
                {
                    await DisplayAlert("Thông báo", "Giá điều kiện không được nằm ngoài biên độ trần sàn.", "ok");
                    EntryGiaDieuKien.Text = "";
                }
                if (GiaDieuKien < 1.272 && GiaDieuKien > 1.118 && string.IsNullOrEmpty(EntryBienTruot.Text))
                {
                    Lbl_GiaDieuChinh.Text = GiaDieuKien.ToString();
                }
                if (IsConditionUP.IsVisible == true)
                {
                    if (!string.IsNullOrEmpty(EntryGiaDieuKien.Text) && !string.IsNullOrEmpty(EntryBienTruot.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Green;
                    }
                    if (!string.IsNullOrEmpty(EntryGiaDieuKien.Text) && !string.IsNullOrEmpty(EntryBienTruot.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Red;
                    }
                    if (string.IsNullOrEmpty(EntryGiaDieuKien.Text) || string.IsNullOrEmpty(EntryBienTruot.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                    }
                }
            }
            else
                EntryGiaDieuKien.Text = "";
        }

        private async void UnfocusBientruot(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(EntryGiaDieuKien.Text) && !string.IsNullOrEmpty(EntryBienTruot.Text))
            {
                GiaBienTruot = double.Parse(EntryBienTruot.Text);
                GiaDieuKien = double.Parse(EntryGiaDieuKien.Text);
                if ((GiaDieuKien + GiaBienTruot) > 1.272)
                {
                    await DisplayAlert("Thông báo", "Giá điều kiện không được nằm ngoài biên độ trần sàn.", "ok");
                    EntryBienTruot.Text = "0";
                }
                if ((GiaDieuKien + GiaBienTruot) < 1.272)
                {
                    Lbl_GiaDieuChinh.Text = (GiaDieuKien + GiaBienTruot).ToString();
                }
                if (IsConditionUP.IsVisible == true)
                {
                    if (!string.IsNullOrEmpty(EntryGiaDieuKien.Text) && !string.IsNullOrEmpty(EntryBienTruot.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Green;
                    }
                    if (!string.IsNullOrEmpty(EntryGiaDieuKien.Text) && !string.IsNullOrEmpty(EntryBienTruot.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Red;
                    }
                    if (string.IsNullOrEmpty(EntryGiaDieuKien.Text) || string.IsNullOrEmpty(EntryBienTruot.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                    }
                }
            }
            if (string.IsNullOrEmpty(EntryBienTruot.Text))
            {
                EntryBienTruot.Text = "0";
            }
        }
        //điều kiện chung
        private void UnfocusEntryKhoiLuong(object sender, FocusEventArgs e)
        {
            if (IsConditionUP.IsVisible == true)
            {
                if (!string.IsNullOrEmpty(EntryGiaDieuKien.Text) && !string.IsNullOrEmpty(EntryBienTruot.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Green;
                }
                if (!string.IsNullOrEmpty(EntryGiaDieuKien.Text) && !string.IsNullOrEmpty(EntryBienTruot.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Red;
                }
                if (string.IsNullOrEmpty(EntryGiaDieuKien.Text) || string.IsNullOrEmpty(EntryBienTruot.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                }
            }
            if (IsConditionDOWN.IsVisible == true)
            {
                if (!string.IsNullOrEmpty(EntryGiaDieuKienDown.Text) && !string.IsNullOrEmpty(EntryBienTruotDown.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Green;
                }
                if (!string.IsNullOrEmpty(EntryGiaDieuKienDown.Text) && !string.IsNullOrEmpty(EntryBienTruotDown.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Red;
                }
                if (string.IsNullOrEmpty(EntryGiaDieuKienDown.Text) || string.IsNullOrEmpty(EntryBienTruotDown.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                }
            }
            if (IsConditionOCO.IsVisible == true)
            {
                if (!string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) && !string.IsNullOrEmpty(EntryBienTruotOCO.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Green;
                }
                if (!string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) && !string.IsNullOrEmpty(EntryBienTruotOCO.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Red;
                }
                if (string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) || string.IsNullOrEmpty(EntryBienTruotOCO.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || !string.IsNullOrEmpty(EntryGia.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                }
            }
            if (IsConditionBB.IsVisible == true)
            {
                if (!string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) && !string.IsNullOrEmpty(EntryGia.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text) && !string.IsNullOrEmpty(Lbl_GiaDieukienBB.Text) && !string.IsNullOrEmpty(Lbl_GiaDieuChinhBB.Text) && !string.IsNullOrEmpty(Lbl_GiaChotLaiBB.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Green;
                }
                if (!string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) && !string.IsNullOrEmpty(EntryGia.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text) && !string.IsNullOrEmpty(Lbl_GiaDieukienBB.Text) && !string.IsNullOrEmpty(Lbl_GiaDieuChinhBB.Text) && !string.IsNullOrEmpty(Lbl_GiaChotLaiBB.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Red;
                }
                if (string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) || string.IsNullOrEmpty(EntryGia.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || !string.IsNullOrEmpty(EntryGia.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text) || !string.IsNullOrEmpty(Lbl_GiaDieukienBB.Text) || !string.IsNullOrEmpty(Lbl_GiaDieuChinhBB.Text) || !string.IsNullOrEmpty(Lbl_GiaChotLaiBB.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                }
            }
        }

        private async void UnfocusEntryMatKhau(object sender, FocusEventArgs e)
        {
            if (IsConditionUP.IsVisible == true)
            {
                if (!string.IsNullOrEmpty(EntryGiaDieuKien.Text) && !string.IsNullOrEmpty(EntryBienTruot.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Green;
                }
                if (!string.IsNullOrEmpty(EntryGiaDieuKien.Text) && !string.IsNullOrEmpty(EntryBienTruot.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Red;
                }
                if (string.IsNullOrEmpty(EntryGiaDieuKien.Text) || string.IsNullOrEmpty(EntryBienTruot.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                }
            }
            if (IsConditionDOWN.IsVisible == true)
            {
                if (!string.IsNullOrEmpty(EntryGiaDieuKienDown.Text) && !string.IsNullOrEmpty(EntryBienTruotDown.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Green;
                }
                if (!string.IsNullOrEmpty(EntryGiaDieuKienDown.Text) && !string.IsNullOrEmpty(EntryBienTruotDown.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Red;
                }
                if (string.IsNullOrEmpty(EntryGiaDieuKienDown.Text) || string.IsNullOrEmpty(EntryBienTruotDown.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                }
            }
            if (IsConditionOCO.IsVisible == true)
            {
                if (!string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) && !string.IsNullOrEmpty(EntryBienTruotOCO.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Green;
                }
                if (!string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) && !string.IsNullOrEmpty(EntryBienTruotOCO.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Red;
                }
                if (string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) || string.IsNullOrEmpty(EntryBienTruotOCO.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || !string.IsNullOrEmpty(EntryGia.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                }
            }
            if (IsConditionBB.IsVisible == true)
            {
                if (!string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) && !string.IsNullOrEmpty(EntryGia.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text) && !string.IsNullOrEmpty(Lbl_GiaDieukienBB.Text) && !string.IsNullOrEmpty(Lbl_GiaDieuChinhBB.Text) && !string.IsNullOrEmpty(Lbl_GiaChotLaiBB.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Green;
                }
                if (!string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) && !string.IsNullOrEmpty(EntryGia.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text) && !string.IsNullOrEmpty(Lbl_GiaDieukienBB.Text) && !string.IsNullOrEmpty(Lbl_GiaDieuChinhBB.Text) && !string.IsNullOrEmpty(Lbl_GiaChotLaiBB.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.Red;
                }
                if (string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) || string.IsNullOrEmpty(EntryGia.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || !string.IsNullOrEmpty(EntryGia.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text) || !string.IsNullOrEmpty(Lbl_GiaDieukienBB.Text) || !string.IsNullOrEmpty(Lbl_GiaDieuChinhBB.Text) || !string.IsNullOrEmpty(Lbl_GiaChotLaiBB.Text))
                {
                    Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                }
            }
            if (EntryMatKhau.Text != "09121998")
            {
                await DisplayAlert("Thông báo", "Mật khẩu không đúng", "ok");
                EntryMatKhau.Text = "";
            }
        }
        //lệnh down
        private async void UnfocusGiaDieuKienDOWN(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(EntryGiaDieuKienDown.Text))
            {
                GiaDieuKien = double.Parse(EntryGiaDieuKienDown.Text);
                GiaBienTruot = double.Parse(EntryBienTruotDown.Text);
                if (GiaDieuKien > 1.272 || GiaDieuKien < 1.118)
                {
                    await DisplayAlert("Thông báo", "Giá điều kiện không được nằm ngoài biên độ trần sàn.", "ok");
                    EntryGiaDieuKienDown.Text = "";
                }
                if (GiaDieuKien < 1.272 && GiaDieuKien > 1.118 && !string.IsNullOrEmpty(EntryBienTruotDown.Text) && (GiaDieuKien - GiaBienTruot) > 1.118)
                {
                    Lbl_GiaDieuChinhDown.Text = (GiaDieuKien - GiaBienTruot).ToString();
                }
                if (GiaDieuKien < 1.272 && GiaDieuKien > 1.118 && !string.IsNullOrEmpty(EntryBienTruotDown.Text) && (GiaDieuKien - GiaBienTruot) < 1.118)
                {
                    await DisplayAlert("Thông báo", "Giá điều kiện không được nằm ngoài biên độ trần sàn.", "ok");
                    EntryGiaDieuKienDown.Text = "";
                }
                if (GiaDieuKien < 1.272 && GiaDieuKien > 1.118 && string.IsNullOrEmpty(EntryBienTruotDown.Text))
                {
                    Lbl_GiaDieuChinhDown.Text = GiaDieuKien.ToString();
                }
                if (IsConditionDOWN.IsVisible == true)
                {
                    if (!string.IsNullOrEmpty(EntryGiaDieuKienDown.Text) && !string.IsNullOrEmpty(EntryBienTruotDown.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Green;
                    }
                    if (!string.IsNullOrEmpty(EntryGiaDieuKienDown.Text) && !string.IsNullOrEmpty(EntryBienTruotDown.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Red;
                    }
                    if (string.IsNullOrEmpty(EntryGiaDieuKienDown.Text) || string.IsNullOrEmpty(EntryBienTruotDown.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                    }
                }
            }
            else
                EntryGiaDieuKienDown.Text = "";
        }

        private async void UnfocusBientruotDOWN(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(EntryGiaDieuKienDown.Text) && !string.IsNullOrEmpty(EntryBienTruotDown.Text))
            {
                GiaBienTruot = double.Parse(EntryBienTruotDown.Text);
                GiaDieuKien = double.Parse(EntryGiaDieuKienDown.Text);
                if ((GiaDieuKien - GiaBienTruot) < 1.118)
                {
                    await DisplayAlert("Thông báo", "Giá điều kiện không được nằm ngoài biên độ trần sàn.", "ok");
                    EntryBienTruotDown.Text = "0";
                }
                if ((GiaDieuKien - GiaBienTruot) < 1.272 && (GiaDieuKien - GiaBienTruot) > 1.118)
                {
                    Lbl_GiaDieuChinhDown.Text = (GiaDieuKien + GiaBienTruot).ToString();
                }
                if (IsConditionDOWN.IsVisible == true)
                {
                    if (!string.IsNullOrEmpty(EntryGiaDieuKienDown.Text) && !string.IsNullOrEmpty(EntryBienTruotDown.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Green;
                    }
                    if (!string.IsNullOrEmpty(EntryGiaDieuKienDown.Text) && !string.IsNullOrEmpty(EntryBienTruotDown.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Red;
                    }
                    if (string.IsNullOrEmpty(EntryGiaDieuKienDown.Text) || string.IsNullOrEmpty(EntryBienTruotDown.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                    }
                }
            }
            if (string.IsNullOrEmpty(EntryBienTruotDown.Text))
            {
                EntryBienTruotDown.Text = "0";
            }
        }
        //xử lý entry giá
        private async void UnfocusEntryGia(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(EntryGia.Text))
            {
                GiaDieuKien = double.Parse(EntryGia.Text);
                if (GiaDieuKien > 1.272 || GiaDieuKien < 1.118)
                {
                    await DisplayAlert("Thông báo", "Giá điều kiện không được nằm ngoài biên độ trần sàn.", "ok");
                    EntryGiaDieuKien.Text = "";
                }
                if (GiaDieuKien < 1.272 && GiaDieuKien > 1.118)
                {
                    EntryGiaDieuKien.Text = GiaDieuKien.ToString();
                    Lbl_GiaChotLai.Text= GiaDieuKien.ToString();
                    if (IsConditionOCO.IsVisible == true)
                    {
                        if (!string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) && !string.IsNullOrEmpty(EntryBienTruotOCO.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                        {
                            Btn_XacNhan.BackgroundColor = Color.Green;
                        }
                        if (!string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) && !string.IsNullOrEmpty(EntryBienTruotOCO.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                        {
                            Btn_XacNhan.BackgroundColor = Color.Red;
                        }
                        if (string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) || string.IsNullOrEmpty(EntryBienTruotOCO.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || !string.IsNullOrEmpty(EntryGia.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text))
                        {
                            Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                        }
                    }
                }
            }
            else
            {
                EntryGia.Text = "";
                Lbl_GiaChotLai.Text = "";
            }
        }

        private async void ClickGiaTran(object sender, EventArgs e)
        {
            ColorEntryGia.BackgroundColor = Color.FromHex("#c9c9c9");
            Lbl_giatran.BackgroundColor = Color.FromHex("#c9c9c9");
            await Task.Delay(300);
            ColorEntryGia.BackgroundColor = Color.White;
            Lbl_giatran.BackgroundColor = Color.White;
        }

        private async void ClickGiaThamChieu(object sender, EventArgs e)
        {
            ColorEntryGia.BackgroundColor = Color.FromHex("#c9c9c9");
            Lbl_giathamchieu.BackgroundColor = Color.FromHex("#c9c9c9");
            await Task.Delay(300);
            ColorEntryGia.BackgroundColor = Color.White;
            Lbl_giathamchieu.BackgroundColor = Color.White;
        }

        private async void ClickGiaSan(object sender, EventArgs e)
        {
            ColorEntryGia.BackgroundColor = Color.FromHex("#c9c9c9");
            Lbl_giasan.BackgroundColor = Color.FromHex("#c9c9c9");
            await Task.Delay(300);
            ColorEntryGia.BackgroundColor = Color.White;
            Lbl_giasan.BackgroundColor = Color.White;
        }
        //lệnh OCO
        private async void UnfocusGiaDieuKienOCO(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text))
            {
                GiaDieuKien = double.Parse(EntryGiaDieuKienOCO.Text);
                GiaBienTruot = double.Parse(EntryBienTruotOCO.Text);
                if (GiaDieuKien > 1.272 || GiaDieuKien < 1.118)
                {
                    await DisplayAlert("Thông báo", "Giá điều kiện không được nằm ngoài biên độ trần sàn.", "ok");
                    EntryGiaDieuKienOCO.Text = "";
                }
                if (GiaDieuKien < 1.272 && GiaDieuKien > 1.118 && !string.IsNullOrEmpty(EntryBienTruotOCO.Text) && (GiaDieuKien + GiaBienTruot) < 1.272)
                {
                    Lbl_GiaDieuChinhOCO.Text = (GiaDieuKien + GiaBienTruot).ToString();
                }
                if (GiaDieuKien < 1.272 && GiaDieuKien > 1.118 && !string.IsNullOrEmpty(EntryBienTruotOCO.Text) && (GiaDieuKien + GiaBienTruot) > 1.272)
                {
                    await DisplayAlert("Thông báo", "Giá điều kiện không được nằm ngoài biên độ trần sàn.", "ok");
                    EntryGiaDieuKienOCO.Text = "";
                }
                if (GiaDieuKien < 1.272 && GiaDieuKien > 1.118 && string.IsNullOrEmpty(EntryBienTruotOCO.Text))
                {
                    Lbl_GiaDieuChinhOCO.Text = GiaDieuKien.ToString();
                }
                if (IsConditionOCO.IsVisible == true)
                {
                    if (!string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) && !string.IsNullOrEmpty(EntryBienTruotOCO.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Green;
                    }
                    if (!string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) && !string.IsNullOrEmpty(EntryBienTruotOCO.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Red;
                    }
                    if (string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) || string.IsNullOrEmpty(EntryBienTruotOCO.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || !string.IsNullOrEmpty(EntryGia.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                    }
                }
            }
            else
                EntryGiaDieuKienOCO.Text = "";
        }

        private async void UnfocusGiaBienTruotOCO(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) && !string.IsNullOrEmpty(EntryBienTruotOCO.Text))
            {
                GiaBienTruot = double.Parse(EntryBienTruotOCO.Text);
                GiaDieuKien = double.Parse(EntryGiaDieuKienOCO.Text);
                if ((GiaDieuKien + GiaBienTruot) > 1.272)
                {
                    await DisplayAlert("Thông báo", "Giá điều kiện không được nằm ngoài biên độ trần sàn.", "ok");
                    EntryBienTruotOCO.Text = "0";
                }
                if ((GiaDieuKien + GiaBienTruot) < 1.272)
                {
                    Lbl_GiaDieuChinhOCO.Text = (GiaDieuKien + GiaBienTruot).ToString();
                }
                if (IsConditionOCO.IsVisible == true)
                {
                    if (!string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) && !string.IsNullOrEmpty(EntryBienTruotOCO.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Green;
                    }
                    if (!string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) && !string.IsNullOrEmpty(EntryBienTruotOCO.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Red;
                    }
                    if (string.IsNullOrEmpty(EntryGiaDieuKienOCO.Text) || string.IsNullOrEmpty(EntryBienTruotOCO.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || !string.IsNullOrEmpty(EntryGia.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                    }
                }
            }
            if (string.IsNullOrEmpty(EntryBienTruot.Text))
            {
                EntryBienTruotOCO.Text = "0";
            }
        }
        //lệnh BB
        private async void UnfocusKhoangLoBB(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(EntryKhoangLoBB.Text) && !string.IsNullOrEmpty(EntryGia.Text))
            {
                GiaBienTruot = double.Parse(EntryKhoangLoBB.Text);
                GiaDieuKien = double.Parse(EntryGia.Text);
                if ((GiaDieuKien + GiaBienTruot) > 1.272)
                {
                    await DisplayAlert("Thông báo", "Giá điều kiện không được nằm ngoài biên độ trần sàn.", "ok");
                    EntryGiaDieuKienBB.Text = "0";
                }
                if ((GiaDieuKien + GiaBienTruot) < 1.272)
                {
                    Lbl_GiaDieukienBB.Text = (GiaDieuKien + GiaBienTruot).ToString();
                }
                if (IsConditionBB.IsVisible == true)
                {
                    if (!string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) && !string.IsNullOrEmpty(EntryGia.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text) && !string.IsNullOrEmpty(Lbl_GiaDieukienBB.Text) && !string.IsNullOrEmpty(Lbl_GiaDieuChinhBB.Text) && !string.IsNullOrEmpty(Lbl_GiaChotLaiBB.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Green;
                    }
                    if (!string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) && !string.IsNullOrEmpty(EntryGia.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text) && !string.IsNullOrEmpty(Lbl_GiaDieukienBB.Text) && !string.IsNullOrEmpty(Lbl_GiaDieuChinhBB.Text) && !string.IsNullOrEmpty(Lbl_GiaChotLaiBB.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Red;
                    }
                    if (string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) || string.IsNullOrEmpty(EntryGia.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || !string.IsNullOrEmpty(EntryGia.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text) || !string.IsNullOrEmpty(Lbl_GiaDieukienBB.Text) || !string.IsNullOrEmpty(Lbl_GiaDieuChinhBB.Text) || !string.IsNullOrEmpty(Lbl_GiaChotLaiBB.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                    }
                }
            }
            if (string.IsNullOrEmpty(EntryKhoangLoBB.Text)|| EntryKhoangLoBB.Text=="0")
            {
                EntryKhoangLoBB.Text = "0";
                Lbl_GiaDieukienBB.Text = "";
            }
        }

        private async void UnfocusKhoangLaiBB(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) && !string.IsNullOrEmpty(EntryGia.Text))
            {
                GiaBienTruot = double.Parse(EntryGiaDieuKienBB.Text);
                GiaDieuKien = double.Parse(EntryGia.Text);
                if ((GiaDieuKien + GiaBienTruot) > 1.272)
                {
                    await DisplayAlert("Thông báo", "Giá điều kiện không được nằm ngoài biên độ trần sàn.", "ok");
                    EntryGiaDieuKienBB.Text = "0";
                }
                if ((GiaDieuKien + GiaBienTruot) < 1.272)
                {
                    Lbl_GiaChotLaiBB.Text = (GiaDieuKien + GiaBienTruot).ToString();
                }
                if (IsConditionBB.IsVisible == true)
                {
                    if (!string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) && !string.IsNullOrEmpty(EntryGia.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text) && !string.IsNullOrEmpty(Lbl_GiaDieukienBB.Text) && !string.IsNullOrEmpty(Lbl_GiaDieuChinhBB.Text) && !string.IsNullOrEmpty(Lbl_GiaChotLaiBB.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Green;
                    }
                    if (!string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) && !string.IsNullOrEmpty(EntryGia.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text) && !string.IsNullOrEmpty(Lbl_GiaDieukienBB.Text) && !string.IsNullOrEmpty(Lbl_GiaDieuChinhBB.Text) && !string.IsNullOrEmpty(Lbl_GiaChotLaiBB.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Red;
                    }
                    if (string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) || string.IsNullOrEmpty(EntryGia.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || !string.IsNullOrEmpty(EntryGia.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text) || !string.IsNullOrEmpty(Lbl_GiaDieukienBB.Text) || !string.IsNullOrEmpty(Lbl_GiaDieuChinhBB.Text) || !string.IsNullOrEmpty(Lbl_GiaChotLaiBB.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                    }
                }
            }
            if (string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) || EntryGiaDieuKienBB.Text == "0")
            {
                EntryGiaDieuKienBB.Text = "0";
                Lbl_GiaChotLaiBB.Text = "";
            }
        }

        private async void UnfocusBienTruotBB(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(EntryBientruotBB.Text) && !string.IsNullOrEmpty(EntryGia.Text))
            {
                GiaBienTruot = double.Parse(EntryBientruotBB.Text);
                GiaDieuKien = double.Parse(EntryGia.Text);
                if ((GiaDieuKien + GiaBienTruot) > 1.272)
                {
                    await DisplayAlert("Thông báo", "Giá điều kiện không được nằm ngoài biên độ trần sàn.", "ok");
                    EntryBientruotBB.Text = "0";
                }
                if ((GiaDieuKien + GiaBienTruot) < 1.272)
                {
                    Lbl_GiaDieuChinhBB.Text = (GiaDieuKien + GiaBienTruot).ToString();
                }
                if (IsConditionBB.IsVisible == true)
                {
                    if (!string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) && !string.IsNullOrEmpty(EntryGia.Text) && EntryMatKhau.Text == "09121998" && Lbl_Mua.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text) && !string.IsNullOrEmpty(Lbl_GiaDieukienBB.Text) && !string.IsNullOrEmpty(Lbl_GiaDieuChinhBB.Text) && !string.IsNullOrEmpty(Lbl_GiaChotLaiBB.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Green;
                    }
                    if (!string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) && !string.IsNullOrEmpty(EntryGia.Text) && EntryMatKhau.Text == "09121998" && Lbl_Ban.FontSize == 22 && !string.IsNullOrEmpty(EntryGia.Text) && !string.IsNullOrEmpty(EntryKhoiLuong.Text) && !string.IsNullOrEmpty(Lbl_GiaDieukienBB.Text) && !string.IsNullOrEmpty(Lbl_GiaDieuChinhBB.Text) && !string.IsNullOrEmpty(Lbl_GiaChotLaiBB.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.Red;
                    }
                    if (string.IsNullOrEmpty(EntryGiaDieuKienBB.Text) || string.IsNullOrEmpty(EntryGia.Text) || string.IsNullOrEmpty(EntryMatKhau.Text) || !string.IsNullOrEmpty(EntryGia.Text) || string.IsNullOrEmpty(EntryKhoiLuong.Text) || !string.IsNullOrEmpty(Lbl_GiaDieukienBB.Text) || !string.IsNullOrEmpty(Lbl_GiaDieuChinhBB.Text) || !string.IsNullOrEmpty(Lbl_GiaChotLaiBB.Text))
                    {
                        Btn_XacNhan.BackgroundColor = Color.FromHex("#b3b3b3");
                    }
                }
            }
            if (string.IsNullOrEmpty(EntryBientruotBB.Text) || EntryBientruotBB.Text == "0")
            {
                EntryBientruotBB.Text = "0";
                Lbl_GiaDieuChinhBB.Text = "";
            }
        }
    }
}