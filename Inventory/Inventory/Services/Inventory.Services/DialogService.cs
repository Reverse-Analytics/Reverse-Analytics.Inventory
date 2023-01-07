using Inventory.Core.Dialogs;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using System.Windows.Forms;

namespace Inventory.Services
{
    public class DialogService : IDialogService
    {
        public async Task<bool> ShowConfirmation()
        {
            var dialog = new ConfirmationDialog();

            var result = await DialogHost.Show(dialog, "RootDialog");

            if (result is bool boolean)
            {
                return boolean;
            }

            return false;
        }

        public async Task<bool> ShowConfirmation(string title)
        {
            var dialog = new ConfirmationDialog(title);

            var result = await DialogHost.Show(dialog, "RootDialog");

            if (result is bool boolean)
            {
                return boolean;
            }

            return false;
        }

        public async Task<bool> ShowConfirmation(string title, string message)
        {
            var dialog = new ConfirmationDialog(title, message);

            var result = await DialogHost.Show(dialog, "RootDialog");

            if(result is bool boolean)
            {
                return boolean;
            }

            return false;
        }

        public async void ShowError()
        {
            var dialog = new ErrorDialog();

            await DialogHost.Show(dialog);
        }

        public async void ShowError(string message)
        {
            var dialog = new ErrorDialog(message);

            await DialogHost.Show(dialog);
        }

        public async void ShowError(string title, string message)
        {
            var dialog = new ErrorDialog(title, message);

            await DialogHost.Show(dialog);
        }

        public void ShowError(string title, string message, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
