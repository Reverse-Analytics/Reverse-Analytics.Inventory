using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace Inventory.Modules.Dashboard.ViewModels
{
    public class DashboardViewModel : BindableBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public DashboardViewModel()
        {
            Message = "View A from Dashboard";
            this.DataPoints = new ObservableCollection<LineChartModel>();
            DateTime year = new DateTime(2005, 5, 1);

            DataPoints.Add(new LineChartModel() { Year = year.AddYears(1), Germany = 24, England = 34 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(2), Germany = 14, England = 24 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(3), Germany = 25, England = 35 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(4), Germany = 08, England = 18 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(5), Germany = 27, England = 37 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(6), Germany = 34, England = 44 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(7), Germany = 39, England = 49 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(8), Germany = 17, England = 27 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(9), Germany = 24, England = 34 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(10), Germany = 28, England = 38 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(11), Germany = 34, England = 44 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(12), Germany = 39, England = 49 });

            this.Performance = new ObservableCollection<AreaChartModel>();
            Performance.Add(new AreaChartModel() { Load = 2005, Server1 = 23, Server2 = 16, Server3 = 5 });
            Performance.Add(new AreaChartModel() { Load = 2006, Server1 = 40, Server2 = 25, Server3 = 13 });
            Performance.Add(new AreaChartModel() { Load = 2007, Server1 = 15, Server2 = 22, Server3 = 43 });
            Performance.Add(new AreaChartModel() { Load = 2008, Server1 = 10, Server2 = 55, Server3 = 25 });
            Performance.Add(new AreaChartModel() { Load = 2009, Server1 = 62, Server2 = 6, Server3 = 39 });
            Performance.Add(new AreaChartModel() { Load = 2010, Server1 = 10, Server2 = 40, Server3 = 19 });
            Performance.Add(new AreaChartModel() { Load = 2011, Server1 = 29, Server2 = 22, Server3 = 59 });
            Performance.Add(new AreaChartModel() { Load = 2012, Server1 = 74, Server2 = 54, Server3 = 40 });
            Performance.Add(new AreaChartModel() { Load = 2013, Server1 = 20, Server2 = 62, Server3 = 45 });

            this.List = new ObservableCollection<SplineChartModel>();
            DateTime yr = new DateTime(2010, 1, 1);
            List.Add(new SplineChartModel() { XValue = "Jan", YValue1 = 37, YValue2 = 41 });
            List.Add(new SplineChartModel() { XValue = "Feb", YValue1 = 37, YValue2 = 45 });
            List.Add(new SplineChartModel() { XValue = "Mar", YValue1 = 39, YValue2 = 48 });
            List.Add(new SplineChartModel() { XValue = "Apr", YValue1 = 43, YValue2 = 52 });
            List.Add(new SplineChartModel() { XValue = "May", YValue1 = 48, YValue2 = 57 });
            List.Add(new SplineChartModel() { XValue = "Jun", YValue1 = 54, YValue2 = 61 });
            List.Add(new SplineChartModel() { XValue = "Jul", YValue1 = 57, YValue2 = 66 });
            List.Add(new SplineChartModel() { XValue = "Aug", YValue1 = 57, YValue2 = 66 });
            List.Add(new SplineChartModel() { XValue = "Sep", YValue1 = 54, YValue2 = 63 });
            List.Add(new SplineChartModel() { XValue = "Oct", YValue1 = 48, YValue2 = 55 });
            List.Add(new SplineChartModel() { XValue = "Nov", YValue1 = 43, YValue2 = 50 });
            List.Add(new SplineChartModel() { XValue = "Dec", YValue1 = 37, YValue2 = 45 });

            this.Products = new ObservableCollection<SplineRangeAreaModel>();
            Products.Add(new SplineRangeAreaModel() { Month = "Jan", LowMetric = 20, HighMetric = 53 });
            Products.Add(new SplineRangeAreaModel() { Month = "Feb", LowMetric = 22, HighMetric = 76 });
            Products.Add(new SplineRangeAreaModel() { Month = "Mar", LowMetric = 30, HighMetric = 80 });
            Products.Add(new SplineRangeAreaModel() { Month = "Apr", LowMetric = 26, HighMetric = 50 });
            Products.Add(new SplineRangeAreaModel() { Month = "May", LowMetric = 36, HighMetric = 68 });
            Products.Add(new SplineRangeAreaModel() { Month = "Jun", LowMetric = 20, HighMetric = 90 });
            Products.Add(new SplineRangeAreaModel() { Month = "Jul", LowMetric = 40, HighMetric = 73 });
            Products.Add(new SplineRangeAreaModel() { Month = "Aug", LowMetric = 22, HighMetric = 76 });
            Products.Add(new SplineRangeAreaModel() { Month = "Sep", LowMetric = 30, HighMetric = 80 });
            Products.Add(new SplineRangeAreaModel() { Month = "Oct", LowMetric = 26, HighMetric = 50 });
            Products.Add(new SplineRangeAreaModel() { Month = "Nov", LowMetric = 36, HighMetric = 68 });
            Products.Add(new SplineRangeAreaModel() { Month = "Dec", LowMetric = 20, HighMetric = 90 });
        }

        public ObservableCollection<SplineRangeAreaModel> Products { get; set; }

        public ObservableCollection<SplineChartModel> List
        {
            get;
            set;
        }

        public ObservableCollection<AreaChartModel> Performance
        {
            get;
            set;
        }

        public ObservableCollection<LineChartModel> DataPoints
        {
            get;
            set;
        }
    }

    public class LineChartModel
    {
        public DateTime Year
        {
            get;
            set;
        }
        public double Germany
        {
            get;
            set;
        }
        public double England
        {
            get;
            set;
        }
    }

    public class SplineChartModel
    {
        public string XValue { get; set; }

        public double YValue1 { get; set; }
        public double YValue2 { get; set; }
    }

    public class SplineAreaChartModel
    {
        public string ProdName { get; set; }
        public double Price { get; set; }
        public double Stock { get; set; }
    }

    public class AreaChartModel
    {
        public double Server1
        {
            get;
            set;
        }

        public double Server2
        {
            get;
            set;
        }

        public double Server3
        {
            get;
            set;
        }

        public double Load
        {
            get;
            set;
        }
    }

    public class SplineRangeAreaModel
    {
        public string Month { get; set; }
        public double LowMetric { get; set; }
        public double HighMetric { get; set; }
    }

}
