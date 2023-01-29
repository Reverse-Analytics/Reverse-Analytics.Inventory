using System.Windows.Controls;

namespace Inventory.Core.Dialogs
{
    /// <summary>
    /// Interaction logic for SuccessDialog.xaml
    /// </summary>
    public partial class SuccessDialog : UserControl
    {
        public SuccessDialog()
        {
            InitializeComponent();
        }

        public SuccessDialog(string message)
            : this()
        {
            if (!string.IsNullOrEmpty(message))
                this.message.Text = message;
        }

        public SuccessDialog(string title, string message)
            : this(message)
        {
            if (!string.IsNullOrEmpty(title))
                this.title.Text = title;
        }
    }
}
