using Event_management.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
