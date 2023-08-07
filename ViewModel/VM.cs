using ControlzEx.Controls;
using MahApps.Metro.Controls;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Equity.ViewModel
{
    public class VM:BaseVM
    {
        public VM()
        {
            DateTimes = GetDateTimes();

            Draw(Equity, _lineSeriesMyEquity);

            Calculate(DaysMyPL, Equity, MyEquity);
        }

        #region Fields ===================================================================================

        LineSeries _lineSeriesMyEquity = new LineSeries()
        {
            StrokeThickness = 2,

            Color = OxyColors.Green,

            Title = "MyEquity",

        };

        LineSeries _lineSeriesEquityMoex = new LineSeries()
        {
            StrokeThickness = 2,

            Color = OxyColors.Red,

            Title = "MCFTR"
        };

        LineSeries _lineSeriesEquitySp = new LineSeries()
        {
            StrokeThickness = 2,

            Color = OxyColors.Blue,

            Title = "S&P500 Total Return"
        };

        LineSeries _lineSeriesEquityMredc = new LineSeries()
        {
            StrokeThickness = 2,

            Color = OxyColors.DarkOrange,

            Title = "MREDC Index"
        };

        LineSeries _lineSeriesEquityRueybcstr = new LineSeries()
        {
            StrokeThickness = 2,

            Color = OxyColors.Black,

            Title = "RUEYBCSTR Index"
        };

        private string MyEquity = "MyEquity";
        private string RTS = "RTS";
        private string MOEX = "MCFTR";
        private string MREDC = "MREDC";
        private string SP = "S&P500 Total Return";
        private string RUEYBCSTR = "RUEYBCSTR";

        private decimal RiskFreeRate;

        static readonly HttpClient client = new HttpClient();


        #endregion

        #region Properties ===================================================================================

        public ObservableCollection<DateTime> DateTimes { get; set; } = new ObservableCollection<DateTime>();

        public ObservableCollection<decimal> Equity { get; set; } = new ObservableCollection<decimal>();

        public ObservableCollection<decimal> DaysMyPL { get; set; } = new ObservableCollection<decimal>();

        public ObservableCollection<decimal> DaysMoexPL { get; set; } = new ObservableCollection<decimal>();

        public ObservableCollection<decimal> DaysMredcPL { get; set; } = new ObservableCollection<decimal>();

        public ObservableCollection<decimal> DaysRueybcstrPL { get; set; } = new ObservableCollection<decimal>();

        public ObservableCollection<decimal> DaysSpPL { get; set; } = new ObservableCollection<decimal>();

        public ObservableCollection<decimal> EquityMoex { get; set; } = new ObservableCollection<decimal>();

        public ObservableCollection<decimal> EquityMredc { get; set; } = new ObservableCollection<decimal>();

        public ObservableCollection<decimal> EquityRueybcstr { get; set; } = new ObservableCollection<decimal>();

        public ObservableCollection<decimal> EquitySp { get; set; } = new ObservableCollection<decimal>();

        public ObservableCollection<Data> Datas { get; set; } = new ObservableCollection<Data>();

        public PlotModel Model
        {
            get => _model;

            set
            {
                _model = value;
            }
        }
        private PlotModel _model = new PlotModel(){Background = OxyColors.Silver};

        public bool IsCheckedMOEX
        {
            get => _isCheckedMOEX;

            set
            {
                _isCheckedMOEX = value;
                HandleIsChecked(EquityMoex, DaysMoexPL, MOEX, _lineSeriesEquityMoex).ConfigureAwait(false);
                OnPropertyChanged(nameof(IsCheckedMOEX));

            }
        }
        private bool _isCheckedMOEX;

        public bool IsCheckedMREDC
        {
            get => _isCheckedMREDC;

            set
            {
                _isCheckedMREDC = value;
                HandleIsChecked(EquityMredc, DaysMredcPL, MREDC, _lineSeriesEquityMredc).ConfigureAwait(false);
                OnPropertyChanged(nameof(IsCheckedMREDC));

            }
        }
        private bool _isCheckedMREDC;

        public bool IsCheckedRUEYBCSTR
        {
            get => _isCheckedRUEYBCSTR;

            set
            {
                _isCheckedRUEYBCSTR = value;
                HandleIsChecked(EquityRueybcstr, DaysRueybcstrPL, RUEYBCSTR, _lineSeriesEquityRueybcstr).ConfigureAwait(false);
                OnPropertyChanged(nameof(IsCheckedRUEYBCSTR));

            }
        }
        private bool _isCheckedRUEYBCSTR;

        public bool IsCheckedSP
        {
            get => _isCheckedSP;

            set
            {
                _isCheckedSP = value;
                HandleIsCheckedSP().ConfigureAwait(false);
                OnPropertyChanged(nameof(IsCheckedSP));
            }
        }
        private bool _isCheckedSP;

        #endregion

        #region Commands ===================================================================================



        #endregion

        #region Methods ===================================================================================

        private async Task HandleIsCheckedSP()
        {
            if (IsCheckedSP)
            {
                await GetEquitySp();
                Draw(EquitySp, _lineSeriesEquitySp);
                Calculate(DaysSpPL, EquitySp, SP);
            }
            else
            {
                DeleteEquity(EquitySp, DaysSpPL, SP, _lineSeriesEquitySp);
            }
        }

        private async Task HandleIsChecked(ObservableCollection<decimal> equityList, ObservableCollection<decimal> daysPl, string tiker, LineSeries lineSeries)
        {
            switch (tiker)
            {
                case "MCFTR":
                    if (IsCheckedMOEX)
                    {
                        await GetEquityMoex(equityList, tiker, daysPl);
                        Draw(equityList, lineSeries);
                        Calculate(daysPl, equityList, tiker);
                    }
                    else
                    {
                        DeleteEquity(equityList, daysPl, tiker, lineSeries);
                    }
                    break;
                case "MREDC":
                    if (IsCheckedMREDC)
                    {
                        await GetEquityMoex(equityList, tiker, daysPl);
                        Draw(equityList, lineSeries);
                        Calculate(daysPl, equityList, tiker);
                    }
                    else
                    {
                        DeleteEquity(equityList, daysPl, tiker, lineSeries);
                    }
                    break;
                case "RUEYBCSTR":
                    if (IsCheckedRUEYBCSTR)
                    {
                        await GetEquityMoex(equityList, tiker, daysPl);
                        Draw(equityList, lineSeries);
                        Calculate(daysPl, equityList, tiker);
                    }
                    else
                    {
                        DeleteEquity(equityList, daysPl, tiker, lineSeries);
                    }
                    break;
            }
        }

        private void DeleteEquity(ObservableCollection<decimal> equityList, ObservableCollection<decimal> daysPl,
            string tiker, LineSeries lineSeries)
        {
            Model.Series.Remove(lineSeries);
            Model.InvalidatePlot(true);
            OnPropertyChanged(nameof(Model));

            equityList.Clear();
            daysPl.Clear();
            lineSeries.Points.Clear();

            foreach (var Data in Datas)
            {
                if (Data.Name == tiker)
                {
                    Datas.Remove(Data);
                }
            }
        }

        private async Task GetEquityMoex(ObservableCollection<decimal> equityList, string tiker, ObservableCollection<decimal> daysPl)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{tiker} .csv");

            if (!File.Exists(filePath))
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write("Дата;Цена;Откр.;Макс.;Мин.");
                }
            }

            try
            {
                string[] lines = System.IO.File.ReadAllLines(filePath);

                if (lines == null)
                {
                    MessageBox.Show($"В файле {tiker}.csv отсутствуют значения");
                    return;
                }

                List<decimal> closeMoex = new List<decimal>();

                List<DateTime> missedDates = new List<DateTime>();
                foreach (var dateTime in DateTimes)
                {
                    bool isFound = false;
                    foreach (var line in lines)
                    {
                        var parts = line.Split(';');
                        if (parts.Length < 4) // если строка не содержит нужное количество столбцов - пропускаем ее
                        {
                            continue;
                        }
                        DateTime date;
                        if (!DateTime.TryParseExact(parts[0], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            // если столбец не содержит корректную дату - пропускаем его
                            continue;
                        }
                        if (date == dateTime)
                        {
                            isFound = true;
                            break;
                        }
                    }
                    if (!isFound)
                    {
                        missedDates.Add(dateTime);
                    }
                }

                if (missedDates.Count != 0)
                {
                    await FindMissedDatesMoex(missedDates, tiker);
                }
                
                foreach (var line in File.ReadAllLines(filePath))
                {
                    var parts = line.Split(';');
                    if (parts.Length != 5)
                        continue;

                    var dateStr = parts[0];
                    if (!DateTime.TryParse(dateStr, out var date))
                        continue;

                    if (!DateTimes.Contains(date))
                        continue;

                    var equityStr = parts[1];
                    if (!decimal.TryParse(equityStr, out var equity))
                        continue;

                    closeMoex.Add(equity);

                }

                for (int i = 0; i < closeMoex.Count; i++)
                {
                    if (i == 0)
                    {
                        equityList.Add(0);
                        daysPl.Add(0);
                        continue;
                    }

                    equityList.Add(Math.Round((closeMoex[i] / closeMoex[0] - 1) * 100, 2));
                    daysPl.Add(Math.Round((closeMoex[i] / closeMoex[i-1] - 1) * 100, 2));
                }

                
            }
            catch (Exception e)
            {
                MessageBox.Show("error: " + e);
            }

        }

        private async Task GetEquitySp()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SP500TR.csv");

            if (!File.Exists(filePath))
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Здесь можно записать данные в файл
                    writer.Write("Дата;Цена;Откр.;Макс.;Мин.;Объём;Изм. %");
                }
            }

            try
            {
                string[] lines = System.IO.File.ReadAllLines(filePath);
                if (lines == null)
                {
                    MessageBox.Show("В файле MCFTR.csv отсутствуют значения");
                    return;
                }

                List<decimal> closeSp = new List<decimal>();

                List<DateTime> missedDates = new List<DateTime>();

                foreach (var dateTime in DateTimes)
                {
                    bool isFound = false;
                    foreach (var line in lines)
                    {
                        var parts = line.Split(';');
                        if (parts.Length < 6) // если строка не содержит нужное количество столбцов - пропускаем ее
                        {
                            continue;
                        }
                        DateTime date;
                        if (!DateTime.TryParseExact(parts[0], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            // если столбец не содержит корректную дату - пропускаем его
                            continue;
                        }
                        if (date == dateTime)
                        {
                            isFound = true;
                            break;
                        }
                    }
                    if (!isFound)
                    {
                        missedDates.Add(dateTime);
                    }
                }

                if (missedDates.Count != 0)
                {
                    await FindMissedDatesSp(missedDates);
                }

                foreach (var line in File.ReadAllLines(filePath))
                {
                    var parts = line.Split(';');
                    if (parts.Length != 7)
                        continue;

                    var dateStr = parts[0];
                    if (!DateTime.TryParse(dateStr, out var date))
                        continue;

                    if (!DateTimes.Contains(date))
                        continue;

                    var equityStr = parts[1].Replace(',', '.');
                    if (!decimal.TryParse(equityStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var equity))
                        continue;

                    closeSp.Add(equity);

                }

                for (int i = 0; i < closeSp.Count; i++)
                {
                    if (i == 0)
                    {
                        EquitySp.Add(0);
                        DaysSpPL.Add(0);
                        continue;
                    }

                    EquitySp.Add(Math.Round((closeSp[i] / closeSp[0] - 1) * 100, 2));
                    DaysSpPL.Add(Math.Round((closeSp[i] / closeSp[i-1] - 1) * 100, 2));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("error: " + e);
            }

        }

        private static async Task FindMissedDatesMoex(List<DateTime> missedDates, string tiker)
        {
            int partedSize = 15;
            List<List<DateTime>> parted = new List<List<DateTime>>();

            for (int i = 0; i < missedDates.Count; i += partedSize)
            {
                List<DateTime> chunk = missedDates.Skip(i).Take(partedSize).ToList();
                parted.Add(chunk);
            }

            foreach (var part in parted)
            {
                DateTime min = part.Min();
                DateTime max = part.Max();

                int iterationCount = 0;

                try
                {
                    string fileName = $"{tiker} .csv";

                    var url = new Uri($"http://iss.moex.com/iss/history/engines/stock/markets/index/sessions/total/securities/{tiker}.csv");

                    var query = $"?from={min.AddDays(-25).ToString("yyyy-MM-dd")}&till={max.ToString("yyyy-MM-dd")}";

                    var fullUrl = new Uri(url, query);

                    var response = await client.GetAsync(fullUrl);
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);// регистрация провайдера кодировок, иначе csv с кодировкой windows-1251 не распознается
                    using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync(), Encoding.GetEncoding("windows-1251")))
                    {
                        string fileContent = await reader.ReadToEndAsync();

                        foreach (var date in part)
                        {
                            List<decimal> ohlc = SearchIndexForDate(date);

                            if (ohlc != null)
                            {
                                string newLine = Environment.NewLine + $"{date:dd.MM.yyyy};{ohlc[3].ToString("0.##")};{ohlc[0].ToString("0.##")};{ohlc[1].ToString("0.##")};{ohlc[2].ToString("0.##")}";

                                using (StreamWriter writer = File.AppendText(fileName))
                                {
                                    writer.Write(newLine);
                                }
                            }
                            
                        }

                        List<decimal> SearchIndexForDate(DateTime date)
                        {
                            string pattern = @".*;" + tiker + @";" + date.ToString("yyyy-MM-dd") + @";.*;.*;(\d+\.\d+);(\d+\.\d+);(\d+\.\d+);(\d+\.\d+);";

                            Match match = Regex.Match(fileContent, pattern);

                            if (match.Success)
                            {
                                // Вывести значение индекса
                                List<decimal> ohlc = new List<decimal>();
                                NumberFormatInfo numberFormatInfo = new NumberFormatInfo()
                                    { NumberDecimalSeparator = "." };

                                ohlc.Add(decimal.Parse((match.Groups[2].Value), numberFormatInfo));
                                ohlc.Add(decimal.Parse((match.Groups[3].Value), numberFormatInfo));
                                ohlc.Add(decimal.Parse((match.Groups[4].Value), numberFormatInfo));
                                ohlc.Add(decimal.Parse((match.Groups[1].Value), numberFormatInfo));

                                return ohlc;
                            }
                            else
                            {
                                if (iterationCount < 100)
                                {
                                    iterationCount++;
                                    return SearchIndexForDate(date.AddDays(-1));
                                }
                                else
                                {
                                    throw new Exception($"Отсутствуют значения индекса {tiker} на указанные даты");
                                    return null;
                                }
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }
            }
        }

        private static async Task FindMissedDatesSp(List<DateTime> missedDates)
        {
            int partedSize = 15;
            List<List<DateTime>> parted = new List<List<DateTime>>();

            for (int i = 0; i < missedDates.Count; i += partedSize)
            {
                List<DateTime> chunk = missedDates.Skip(i).Take(partedSize).ToList();
                parted.Add(chunk);
            }

            foreach (var part in parted)
            {
                DateTime min = part.Min();
                DateTime max = part.Max();

                try
                {
                    // Создаем URL для запроса
                    string url =
                        $"https://finance.yahoo.com/quote/%5ESP500TR/history?period1={ToUnixTimestamp(min.AddDays(-4))}&period2={ToUnixTimestamp(max.AddDays(4))}&interval=1d&filter=history&frequency=1d";

                    HttpResponseMessage response = await client.GetAsync(url);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    string fileName = "SP500TR.csv";

                    foreach (var date in part)
                    {
                        List<double> ohlc = SearchIndexForDate(date);

                        string newLine = Environment.NewLine + $"{date:dd.MM.yyyy};{ohlc[3].ToString("0.##").Replace(".", ",")};{ohlc[0].ToString("0.##").Replace(".", ",")};{ohlc[1].ToString("0.##").Replace(".", ",")};{ohlc[2].ToString("0.##").Replace(".", ",")};;";

                        using (StreamWriter writer = File.AppendText(fileName))
                        {
                            writer.Write(newLine);
                        }
                    }

                    long ToUnixTimestamp(DateTime date)
                    {
                        return (long)(date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                            .TotalSeconds;
                    }

                    List<double> SearchIndexForDate(DateTime date)
                    {
                        string pattern = $"<td class=\"Py\\(10px\\) Ta\\(start\\) Pend\\(10px\\)\"><span>{date.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("en-US"))}" +
                                     $"</span></td><td class=\"Py\\(10px\\) Pstart\\(10px\\)\"><span>(?<open>[0-9,]+.[0-9]+)</span></td><td class=\"Py\\(10px\\) Pstart\\(10px\\)\"><span>(?<high>[0-9,]+.[0-9]+)</span></td>" +
                                     $"<td class=\"Py\\(10px\\) Pstart\\(10px\\)\"><span>(?<low>[0-9,]+.[0-9]+)</span></td><td class=\"Py\\(10px\\) Pstart\\(10px\\)\"><span>(?<close>[0-9,]+.[0-9]+)</span></td>";
                        Match match = Regex.Match(responseBody, pattern);

                        if (match.Success)
                        {
                            // Вывести значение индекса
                            List<double> ohlc = new List<double>();

                            ohlc.Add(double.Parse(match.Groups["open"].Value, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US")));
                            ohlc.Add(double.Parse(match.Groups["high"].Value, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US")));
                            ohlc.Add(double.Parse(match.Groups["low"].Value, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US")));
                            ohlc.Add(double.Parse(match.Groups["close"].Value, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US")));

                            return ohlc;
                        }
                        else
                        {
                            return SearchIndexForDate(date.AddDays(-1));
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
               
            }
        }

        private ObservableCollection<DateTime> GetDateTimes()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Equity.txt");

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл Equity.txt не найден!");
                return null;
            }

            ObservableCollection<DateTime> _dtCollection = new ObservableCollection<DateTime>();

            List<decimal> _depo = new List<decimal>();
            List<decimal> _cashFlow = new List<decimal>();

            try
            {
                string[] liStrings = System.IO.File.ReadAllLines(filePath);

                if (liStrings == null)
                {
                    MessageBox.Show("В файле Equity.txt отсутствуют значения");
                    return null;
                }

                
                foreach (string str in liStrings)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        string[] split = str.Split(';');

                        if (!DateTime.TryParse(split[0], out var date))
                            continue;
                        _dtCollection.Add(date);

                        if (!decimal.TryParse(split[1], out var depo))
                            continue;
                        _depo.Add(depo);

                        if (string.IsNullOrEmpty(split[2]))
                            _cashFlow.Add(0);

                        if (!decimal.TryParse(split[2], out var cashFlow))
                            continue;
                        _cashFlow.Add(cashFlow);
                    }
                }

                Equity.Add(0);
                DaysMyPL.Add(0);

                for (int i = 1; i < _depo.Count; i++)
                {
                    decimal sumdepositing = 0;
                    decimal sumwithdrawal = 0;

                    for (int index = 0; index <= i; index++)
                    {
                        if (_cashFlow[index] > 0)
                        {
                            sumdepositing += _cashFlow[index];
                        }
                        else
                        {
                            sumwithdrawal += _cashFlow[index];
                        }
                    }

                    decimal _percent = ((_depo[i] - sumwithdrawal) / (_depo[0] + sumdepositing) - 1)*100;
                    decimal _percentDay = _depo[i] * 100 / (_depo[i - 1] + _cashFlow[i]) - 100;

                    Equity.Add(Math.Round(_percent, 2));
                    DaysMyPL.Add(Math.Round(_percentDay, 2));
                }

                return _dtCollection;
            }
            catch (Exception e)
            {
                MessageBox.Show("error: " + e);
                return null;
            }
        }

        private void Draw(ObservableCollection<decimal> equity, LineSeries _lineSeries)
        {
            if (DateTimes.Count == equity.Count)
            {
                for (int i = 0; i < DateTimes.Count; i++)
                {
                    DateTime date = DateTimes[i];
                    double x = date.ToOADate();
                    decimal y = equity[i];
                    DataPoint point = new DataPoint(x, (double)y);
                    _lineSeries.Points.Add(point);
                }

                var xAxis = new DateTimeAxis
                {
                    StringFormat = "dd.MM.yyyy",
                    Position = AxisPosition.Bottom,
                    Minimum = DateTimes.First().ToOADate(),
                    Maximum = DateTimes.Last().ToOADate(),
                    Title = "Date"
                };
                Model.Legends.Add(new Legend()
                {
                    LegendBackground = OxyColor.FromAColor(220, OxyColors.White),
                    LegendBorder = OxyColors.Black,
                    LegendBorderThickness = 1.0,
                    LegendPlacement = LegendPlacement.Inside,
                    LegendPosition = LegendPosition.LeftTop,
                    LegendOrientation = LegendOrientation.Vertical,
                    LegendLineSpacing = 8,
                    LegendMaxWidth = 1000,
                    LegendFontSize = 12
                });

                Model.Axes.Add(xAxis);
                Model.Series.Add(_lineSeries);
                Model.InvalidatePlot(true);
                OnPropertyChanged(nameof(Model));
            }

            
        }

        private void Calculate(ObservableCollection<decimal> daysPl, ObservableCollection<decimal> equity, string name)
        {
            Datas.Add(new Data());

            Datas[Datas.Count-1].Name = name;

            Datas[Datas.Count - 1].TotalYield = equity[equity.Count - 1];

            decimal peak = 0m, trough = 0m, drawdown = 0m, maxDrawdown = 0m;

            for (int i = 0; i < equity.Count; i++)
            {
                if (equity[i] > peak)
                {
                    peak = equity[i];
                    trough = peak;
                }
                else if (equity[i] < trough)
                {
                    trough = equity[i];
                    drawdown = (peak - trough);
                    if (drawdown > maxDrawdown)
                        maxDrawdown = drawdown;
                }
            }

            Datas[Datas.Count - 1].MaxDrawDown = maxDrawdown;

            if (name == MyEquity || name == MOEX || name == MREDC || name == RUEYBCSTR)
            {
                RiskFreeRate = 8;
            }
            else
            {
                RiskFreeRate = 2;
            }

            Datas[Datas.Count - 1].CAGR = CalcCagr(equity.Last());

            Datas[Datas.Count - 1].SharpeRatio = CalcSharpeRatio(daysPl, equity.Last(), RiskFreeRate);

            Datas[Datas.Count - 1].SortinoRatio = CalcSortinoRatio(daysPl, RiskFreeRate);

            if (Datas.Count == 1)
            {
                Datas[0].MarketCorrelation = 1;
            }
            else
            {
                Datas[Datas.Count - 1].MarketCorrelation = CalcCorrelation(daysPl);
            }


        }

        private decimal CalcCorrelation(ObservableCollection<decimal> rateList)
        {
            List<decimal> rateListMyEquity = new List<decimal>();

            for (int i = 0; i < Equity.Count; i++)
            {
                if (i == 0)
                {
                    rateListMyEquity.Add(Equity[i]);
                    continue;
                }

                rateListMyEquity.Add(Equity[i] - Equity[i - 1]);
            }
            decimal mean1 = rateList.Average();
            decimal mean2 = rateListMyEquity.Average();

            // рассчитываем стандартное отклонение для каждого списка
            decimal stdDev1 = (decimal)Math.Sqrt(rateList.Average(v => Math.Pow((double)(v - mean1), 2)));
            decimal stdDev2 = (decimal)Math.Sqrt(rateListMyEquity.Average(v => Math.Pow((double)(v - mean2), 2)));

            // рассчитываем ковариацию
            decimal covariance = rateList.Zip(rateListMyEquity, (x, y) => (x - mean1) * (y - mean2)).Sum() / rateList.Count;

            // рассчитываем коэффициент корреляции
             return  covariance / (stdDev1 * stdDev2);

        }

        private decimal CalcSharpeRatio(ObservableCollection<decimal> rateList, decimal total, decimal riskFreeRate)
        {
            TimeSpan days = DateTimes.Last() - DateTimes.First();
            decimal daysCount = (decimal)days.TotalDays;

            decimal average = rateList.Average();

            double sumOfSquaresOfDifferences = rateList.Select(val => (double)(val - average) * (double)(val - average)).Sum();
            double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / rateList.Count());

            if (standardDeviation == 0)
            {
                return Decimal.MaxValue;
            }
            else
            {
                return (average - riskFreeRate / 52) / (decimal)standardDeviation * (decimal)Math.Sqrt(52);
            }
            
            
        }

        private decimal CalcSortinoRatio(ObservableCollection<decimal> rateList, decimal riskFreeRate)
        {

            List<decimal> resultRate = new List<decimal>();

            List<decimal> negativeRate = new List<decimal>();

            for (int i = 0; i < rateList.Count; i++)
            {
                if (i == 0) continue;

                resultRate.Add(riskFreeRate/52 - rateList[i]);
                
            }

            for (int i = 0; i < resultRate.Count; i++)
            {
                if (resultRate[i] > 0)
                {
                    negativeRate.Add(resultRate[i] * resultRate[i]);
                }
            }

            decimal volDown = (decimal)Math.Sqrt((double)(negativeRate.Sum()/ resultRate.Count));

            if (volDown == 0)
            {
                return Decimal.MaxValue;
            }
            else
            {
                return (rateList.Skip(1).Average() - riskFreeRate / 52) / volDown * (decimal)Math.Sqrt(52);
            }

            
            
        }

        private double CalcCagr(decimal total)
        {
            TimeSpan days = DateTimes.Last() - DateTimes.First();
            decimal daysCount = (decimal)days.TotalDays;

            double cagr = Math.Pow((double)(total/100 + 1), (double)(1 / (daysCount / 365)));
            return Math.Round((cagr - 1)*100, 2);
        }

        #endregion
    }
}
