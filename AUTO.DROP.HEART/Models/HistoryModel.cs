using System;
using Prism.Mvvm;

namespace AUTO.DROP.HEART.Models
{
    public class HistoryModel : BindableBase
    {
        private int _stt;
        private string _id;
        private string _story;
        private string _note;
        private string _messager;

        public int Stt
        {
            get => _stt;
            set => SetProperty(ref _stt, value);
        }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Story
        {
            get => _story;
            set => SetProperty(ref _story, value);
        }

        public DateTime DateCreate =>DateTime.Now;

        public string Note
        {
            get => _note;
            set => SetProperty(ref _note, value);
        }

        public string Messager
        {
            get => _messager;
            set => SetProperty(ref _messager, value);
        }
    }
}