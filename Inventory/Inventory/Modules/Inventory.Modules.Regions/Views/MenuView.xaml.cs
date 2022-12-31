using System.Windows;
using System.Windows.Controls;

namespace Inventory.Modules.Regions.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var mw = Window.GetWindow(this);
            mw.Close();
        }
    }
}
