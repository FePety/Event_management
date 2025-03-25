using Event_management.Core.Contracts;
using Event_management.Core.Models;
using System.Linq;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace Event_management.Core.Services
{
    // Mock implementation of the authentication service for testing purposes
    public class MockAuthService : IAuthService
    {
        // Constant representing the resource name used in the password vault
        private const string VAULT_RESOURCE = "EventManagementApp";

        // Password vault for securely storing user credentials
        private readonly PasswordVault _vault;

        // Constructor initializes the password vault
        public MockAuthService()
        {
            _vault = new PasswordVault();
        }

        // Checks if the user is logged in by verifying stored credentials
        public async Task<bool> IsUserLoggedInAsync()
        {
            return GetStoredCredentials() != null;
        }

        // Asynchronously attempts to log in a user with the provided credentials
        public async Task<BaseResponse> LoginAsync(string username, string password)
        {
            var response = new BaseResponse();

            // Check if either the username or password field is empty
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                // Add an error response indicating that the fields cannot be empty
                response.AddError("EMPTY_FIELDS",
                    "Username and password cannot be empty.",
                    "Please enter your username and password and try again.");
                return response;
            }

            // Hard-coded username and password
            bool isValid = (username == "mxc@mxc.hu" && password == "Mxc123");

            if (isValid)
            {
                // Store credentials for future authentication
                StoreCredentials(username, password);
                response.AddInfo("LOGIN_SUCCESS",
                    "Login Successful!",
                    "The user has successfully logged in.");
            }
            else
            {
                // Return an error if authentication fails
                response.AddError("INVALID_CREDENTIALS",
                    "Incorrect username or password!",
                    "Please check the information you entered.");
            }

            return response;
        }

        public async Task<BaseResponse> LogoutAsync()
        {
            // Clear stored credentials upon logout
            ClearCredentials();

            // Create a response indicating successful logout
            var response = new BaseResponse();
            response.AddInfo("LOGOUT_SUCCESS",
                "Logout successful!",
                "The user has logged out of the application.");

            return response;
        }

        private void StoreCredentials(string username, string password)
        {
            // Clear any existing stored credentials to avoid duplicates
            ClearCredentials();

            // Store the new credentials securely in the password vault
            _vault.Add(new PasswordCredential(
                VAULT_RESOURCE,
                username,
                password
            ));
        }

        private PasswordCredential GetStoredCredentials()
        {
            try
            {
                // Retrieve the first stored credential for the given resource
                return _vault.FindAllByResource(VAULT_RESOURCE)
                    .FirstOrDefault();
            }
            catch
            {
                // Return null if no credentials are found or an exception occurs
                return null;
            }
        }

        private void ClearCredentials()
        {
            // Retrieve stored credentials
            var creds = GetStoredCredentials();

            // If credentials exist, remove them from the vault
            if (creds != null)
            {
                _vault.Remove(creds);
            }
        }
    }
}
