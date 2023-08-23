using System.Globalization;
using System.Windows.Controls;

namespace Inventory.Core.ValidationRules
{
    internal class PhoneNumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return IsPhoneNumberValid(value) ?
                ValidationResult.ValidResult :
                new ValidationResult(false, "Invalid phone format.");
        }

        private bool IsPhoneNumberValid(object value)
        {
            if (value is not string phoneNumber)
            {
                return false;
            }

            if (phoneNumber.Length != 14)
            {
                return false;
            }

            return true;
        }
    }
}
