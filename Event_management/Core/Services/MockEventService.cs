using Event_management.Core.Contracts;
using Event_management.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_management.Core.Services
{
    public class MockEventService : IEventService
    {
        private List<Event> _events = new List<Event>
        {
            new Event { Name = "Startup Conference", Location = "San Francisco", Country = "USA", Capacity = 300 },
            new Event { Name = "Gaming Expo", Location = "Tokyo", Country = "Japan", Capacity = 10000 },
        };

        public async Task<ApiResponse<List<Event>>> GetEventsAsync()
        {
            await Task.Delay(300);
            return new ApiResponse<List<Event>>(_events);
        }

        public async Task<BaseResponse> AddEventAsync(Event newEvent)
        {
            await Task.Delay(300);
            var response = new BaseResponse();

            if (string.IsNullOrWhiteSpace(newEvent.Name) || string.IsNullOrWhiteSpace(newEvent.Location))
            {
                response.AddError("INVALID_DATA", "Name and location cannot be empty!", "Fill in all fields.");
                return response;
            }

            _events.Add(newEvent);
            response.AddInfo("EVENT_ADDED", "Event added successfully!", $"Event: {newEvent.Name}");
            return response;
        }

        public async Task<BaseResponse> UpdateEventAsync(Event originalEvent, Event updatedEvent)
        {
            await Task.Delay(300);

            var response = new ApiResponse<Event>(null);

            var existingEvent = originalEvent == null
                ? null : _events.FirstOrDefault(e =>
                    e.Name == originalEvent.Name &&
                    e.Location == originalEvent.Location &&
                    e.Country == originalEvent.Country &&
                    e.Capacity == originalEvent.Capacity);

            if (existingEvent == null)
            {
                response.AddError("EVENT_NOT_FOUND", "Event not found!", "Check the event name.");
                return response;
            }

            existingEvent.Name = updatedEvent.Name;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.Country = updatedEvent.Country;
            existingEvent.Capacity = updatedEvent.Capacity;

            response.Data = existingEvent;
            response.AddInfo("EVENT_UPDATED", "Event successfully updated!", $"Event: {updatedEvent.Name}");

            return response;
        }

        public async Task<BaseResponse> DeleteEventAsync(string eventName)
        {
            await Task.Delay(300);
            var response = new BaseResponse();

            var eventToDelete = _events.FirstOrDefault(e => e.Name == eventName);
            if (eventToDelete == null)
            {
                response.AddError("EVENT_NOT_FOUND", "Event not found!", "Check the event name.");
                return response;
            }

            _events.Remove(eventToDelete);
            response.AddInfo("EVENT_DELETED", "Event successfully deleted!", $"Event: {eventName}");
            return response;
        }
    }
}
