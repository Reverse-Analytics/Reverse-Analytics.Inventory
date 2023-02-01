using Inventory.Core;
using Inventory.Core.Dialogs;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;

namespace Inventory.Services
{
    public class DialogService : IDialogService
    {
        public async Task<bool> ShowConfirmation(string title = "", string message = "")
        {
            CloseDialogSessionIfActive(RegionNames.NotificationRegion);

            var confirmationDialog = new ConfirmationDialog(title: title, message: message);

            var dialogResult = await DialogHost.Show(confirmationDialog, RegionNames.NotificationRegion);

            if (dialogResult is bool result)
            {
                return result;
            }

            return false;
        }

        public async Task ShowError(string title = "", string message = "")
        {
            CloseDialogSessionIfActive(RegionNames.NotificationRegion);

            var errorDialog = new ErrorDialog(title: title, message: message);

            await DialogHost.Show(errorDialog, RegionNames.NotificationRegion);
        }

        public async Task ShowSuccess(string title = "", string message = "")
        {
            CloseDialogSessionIfActive(RegionNames.NotificationRegion);

            var successDialog = new SuccessDialog(title: title, message: message);

            await DialogHost.Show(successDialog, RegionNames.NotificationRegion);
        }

        private static void CloseDialogSessionIfActive(string dialogRegion)
        {
            if (DialogHost.IsDialogOpen(dialogRegion))
            {
                DialogHost.Close(dialogRegion);
            }
        }
    }
}
