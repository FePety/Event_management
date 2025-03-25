using Event_management.Core.Models;
using System.Threading.Tasks;

namespace Event_management.Core.Contracts
{
    // Interface defining authentication-related operations
    public interface IAuthService
    {
        // Asynchronously checks if the user is logged in
        Task<bool> IsUserLoggedInAsync();

        // Asynchronously logs in a user with the provided username and password
        Task<BaseResponse> LoginAsync(string username, string password);

        // Asynchronously logs out the current user
        Task<BaseResponse> LogoutAsync();
    }
}
