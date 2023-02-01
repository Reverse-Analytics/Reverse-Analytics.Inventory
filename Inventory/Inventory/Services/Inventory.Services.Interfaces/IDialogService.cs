namespace Inventory.Services.Interfaces
{
    public interface IDialogService
    {
        public Task ShowError(string title = "", string message = "");
        public Task ShowSuccess(string title = "", string message = "");
        public Task<bool> ShowConfirmation(string title = "", string message = "");
    }
}
