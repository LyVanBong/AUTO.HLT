using AUTO.HLT.ADMIN.Models.Facebook;
using AUTO.HLT.ADMIN.Models.RequestProviderModel;
using AUTO.HLT.ADMIN.Services.Facebook;
using AUTO.HLT.ADMIN.Views.AddWork;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Input;
using AUTO.HLT.ADMIN.Views.AutoHltCrm;
using AUTO.HLT.ADMIN.Views.CheckWork;

namespace AUTO.HLT.ADMIN.ViewModels.Main
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "AUTOHLT ADMIN";
        private IRegionManager _regionManager;

        public ICommand UseFeatureCommand { get; private set; }
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            UseFeatureCommand = new DelegateCommand<string>(UseFeature);
        }

        private void UseFeature(string key)
        {
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            switch (key)
            {
                case "0":
                    _regionManager.RequestNavigate("ContentRegion", nameof(AddWorkView));
                    break;
                case "1":
                    _regionManager.RequestNavigate("ContentRegion", nameof(CheckWorkView));
                    break;
                case "2":
                    _regionManager.RequestNavigate("ContentRegion", nameof(AutoHltCrmView));
                    break;
            }
        }
    }
}
