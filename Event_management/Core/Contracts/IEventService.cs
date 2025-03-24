using Event_management.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Event_management.Core.Contracts
{
    public interface IEventService
    {
        Task<ApiResponse<List<Event>>> GetEventsAsync();
        Task<BaseResponse> AddEventAsync(Event newEvent);
        Task<BaseResponse> UpdateEventAsync(Event updatedEvent);
        Task<BaseResponse> DeleteEventAsync(string eventName);
    }
}
