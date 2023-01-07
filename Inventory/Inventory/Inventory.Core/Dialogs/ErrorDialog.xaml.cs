using System.Windows.Controls;

namespace Inventory.Core.Dialogs
{
    /// <summary>
    /// Interaction logic for ErrorDialog.xaml
    /// </summary>
    public partial class ErrorDialog : UserControl
    {
        public ErrorDialog()
        {
            InitializeComponent();
        }

        public ErrorDialog(string message)
            : this()
        {
            this.description.Text = message;
        }

        public ErrorDialog(string title, string message)
            : this(message)
        {
            this.title.Text = title;
        }
    }
}
