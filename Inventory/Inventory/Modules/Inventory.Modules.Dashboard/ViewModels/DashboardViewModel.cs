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

            DataPoints.Add(new LineChartModel() { Year = year.AddYears(1), Supplies = 24, Sales = 34 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(2), Supplies = 14, Sales = 24 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(3), Supplies = 25, Sales = 35 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(4), Supplies = 08, Sales = 18 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(5), Supplies = 27, Sales = 37 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(6), Supplies = 34, Sales = 44 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(7), Supplies = 39, Sales = 49 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(8), Supplies = 17, Sales = 27 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(9), Supplies = 24, Sales = 34 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(10), Supplies = 28, Sales = 38 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(11), Supplies = 34, Sales = 44 });
            DataPoints.Add(new LineChartModel() { Year = year.AddYears(12), Supplies = 39, Sales = 49 });
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
        public double Supplies
        {
            get;
            set;
        }
        public double Sales
        {
            get;
            set;
        }
    }
}
