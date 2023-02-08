using Inventory.Core.Mvvm;
using Prism.Regions;

namespace Inventory.Modules.Customers.ViewModels
{
    public class DebtsViewModel : ViewModelBase, IRegionMemberLifetime
    {
        public bool KeepAlive => false;
    }
}
