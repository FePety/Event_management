using Event_management.Core.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI.Popups;

namespace Event_management.Modules.Event.ViewModels
{
    public class EventViewModel : INotifyPropertyChanged
    {
        private readonly MockEventService _eventService = new MockEventService();
        public ObservableCollection<Core.Models.Event> Events { get; set; } = new ObservableCollection<Core.Models.Event>();

        private string _name;
        private string _location;
        private string _country;
        private int? _capacity;
        private Core.Models.Event _selectedEvent;

        public event PropertyChangedEventHandler PropertyChanged;
        public Core.Models.Event SelectedEvent
        {
            get => _selectedEvent;
            set { _selectedEvent = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(); }
        }

        public string Country
        {
            get => _country;
            set { _country = value; OnPropertyChanged(); }
        }

        public int? Capacity
        {
            get => _capacity;
            set
            {
                if (value < 0) return;
                _capacity = value;
                OnPropertyChanged();
            }
        }

        private bool _isTextBoxEnabled;
        public bool IsTextBoxEnabled
        {
            get => _isTextBoxEnabled;
            set
            {
                if (_isTextBoxEnabled != value)
                {
                    _isTextBoxEnabled = value;
                    OnPropertyChanged(nameof(IsTextBoxEnabled));
                }
            }
        }

        public ICommand SaveCommand { get; }

        public EventViewModel()
        {
            SaveCommand = new RelayCommand(SaveEvent);
            LoadEvents();
        }

        private async void LoadEvents()
        {
            var response = await _eventService.GetEventsAsync();

            Events.Clear();
            foreach (var e in response.Data)
            {
                Events.Add(e);
                Trace.WriteLine("e " + e.Name);
            }
        }

        private async void SaveEvent()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Location))
            {
                await new MessageDialog("A név és a helyszín kötelező!").ShowAsync();
                return;
            }

            var newEvent = new Core.Models.Event
            {
                Name = Name,
                Location = Location,
                Country = Country,
                Capacity = Capacity
            };

            await _eventService.AddEventAsync(newEvent);
            Events.Add(newEvent);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
