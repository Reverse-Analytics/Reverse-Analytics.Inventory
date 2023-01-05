using Inventory.Core;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Input;

namespace Inventory.Modules.Regions.ViewModels
{
    public class SidebarViewModel : BindableBase
    {
        public IRegionManager RegionManager { get; }
        public ICommand NavigateToCommand { get; }

        public SidebarViewModel(IRegionManager regionManager)
        {
            RegionManager = regionManager;
            NavigateToCommand = new DelegateCommand<string>(OnNavigateTo);
        }

        private void OnNavigateTo(string viewName)
        {
            if (string.IsNullOrEmpty(viewName)) return;

            RegionManager.RequestNavigate(RegionNames.ContentRegion, viewName);
        }
    }
}
