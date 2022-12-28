using Prism.Mvvm;

namespace Inventory.ViewModels
{
    public class ShellWindowViewModel : BindableBase
    {
        private string _title = "Reverse Analytics";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ShellWindowViewModel()
        {

        }
    }
}
