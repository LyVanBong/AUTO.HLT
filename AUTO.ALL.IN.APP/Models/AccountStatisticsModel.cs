using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;

namespace AUTO.ALL.IN.APP.Models
{
    public class AccountStatisticsModel : BindableBase
    {
        private int _total;
        private int _new;
        private int _running;
        private int _pause;
        private int _died;

        public int Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        public int New
        {
            get => _new;
            set => SetProperty(ref _new, value);
        }

        public int Running
        {
            get => _running;
            set => SetProperty(ref _running, value);
        }

        public int Pause
        {
            get => _pause;
            set => SetProperty(ref _pause, value);
        }

        public int Died
        {
            get => _died;
            set => SetProperty(ref _died, value);
        }

        public AccountStatisticsModel(ObservableCollection<UserFacebookModel> user)
        {
            if (user != null && user.Any())
            {
                _total = user.Count;
                _new = user.Count(x => x.Status == 0);
                _running = user.Count(x => x.Status == 1);
                _pause = user.Count(x => x.Status == 2);
                _died = user.Count(x => x.Status == 3);
            }
        }
    }
}