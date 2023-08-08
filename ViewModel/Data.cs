using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        /// <summary>
        /// Доходность за период
        /// </summary>
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

        /// <summary>
        /// Cовокупный среднегодовой темп роста
        /// </summary>
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

        /// <summary>
        /// Максимальная просадка
        /// </summary>
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

        /// <summary>
        /// Коэффициент Шарпа
        /// </summary>
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

        /// <summary>
        /// Коэффициент Сортино
        /// </summary>
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

        /// <summary>
        /// Корреляция пользовательской доходности с бенчмарком
        /// </summary>
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

        /// <summary>
        /// Прибыль/убыток ото дня к дню
        /// </summary>
        public ObservableCollection<decimal> DaysPL
        {
            get => _daysPL;

            set
            {
                _daysPL = value;
                OnPropertyChanged(nameof(DaysPL));
            }
        }
        private ObservableCollection<decimal> _daysPL;

        /// <summary>
        /// Прибыль/убыток по отношению к первому дню
        /// </summary>
        public ObservableCollection<decimal> Equity
        {
            get => _equity;

            set
            {
                _equity = value;
                OnPropertyChanged(nameof(Equity));
            }
        }
        private ObservableCollection<decimal> _equity;

        public LineSeries LineSeries
        {
            get => _lineSeries;

            set
            {
                _lineSeries = value;
                OnPropertyChanged(nameof(LineSeries));
            }
        }
        private LineSeries _lineSeries;
    }
}
