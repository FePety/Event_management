using Event_management.Core.Contracts;
using Event_management.Core.Models;
using System.Linq;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace Event_management.Core.Services
{
    public class MockAuthService : IAuthService
    {
        private const string VAULT_RESOURCE = "EventManagementApp";
        private readonly PasswordVault _vault;

        public MockAuthService()
        {
            _vault = new PasswordVault();
        }

        public async Task<bool> IsUserLoggedInAsync()
        {
            await Task.Delay(500);
            return GetStoredCredentials() != null;
        }

        public async Task<BaseResponse> LoginAsync(string username, string password)
        {
            await Task.Delay(500);

            var response = new BaseResponse();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                response.AddError("EMPTY_FIELDS",
                    "Username and password cannot be empty.",
                    "Please enter your username and password and try again.");
                return response;
            }

            bool isValid = (username == "mxc@mxc.hu" && password == "Mxc123");

            if (isValid)
            {
                StoreCredentials(username, password);
                response.AddInfo("LOGIN_SUCCESS",
                    "Login Successful!",
                    "The user has successfully logged in.");
            }
            else
            {
                response.AddError("INVALID_CREDENTIALS",
                    "Incorrect username or password!",
                    "Please check the information you entered.");
            }

            return response;
        }

        public async Task<BaseResponse> LogoutAsync()
        {
            await Task.Delay(500);
            ClearCredentials();

            var response = new BaseResponse();
            response.AddInfo("LOGOUT_SUCCESS",
                "Logout successful!",
                "The user has logged out of the application.");

            return response;
        }

        private void StoreCredentials(string username, string password)
        {
            ClearCredentials();
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
                return _vault.FindAllByResource(VAULT_RESOURCE)
                    .FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        private void ClearCredentials()
        {
            var creds = GetStoredCredentials();
            if (creds != null)
            {
                _vault.Remove(creds);
            }
        }
    }
}
