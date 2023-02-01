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
            if (!string.IsNullOrEmpty(message))
            {
                this.description.Text = message;
            }
        }

        public ConfirmationDialog(string title, string message)
            : this(message)
        {
            if (!string.IsNullOrEmpty(title))
            {
                this.title.Text = title;
            }
        }
    }
}
