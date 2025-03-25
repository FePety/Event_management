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
        // A mock list of events to simulate database or API data
        private List<Event> _events = new List<Event>
        {
            new Event { Name = "Startup Conference", Location = "San Francisco", Country = "USA", Capacity = 300 },
            new Event { Name = "Gaming Expo", Location = "Tokyo", Country = "Japan", Capacity = 10000 },
        };

        public async Task<ApiResponse<List<Event>>> GetEventsAsync()
        {
            // Return the list of mock events wrapped in an API response
            return new ApiResponse<List<Event>>(_events);
        }

        public async Task<BaseResponse> AddEventAsync(Event newEvent)
        {
            var response = new BaseResponse();

            // Check if the event name or location is missing
            if (string.IsNullOrWhiteSpace(newEvent.Name) || string.IsNullOrWhiteSpace(newEvent.Location))
            {
                // Return an error response if data is invalid
                response.AddError("INVALID_DATA", "Name and location cannot be empty!", "Fill in all fields.");
                return response;
            }

            // Add the new event to the list
            _events.Add(newEvent);

            // Return a success response with event details
            response.AddInfo("EVENT_ADDED", "Event added successfully!", $"Event: {newEvent.Name}");
            return response;
        }

        public async Task<BaseResponse> UpdateEventAsync(Event originalEvent, Event updatedEvent)
        {
            var response = new ApiResponse<Event>(null);

            // Find the event that matches the original event details
            var existingEvent = originalEvent == null
                ? null : _events.FirstOrDefault(e =>
                    e.Name == originalEvent.Name &&
                    e.Location == originalEvent.Location &&
                    e.Country == originalEvent.Country &&
                    e.Capacity == originalEvent.Capacity);

            // If no matching event is found, return an error response
            if (existingEvent == null)
            {
                response.AddError("EVENT_NOT_FOUND", "Event not found!", "Check the event name.");
                return response;
            }

            // Update the event properties with the new values
            existingEvent.Name = updatedEvent.Name;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.Country = updatedEvent.Country;
            existingEvent.Capacity = updatedEvent.Capacity;

            // Set the updated event in the response and return success message
            response.Data = existingEvent;
            response.AddInfo("EVENT_UPDATED", "Event successfully updated!", $"Event: {updatedEvent.Name}");

            return response;
        }

        public async Task<BaseResponse> DeleteEventAsync(Event eventToDelete)
        {
            var response = new BaseResponse();

            // Find the event to delete by matching its details
            var eventToRemove = _events.FirstOrDefault(e =>
                e.Name == eventToDelete.Name &&
                e.Location == eventToDelete.Location &&
                e.Country == eventToDelete.Country &&
                e.Capacity == eventToDelete.Capacity);

            // If no matching event is found, return an error response
            if (eventToRemove == null)
            {
                response.AddError("EVENT_NOT_FOUND", "Event not found!", "Check the event details.");
                return response;
            }

            // Remove the event from the list
            _events.Remove(eventToRemove);

            // Return a success response with event details
            response.AddInfo("EVENT_DELETED", "Event successfully deleted!", $"Event: {eventToDelete.Name}");
            return response;
        }
    }
}
