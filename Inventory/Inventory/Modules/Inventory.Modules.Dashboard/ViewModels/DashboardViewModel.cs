using Prism.Mvvm;

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
        }
    }
}
