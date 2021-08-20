using AUTOHLT.MOBILE.FakeModules.Models;
using AUTOHLT.MOBILE.ViewModels;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace AUTOHLT.MOBILE.FakeModules.ViewModels
{
    internal class FDetailProductViewModel : ViewModelBase
    {
        private SuggestionsPlacesModel _suggestionsPlacesModel;

        private List<SuggestionsPlacesModel> _placesData = new List<SuggestionsPlacesModel>()
        {
           new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/admins/11/Tip0CzZ_Z2JM1LbO8X1dg-rA.png",
                Title = "Villa Viet Home 3",
                Detail = "30 khách · 5 Phòng ngủ · 4 Phòng tắm",
            },
new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/35993/large/ddd2d246b715de26bc1552dd7f939b24.jpg",
                Title = "Villa Viet Home 9",
                Detail = "25 khách · 5 Phòng ngủ · 4 Phòng tắm",
            },
new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/19823/large/room_19823_3_1556094350.jpg",
                Title = "Villa Viet Home 8",
                Detail = "30 khách · 5 Phòng ngủ · 4 Phòng tắm",
            },
new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/22986/large/room_22986_32_1554275677.jpg",
                Title = "Villa Viet Home 6",
                Detail = "30 khách · 6 Phòng ngủ · 5 Phòng tắm",
            },
