using System.Windows.Controls;

namespace Inventory.Core.Dialogs
{
    /// <summary>
    /// Interaction logic for ConfirmationDialog.xaml
    /// </summary>
    public partial class ConfirmationDialog : UserControl
    {
        public ConfirmationDialog()
        {
            InitializeComponent();
        }

        public ConfirmationDialog(string message)
            : this()
        {
            this.description.Text = message;
        }

        public ConfirmationDialog(string title, string message)
            : this(message)
        {
            this.title.Text = title;
        }
    }
}
