using CommunityToolkit.Mvvm.Messaging;
using Event_management.Core.Services;
using Event_management.Modules.Event.Messages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Event_management.Modules.Event.ViewModels
{
    public class EventViewModel : INotifyPropertyChanged
    {
        // Mock service to handle event-related operations
        private readonly MockEventService _eventService = new MockEventService();

        // Access the Events collection with change notification
        private ObservableCollection<Core.Models.Event> _events;
        public ObservableCollection<Core.Models.Event> Events
        {
            get => _events;
            set
            {
                if (_events != value)
                {
                    _events = value;
                    OnPropertyChanged();
                }
            }
        }

        // Event that triggers when a property value changes.
        public event PropertyChangedEventHandler PropertyChanged;

        // Notifies listeners of a property change.
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //The event notifies subscribers when a property changes
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EventViewModel()
        {
            // Initialize the Events collection
            Events = new ObservableCollection<Core.Models.Event>();

            LoadEvents();

            // Register a message handler to listen for NewEventMessage and add the new event when received
            WeakReferenceMessenger.Default.Register<NewEventMessage>(this, (r, message) =>
            {
                AddEvent(message.Value);
            });
        }

        // Adds a new event to the Events collection and notifies that the collection has changed.
        public void AddEvent(Core.Models.Event newEvent)
        {
            // Check if the new event is not null
            if (newEvent != null)
            {
                Events.Add(newEvent); // Add the event to the collection
                OnPropertyChanged(nameof(Events)); // Notify listeners that the Events property has changed
            }
        }

        // Asynchronously retrieves events from the event service and updates the Events collection.
        private async void LoadEvents()
        {
            // Get the events asynchronously from the service
            var response = await _eventService.GetEventsAsync();

            // Clear the existing events if the collection is not null
            if (Events != null) Events.Clear();

            // Add each event from the response to the Events collection
            Events = new ObservableCollection<Core.Models.Event>(response.Data);
        }
    }
}
