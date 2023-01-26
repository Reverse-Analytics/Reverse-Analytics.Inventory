using System.Windows;
using System.Windows.Media.Effects;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShellWindow : Window
    {
        public ShellWindow()
        {
            InitializeComponent();
        }

        private void DialogHost_DialogClosed(object sender, MaterialDesignThemes.Wpf.DialogClosedEventArgs eventArgs)
        {
            sideBarRegion.IsEnabled= true;
        }

        private void DialogHost_DialogOpened(object sender, MaterialDesignThemes.Wpf.DialogOpenedEventArgs eventArgs)
        {
            sideBarRegion.IsEnabled = true;
        }
    }
}