new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/66757/large/room_66757_1_1579005667.jpg",
                Title = "DREAM HOUSE Villa 21",
                Detail = "20 khách · 4 Phòng ngủ · 4 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/26948/large/room_26948_13_1561693154.jpg",
                Title = "Royal Villa 2 - View biển - Biệt thự nghỉ dưỡng tuyệt vời",
                Detail = "30 khách · 6 Phòng ngủ · 7 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/admins/11/ORFGKRiPeQs-RFqHwYEJhxW0.png",
                Title = "Huy Anh Villa",
                Detail = "25 khách · 6 Phòng ngủ · 6 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/26037/large/room_26037_10_1559878793.jpg",
                Title = "GerberaHome Trang villa",
                Detail = "30 khách · 9 Phòng ngủ · 10 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/admins/11/qDzX2g3ydukc0AmX_OaCMzw1.png",
                Title = "P22 - Biệt thự hồ bơi liền kề bãi biển Vũng Tàu, View Biển",
                Detail = "20 khách · 3 Phòng ngủ · 3 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/19829/large/room_19829_24_1548145389.jpg",
                Title = "Villa Viet Home 2",
                Detail = "20 khách · 4 Phòng ngủ · 4 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/admins/11/LUtBxWpw5YgvY3HoGcz5dPNc.png",
                Title = "Golden Villa Vung Tau",
                Detail = "20 khách · 8 Phòng ngủ · 10 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/26247/large/room_26247_37_1560330319.jpg",
                Title = "Odwin Villa 02 - Back Beach",
                Detail = "20 khách · 4 Phòng ngủ · 4 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/admins/12/wgh7-0vDHhM30ov7QbVORG1P.png",
                Title = " Victory Villa  Sân Vườn - Hồ Bơi - Karaoke ",
                Detail = "30 khách · 4 Phòng ngủ · 4 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/users/306204/TXzJFk8FDUY772nUyvadQ7jf.jpg",
                Title = "DREAM HOUSE Villa NGỌC TƯỚC",
                Detail = "25 khách · 5 Phòng ngủ · 6 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/users/31742/F_9Nk13ps78MVXYkv7KnfZOE.jpg",
                Title = "SEASIDE WINNER POOL VILLA 4ROOMS",
                Detail = "15 khách · 4 Phòng ngủ · 4 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/42390/large/71A3DEC9-D69F-43E4-8744-6ECD56FD0A05.jpg",
                Title = "Joy'n Villa 2 - Bring the happiness to you",
                Detail = "25 khách · 4 Phòng ngủ · 4 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/admins/11/uw9FdyRKqveoMdwQk2SMYBdw.png",
                Title = "Lavie House 3 - Biệt Thự Hồ Bơi, Karaoke",
                Detail = "30 khách · 5 Phòng ngủ · 6 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/users/65304/14xSakQMLqxXcVmtXs3sbzEJ.jpg",
                Title = "Joy'n Villa 3 - Luxury Pool Villa 5bedrooms (Bida + sauna room)",
                Detail = "35 khách · 5 Phòng ngủ · 4 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/users/407910/Wot3pD-XyiwTMuVOTfGRMMTT.jpg",
                Title = "SAN HÔ VILLA FAMI",
                Detail = "15 khách · 3 Phòng ngủ · 4 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/users/21461/1Gm6xHhAz3hg46O6siUjnHlK.jpg",
                Title = "Cát Tường Pool Villa",
                Detail = "30 khách · 5 Phòng ngủ · 4 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/30538/large/8722ec0a898d6ed3379c.jpg",
                Title = "BAVI Padme Home - Bungalow 2",
                Detail = "3 khách · 1 Phòng ngủ · 1 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/14572/large/1535445885_76C21BF5-597B-4B4F-851D-90A3668AEA67.jpg",
                Title = "Ruahouse Hilltop – Nhà ba gian",
                Detail = "2 khách · 1 Phòng ngủ · 2 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/admins/12/ddtivekfcee8B2z1vHlBm4nI.png",
                Title = "Cloudy View Garden",
                Detail = "20 khách · 2 Phòng ngủ · 2 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/users/12380/lcyhz5HjIkb7SVhmxTPGT3tk.jpg",
                Title = "Hoa Mai Viên Sóc Sơn Villa",
                Detail = "100 khách · 4 Phòng ngủ · 4 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/26362/large/room_26362_21_1560702733.jpg",
                Title = "Tribal Drawing-room Great For Quiet Getaway@VNVECT",
                Detail = "6 khách · 1 Phòng ngủ · 1 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/40999/large/FF302601-A7C6-4295-8575-5D8AC839E3FD.jpg",
                Title = "Brick House Tam Dao Golf một không gian khác biệt một nơi đến lý tưởng",
                Detail = "16 khách · 4 Phòng ngủ · 2 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/14456/large/1537270179_IMG20180705143414-01.jpg",
                Title = "Runaway - The Chipmunk",
                Detail = "5 khách · 2 Phòng ngủ · 1 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/30505/large/2717f3127521937fca30.jpg",
                Title = "BAVI Padme Home - Villa",
                Detail = "26 khách · 2 Phòng ngủ · 3 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/users/29568/XJxj-_8a8zv1PZJ0_ZtYIWC9.jpg",
                Title = "Rose Villas _ Full House",
                Detail = "20 khách · 6 Phòng ngủ · 6 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/users/242536/X-Feq5BtTh0tsVfyvnGigojX.jpg",
                Title = "BIỆT THỰ ĐỒNG QUÊ - Biệt thự 4 Phòng ngủ - tiệc sân vườn  - câu cá & đốt lửa trại",
                Detail = "20 khách · 4 Phòng ngủ · 4 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/15220/large/1537505972_DSC05413.jpg",
                Title = "Reunion - The Chipmunk",
                Detail = "15 khách · 2 Phòng ngủ · 2 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/30534/large/f785394a9a4f7d11245e.jpg",
                Title = "BAVI Padme Home - Bungalow 1",
                Detail = "3 khách · 1 Phòng ngủ · 1 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/16754/medium/1541472554_dsc_3456jpg",
                Title = "Embossi Garden - Lodge 4",
                Detail = "4 khách · 1 Phòng ngủ · 1 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/admins/12/YmXNdw4P34sEO22BilHf7Fip.png",
                Title = "OKia Treehouse - Nhà Xoài",
                Detail = "4 khách · 1 Phòng ngủ · 1 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/16753/medium/1541472104_dsc_3444jpg",
                Title = "Embossi Garden - Lodge 3",
                Detail = "4 khách · 1 Phòng ngủ · 1 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/16751/large/1541471702_dsc_3446jpg",
                Title = "Embossi Garden - Lodge 2",
                Detail = "4 khách · 1 Phòng ngủ · 1 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/admins/12/9AIolI2Oq7l_2eUk8URCNauT.png",
                Title = "Mường Bi Homestay - Trải nghiệm không gian văn hóa Việt Mường",
                Detail = "40 khách · 4 Phòng ngủ · 5 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/42686/large/78858364_10158348898610649_9061496592074276864_o.jpg",
                Title = "Van Son Garden - HomeStay Hoa Lac - Daisy Studio",
                Detail = "6 khách · 1 Phòng ngủ · 1 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/users/168897/KYu7Kgaq-ZF2sKqikgUC7WB_.png",
                Title = "OKiaTreehouse - NHÀ KHẾ",
                Detail = "4 khách · 1 Phòng ngủ · 1 Phòng tắm",
            }, new SuggestionsPlacesModel()
            {
                UriImage = "https://cdn.luxstay.com/rooms/16750/medium/1541471310_dsc_3398jpg",
                Title = "Embossi Garden - Lodge 1",
                Detail = "4 khách · 1 Phòng ngủ · 1 Phòng tắm",
            }
        };

        public List<SuggestionsPlacesModel> PlacesData
        {
            get => _placesData;
            set => SetProperty(ref _placesData, value);
        }

        public ICommand DetailCommand { get; private set; }

        public SuggestionsPlacesModel SuggestionsPlacesModel
        {
            get => _suggestionsPlacesModel;
            set => SetProperty(ref _suggestionsPlacesModel, value);
        }

        public FDetailProductViewModel(INavigationService navigationService) : base(navigationService)
        {
            DetailCommand = new AsyncCommand<SuggestionsPlacesModel>(async (places) => await Task.Run(() =>
            {
                SuggestionsPlacesModel = places;
            }));
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                SuggestionsPlacesModel = parameters.GetValue<SuggestionsPlacesModel>("DetailProduct");
            }
            var random = new Random();
            PlacesData = PlacesData.Skip(random.Next(1, 30)).Take(10).OrderBy(x => x.Title).ToList();
        }
    }
}