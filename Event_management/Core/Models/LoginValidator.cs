using System.Text.RegularExpressions;

namespace Event_management.Core.Models
{
    public class LoginValidator
    {
        public string EmailError { get; private set; }
        public string PasswordError { get; private set; }

        public bool ValidateEmail(string email)
        {
            EmailError = string.Empty;

            if (string.IsNullOrWhiteSpace(email))
            {
                EmailError = "The e-mail field cannot be empty.";
                return false;
            }

            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            if (!emailRegex.IsMatch(email))
            {
                EmailError = "Invalid email address format.";
                return false;
            }

            return true;
        }

        public bool ValidatePassword(string password)
        {
            PasswordError = string.Empty;

            if (string.IsNullOrWhiteSpace(password))
            {
                PasswordError = "The password field cannot be empty.";
                return false;
            }

            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$");
            if (!passwordRegex.IsMatch(password))
            {
                PasswordError = "It must contain one lowercase letter, one uppercase letter, and one number.";
                return false;
            }

            return true;
        }
    }


}
