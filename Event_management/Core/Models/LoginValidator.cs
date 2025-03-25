using System.Text.RegularExpressions;

namespace Event_management.Core.Models
{
    public class LoginValidator
    {
        // Stores validation error messages for email and password.
        // These properties can only be modified within this class.
        public string EmailError { get; private set; }
        public string PasswordError { get; private set; }

        // Validates the provided email address and updates the EmailError if it is invalid.
        public bool ValidateEmail(string email)
        {
            EmailError = string.Empty;

            // Checks if the email field is empty or contains only whitespace.
            if (string.IsNullOrWhiteSpace(email))
            {
                EmailError = "The e-mail field cannot be empty.";
                return false;
            }

            // Ensures the email follows a valid format using a regular expression.
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            
            // Checks if the email does not match the expected format.
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

            // Checks if the password is empty or contains only spaces.
            if (string.IsNullOrWhiteSpace(password))
            {
                PasswordError = "The password field cannot be empty.";
                return false;
            }

            // Checks if the password matches the required pattern:
            // - At least one lowercase letter
            // - At least one uppercase letter
            // - At least one number
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
