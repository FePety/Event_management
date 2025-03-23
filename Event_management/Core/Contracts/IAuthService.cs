using Event_management.Core.Models;
using System.Threading.Tasks;

namespace Event_management.Core.Contracts
{
    public interface IAuthService
    {
        Task<bool> IsUserLoggedInAsync();
        Task<BaseResponse> LoginAsync(string username, string password);
        Task<BaseResponse> LogoutAsync();
    }
}
