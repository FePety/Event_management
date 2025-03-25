using Event_management.Core.Contracts;
using Event_management.Core.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
using CommunityToolkit.Mvvm.Messaging;
using Event_management.Modules.Event.Messages;

namespace Event_management.Modules.Event.ViewModels
{
    public class EventPanelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly IEventService _eventService;
        private string _responseMessage;
        private string _nameError;
        private string _locationError;
        private string _capacityError;
        private double _nameErrorOpacity;
        private double _locationErrorOpacity;
        private double _capacityErrorOpacity;
        private readonly EventValidator _validator;
        private Core.Models.Event _originalEvent;

        public RelayCommand SaveCommand { get; private set; }

        public EventPanelViewModel(IEventService eventService)
        {

            _eventService = eventService;
            _validator = new EventValidator();
            SaveCommand = new RelayCommand(async () => await SaveOrUpdateEvent(), CanSave);
        }
        
        private Core.Models.Event _updatedEvent;
        public Core.Models.Event UpdatedEvent
        {
            get => _updatedEvent;
            set
            {
                if (_updatedEvent != value)
                {
                    _updatedEvent = value;
                    OnPropertyChanged();
                }
            }
        }

        public Core.Models.Event OriginalEvent
        {
            get => _originalEvent;
            set
            {
                if (_originalEvent != value)
                {
                    _originalEvent = value;
                    IsTextBoxEnabled = false;
                    ResponseMessageVisible = Visibility.Collapsed;
                    NameErrorOpacity = 0;
                    LocationErrorOpacity = 0;
                    CapacityErrorOpacity = 0;

                    OnPropertyChanged();
                    UpdatedEvent = new Core.Models.Event(
                                    name: _originalEvent?.Name,
                                    location: _originalEvent?.Location,
                                    country: _originalEvent?.Country,
                                    capacity: _originalEvent?.Capacity ?? 0
                                );

                    OnPropertyChanged(nameof(Name));
                    OnPropertyChanged(nameof(Location));
                    OnPropertyChanged(nameof(Country));
                    OnPropertyChanged(nameof(Capacity));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Name
        {
            get => UpdatedEvent?.Name;
            set
            {
                if (UpdatedEvent != null && UpdatedEvent.Name != value)
                {
                    UpdatedEvent.Name = value;
                    OnPropertyChanged();
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Location
        {
            get => UpdatedEvent?.Location;
            set
            {
                if (UpdatedEvent != null && UpdatedEvent.Location != value)
                {
                    UpdatedEvent.Location = value;
                    OnPropertyChanged();
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Country
        {
            get => UpdatedEvent?.Country;
            set
            {
                if (UpdatedEvent != null && UpdatedEvent.Country != value)
                {
                    UpdatedEvent.Country = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Capacity
        {
            get => UpdatedEvent?.Capacity ?? 0;
            set
            {
                if (UpdatedEvent != null && UpdatedEvent.Capacity != (int)value)
                {
                    UpdatedEvent.Capacity = (int)value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isNewEvent;
        public bool IsNewEvent
        {
            get => _isNewEvent;
            set
            {
                if (_isNewEvent != value)
                {
                    _isNewEvent = value;
                    OnPropertyChanged();
                }
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
                    OnPropertyChanged(nameof(SavedButtonVisible));
                    OnPropertyChanged(nameof(IsTextBoxEnabled));
                }
            }
        }
        public Visibility SavedButtonVisible => IsTextBoxEnabled ? Visibility.Visible : Visibility.Collapsed;

        public string ResponseMessage
        {
            get => _responseMessage;
            set
            {
                _responseMessage = value;
                OnPropertyChanged();
            }
        }

        private Visibility _responseMessageVisible = Visibility.Collapsed;
        public Visibility ResponseMessageVisible
        {
            get => _responseMessageVisible;
            set
            {
                if (_responseMessageVisible != value)
                {
                    _responseMessageVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        private SolidColorBrush _responseTextColor;
        public SolidColorBrush ResponseTextColor
        {
            get => _responseTextColor;
            set
            {
                _responseTextColor = value;
                OnPropertyChanged();
            }
        }

        public string NameError
        {
            get => _nameError;
            private set
            {
                if (_nameError != value)
                {
                    _nameError = value;
                    OnPropertyChanged();
                    NameErrorOpacity = string.IsNullOrEmpty(_nameError) ? 0 : 1;
                }
            }
        }

        public string LocationError
        {
            get => _locationError;
            private set
            {
                if (_locationError != value)
                {
                    _locationError = value;
                    OnPropertyChanged();
                    LocationErrorOpacity = string.IsNullOrEmpty(_locationError) ? 0 : 1;
                }
            }
        }

        public string CapacityError
        {
            get => _capacityError;
            private set
            {
                if (_capacityError != value)
                {
                    _capacityError = value;
                    OnPropertyChanged();
                    CapacityErrorOpacity = string.IsNullOrEmpty(_capacityError) ? 0 : 1;
                }
            }
        }

        public double NameErrorOpacity
        {
            get => _nameErrorOpacity;
            private set
            {
                if (_nameErrorOpacity != value)
                {
                    _nameErrorOpacity = value;
                    OnPropertyChanged();
                }
            }
        }

        public double LocationErrorOpacity
        {
            get => _locationErrorOpacity;
            private set
            {
                if (_locationErrorOpacity != value)
                {
                    _locationErrorOpacity = value;
                    OnPropertyChanged();
                }
            }
        }

        public double CapacityErrorOpacity
        {
            get => _capacityErrorOpacity;
            private set
            {
                if (_capacityErrorOpacity != value)
                {
                    _capacityErrorOpacity = value;
                    OnPropertyChanged();
                }
            }
        }

        public void ResetEventForm()
        {
            IsNewEvent = true;
            UpdatedEvent = new Core.Models.Event();
            IsTextBoxEnabled = true;
            Name = "";
            Location = "";
            Country = "";
            Capacity = 1;
        }

        private async Task SaveOrUpdateEvent()
        {
            if (IsNewEvent)
            {
                await AddEventAsync();
            }
            else
            {
                await UpdateEventAsync();
            }
        }
        
        private async Task AddEventAsync()
        {
            if (UpdatedEvent == null)
                return;

            ResponseMessage = string.Empty;

            bool isValid = _validator.ValidateAll(Name, Location, (int?)Capacity);

            if (!isValid)
            {
                NameError = _validator.NameError;
                LocationError = _validator.LocationError;
                CapacityError = _validator.CapacityError;
                return;
            }

             App.GlobalLoader.SetTimer(2000);

            var response = await _eventService.AddEventAsync(UpdatedEvent);
            if (response?.Error?.Count > 0)
            {
                ResponseMessage = response.Error[0].Message;
                ResponseTextColor = new SolidColorBrush(Colors.Red);
            }
            else
            {
                ResponseMessage = response.Info[0].Message;
                ResponseTextColor = new SolidColorBrush(Colors.Green);
                IsTextBoxEnabled = false;
                WeakReferenceMessenger.Default.Send(new NewEventMessage(UpdatedEvent));
            }
            ResponseMessageVisible = Visibility.Visible;
        }

        private async Task UpdateEventAsync()
        {
            if (OriginalEvent == null)
                return;

            ResponseMessage = string.Empty;

            bool isValid = _validator.ValidateAll(Name, Location, (int?)Capacity);

            if (!isValid)
            {
                NameError = _validator.NameError;
                LocationError = _validator.LocationError;
                CapacityError = _validator.CapacityError;
                return;
            }

             App.GlobalLoader.SetTimer(2000);

            var response = await _eventService.UpdateEventAsync(OriginalEvent, UpdatedEvent);
            if (response?.Error?.Count > 0)
            {
                ResponseMessage = response.Error[0].Message;
                ResponseTextColor = new SolidColorBrush(Colors.Red);
            }
            else
            {
                if (response is ApiResponse<Core.Models.Event> apiResponse)
                {
                    ApplyChanges(apiResponse.Data);
                    ResponseMessage = response.Info[0].Message;
                    ResponseTextColor = new SolidColorBrush(Colors.Green);
                    IsTextBoxEnabled = false;
                }
            }
            ResponseMessageVisible = Visibility.Visible;

        }

        private void ApplyChanges(Core.Models.Event data)
        {
            if (data == null || OriginalEvent == null) return;

            foreach (var prop in typeof(Core.Models.Event).GetProperties())
            {
                if (prop.CanWrite)
                {
                    prop.SetValue(OriginalEvent, prop.GetValue(data));
                }
            }
        }

        private bool CanSave()
        {
            return true;
        }
    }
}
