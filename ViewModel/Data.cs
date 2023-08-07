﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equity.ViewModel
{
    public class Data : BaseVM
    {
        public string Name
        {
            get => _name;

            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _name;

        public decimal TotalYield
        {
            get => Math.Round(_yield, 2);

            set
            {
                _yield = value;
                OnPropertyChanged(nameof(TotalYield));
            }
        }
        private decimal _yield;

        public double CAGR
        {
            get => Math.Round(_cagr, 2);

            set
            {
                _cagr = value;
                OnPropertyChanged(nameof(CAGR));
            }
        }
        private double _cagr;

        public decimal MaxDrawDown
        {
            get => Math.Round(_maxDrawDown, 2);

            set
            {
                _maxDrawDown = value;
                OnPropertyChanged(nameof(MaxDrawDown));
            }
        }
        private decimal _maxDrawDown;

        public decimal SharpeRatio
        {
            get => Math.Round(_sharpeRatio, 2);

            set
            {
                _sharpeRatio = value;
                OnPropertyChanged(nameof(SharpeRatio));
            }
        }
        private decimal _sharpeRatio;

        public decimal SortinoRatio
        {
            get => Math.Round(_sortinoRatio, 2);

            set
            {
                _sortinoRatio = value;
                OnPropertyChanged(nameof(SortinoRatio));
            }
        }
        private decimal _sortinoRatio;

        public decimal MarketCorrelation
        {
            get => Math.Round(_marketCorrelation, 2);

            set
            {
                _marketCorrelation = value;
                OnPropertyChanged(nameof(MarketCorrelation));
            }
        }
        private decimal _marketCorrelation;

    }
}
