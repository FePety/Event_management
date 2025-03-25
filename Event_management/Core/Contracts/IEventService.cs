using Event_management.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Event_management.Core.Contracts
{
    // Interface defining the contract for event-related operations
    public interface IEventService
    {
        // Asynchronously retrieves a list of events
        Task<ApiResponse<List<Event>>> GetEventsAsync();

        // Asynchronously adds a new event
        Task<BaseResponse> AddEventAsync(Event newEvent);

        // Asynchronously updates an existing event
        Task<BaseResponse> UpdateEventAsync(Event originalEvent, Event updatedEvent);

        // Asynchronously deletes an event by its name
        Task<BaseResponse> DeleteEventAsync(Event eventName);
    }
}
