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
        // Event that triggers when a property value changes.
        public event PropertyChangedEventHandler PropertyChanged;

        // Notifies listeners of a property change.
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //The event notifies subscribers when a property changes
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

        // Command executed to save the user.
        public RelayCommand SaveCommand { get; private set; }

        public EventPanelViewModel(IEventService eventService)
        {
            // Initializes the event service and event validator.
            _eventService = eventService;
            _validator = new EventValidator();

            // Initialize the save command along with its execution and condition methods.
            SaveCommand = new RelayCommand(async () => await SaveOrUpdateEvent(), CanSave);
        }

        // Updatedevent with change notification.
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

        // OriginalEvent with change notification and resetting related properties.
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

        // Updated event name with change notification.
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

        // Updated event location with change notification.
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

        // Updated event country with change notification.
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

        // Updated event capacity with change notification.
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

        // Indicates whether the event is new.
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

        // Flag to control the enabled state of text boxes.
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
        // Visibility of the 'Save' button based on whether text boxes are enabled.
        public Visibility SavedButtonVisible => IsTextBoxEnabled ? Visibility.Visible : Visibility.Collapsed;

        // Message displayed as a response, with change notification.
        public string ResponseMessage
        {
            get => _responseMessage;
            set
            {
                _responseMessage = value;
                OnPropertyChanged();
            }
        }

        // Response message visibility with change notification.
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

        // Response text color with change notification.
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

        // Name error message with change notification and opacity adjustment.
        public string NameError
        {
            get => _nameError;
            private set
            {
                if (_nameError != value)
                {
                    _nameError = value;
                    OnPropertyChanged();
                    // Adjusts the opacity of the name error based on whether an error message is present.
                    NameErrorOpacity = string.IsNullOrEmpty(_nameError) ? 0 : 1;
                }
            }
        }

        // Location error message with change notification and opacity adjustment.
        public string LocationError
        {
            get => _locationError;
            private set
            {
                if (_locationError != value)
                {
                    _locationError = value;
                    OnPropertyChanged();
                    // Adjusts the opacity of the name error based on whether an error message is present.
                    LocationErrorOpacity = string.IsNullOrEmpty(_locationError) ? 0 : 1;
                }
            }
        }

        // Capacity error message with change notification and opacity adjustment.
        public string CapacityError
        {
            get => _capacityError;
            private set
            {
                if (_capacityError != value)
                {
                    _capacityError = value;
                    OnPropertyChanged();
                    // Adjusts the opacity of the name error based on whether an error message is present.
                    CapacityErrorOpacity = string.IsNullOrEmpty(_capacityError) ? 0 : 1;
                }
            }
        }

        // Opacity adjustment for name error based on error message presence.
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

        // Opacity adjustment for location error based on error message presence.
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

        // Opacity adjustment for capacity error based on error message presence.
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

        // Resets the event form to its default state.
        public void ResetEventForm()
        {
            ResponseMessageVisible = Visibility.Collapsed;
            IsNewEvent = true;
            UpdatedEvent = new Core.Models.Event();
            IsTextBoxEnabled = true;
            Name = "";
            Location = "";
            Country = "";
            Capacity = 1;
        }

        // Adds a new event if IsNewEvent is true, otherwise updates the existing event.
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

        // Asynchronous method to create a new event
        private async Task AddEventAsync()
        {
            if (UpdatedEvent == null)
                return;

            ResponseMessage = string.Empty;

            // Validate input fields.
            bool isValid = _validator.ValidateAll(Name, Location, (int?)Capacity);

            if (!isValid)
            {
                // Display validation errors.
                NameError = _validator.NameError;
                LocationError = _validator.LocationError;
                CapacityError = _validator.CapacityError;
                return;
            }

            // Show loader while adding the event and simulate server response delay for 2 seconds
            App.GlobalLoader.SetTimer(2000);

            // Send the new event data to the service for adding and await the response
            var response = await _eventService.AddEventAsync(UpdatedEvent);
            if (response?.Error?.Count > 0)
            {
                // Show error message if there was a problem adding the event.
                ResponseMessage = response.Error[0].Message;
                ResponseTextColor = new SolidColorBrush(Colors.Red);
            }
            else
            {
                // Show success message and disable the text boxes.
                ResponseMessage = response.Info[0].Message;
                ResponseTextColor = new SolidColorBrush(Colors.Green);
                IsTextBoxEnabled = false;

                // Notify other parts of the application about the new event.
                WeakReferenceMessenger.Default.Send(new NewEventMessage(UpdatedEvent));
            }
            // Display the response message.
            ResponseMessageVisible = Visibility.Visible;
        }

        // Asynchronous method to update an existing event
        private async Task UpdateEventAsync()
        {
            if (OriginalEvent == null)
                return;

            // Clear any previous response messages
            ResponseMessage = string.Empty;

            // Validate input fields.
            bool isValid = _validator.ValidateAll(Name, Location, (int?)Capacity);

            if (!isValid)
            {
                // Display validation errors.
                NameError = _validator.NameError;
                LocationError = _validator.LocationError;
                CapacityError = _validator.CapacityError;
                return;
            }

            // Show loader while adding the event and simulate server response delay for 2 seconds
            App.GlobalLoader.SetTimer(2000);

            // Send the updated event data to the event service for updating and wait for the response
            var response = await _eventService.UpdateEventAsync(OriginalEvent, UpdatedEvent);
            if (response?.Error?.Count > 0)
            {
                // Display an error message if there was a problem updating the event.
                ResponseMessage = response.Error[0].Message;
                ResponseTextColor = new SolidColorBrush(Colors.Red);
            }
            else
            {
                // If the response is successful and contains event data, apply the changes
                if (response is ApiResponse<Core.Models.Event> apiResponse)
                {
                    // Apply the changes from the updated event data to the original event
                    ApplyChanges(apiResponse.Data);

                    // Set the success message from the response and display it
                    ResponseMessage = response.Info[0].Message;

                    // Set the text color for the response message to green (indicating success)
                    ResponseTextColor = new SolidColorBrush(Colors.Green);

                    // Disable the input fields after the event is successfully updated
                    IsTextBoxEnabled = false;
                }
            }
            // Display the response message.
            ResponseMessageVisible = Visibility.Visible;

        }

        // This method uses reflection to update each property of the original event with 
        private void ApplyChanges(Core.Models.Event data)
        {
            // If the data or the original event is null, return 
            if (data == null || OriginalEvent == null) return;

            // Iterate over each property in the Core.Models.Event class
            foreach (var prop in typeof(Core.Models.Event).GetProperties())
            {
                // Check if the property can be written to (i.e., it's not read-only)
                if (prop.CanWrite)
                {
                    // Set the value of the original event's property to the value of the updated event's property
                    prop.SetValue(OriginalEvent, prop.GetValue(data));
                }
            }
        }

        // Returns true, enabling the Save button unconditionally. 
        // Currently, this means the button will always be enabled, 
        // regardless of the form state or field validation.
        private bool CanSave()
        {
            return true;
        }
    }
}
