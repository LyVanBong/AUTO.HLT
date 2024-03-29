﻿using Prism.Mvvm;

namespace AUTO.ALL.IN.APP.Models
{
    public class OptionReacModel : BindableBase
    {
        private bool _isSelectFunction;
        private int _indexOptionReac = 1;
        private string _comment = "❤️";
        private string _messager = "❤️";
        private int _timeDelay = 5;
        private long _totalReaction;

        public bool IsSelectFunction
        {
            get => _isSelectFunction;
            set => SetProperty(ref _isSelectFunction, value);
        }

        public int IndexOptionReac
        {
            get => _indexOptionReac;
            set => SetProperty(ref _indexOptionReac, value);
        }

        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public string Messager
        {
            get => _messager;
            set => SetProperty(ref _messager, value);
        }

        public int TimeDelay
        {
            get => _timeDelay;
            set => SetProperty(ref _timeDelay, value);
        }

        public long TotalReaction
        {
            get => _totalReaction;
            set => SetProperty(ref _totalReaction, value);
        }
    }
}