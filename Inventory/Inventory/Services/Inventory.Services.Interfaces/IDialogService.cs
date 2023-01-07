namespace Inventory.Services.Interfaces
{
    public interface IDialogService
    {
        public void ShowError();
        public void ShowError(string message);
        public void ShowError(string title, string message);
        public void ShowError(string title, string message, Exception exception);
        public Task<bool> ShowConfirmation();
        public Task<bool> ShowConfirmation(string title);
        public Task<bool> ShowConfirmation(string title, string message);
    }
}
