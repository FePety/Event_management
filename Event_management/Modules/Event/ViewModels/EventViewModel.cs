using CommunityToolkit.Mvvm.Messaging;
using Event_management.Core.Services;
using Event_management.Modules.Event.Messages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Event_management.Modules.Event.ViewModels
{
    public class EventViewModel : INotifyPropertyChanged
    {
        private readonly MockEventService _eventService = new MockEventService();

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand SaveCommand { get; }

        public EventViewModel()
        {         
            Events = new ObservableCollection<Core.Models.Event>();
            LoadEvents();
            WeakReferenceMessenger.Default.Register<NewEventMessage>(this, (r, message) =>
            {
                AddEvent(message.Value);
            });
        }
        
        public void AddEvent(Core.Models.Event newEvent)
        {
            if (newEvent != null)
            {
                Events.Add(newEvent);
                OnPropertyChanged(nameof(Events));
            }
        }

        private async void LoadEvents()
        {
            var response = await _eventService.GetEventsAsync();

            if(Events != null) Events.Clear();
            foreach (var e in response.Data)
            {
                Events.Add(e);
            }
        }
    }
}
