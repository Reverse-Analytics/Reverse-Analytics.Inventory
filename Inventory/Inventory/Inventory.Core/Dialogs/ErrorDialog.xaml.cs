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
            if (!string.IsNullOrEmpty(message))
                this.message.Text = message;
        }

        public ErrorDialog(string title, string message)
            : this(message)
        {
            if (!string.IsNullOrEmpty(title))
                this.title.Text = title;
        }
    }
}
