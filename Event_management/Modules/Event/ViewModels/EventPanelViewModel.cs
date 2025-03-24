using Event_management.Core.Contracts;
using Event_management.Core.Models;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;

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
        private string _errorMessage;
        private readonly EventValidator _validator;
        private Core.Models.Event _selectedEvent;

        public RelayCommand SaveCommand { get; private set; }

        public EventPanelViewModel(IEventService eventService)
        {
            _eventService = eventService;
            _validator = new EventValidator();
            SaveCommand = new RelayCommand(async () => await UpdateEventAsync(), CanSave);
        }

        public Core.Models.Event SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                if (_selectedEvent != value)
                {
                    _selectedEvent = value;
                    IsTextBoxEnabled = false;
                    OnPropertyChanged();
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
            get => SelectedEvent?.Name;
            set
            {
                if (SelectedEvent != null && SelectedEvent.Name != value)
                {
                    SelectedEvent.Name = value;
                    OnPropertyChanged();
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Location
        {
            get => SelectedEvent?.Location;
            set
            {
                if (SelectedEvent != null && SelectedEvent.Location != value)
                {
                    SelectedEvent.Location = value;
                    OnPropertyChanged();
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Country
        {
            get => SelectedEvent?.Country;
            set
            {
                if (SelectedEvent != null && SelectedEvent.Country != value)
                {
                    SelectedEvent.Country = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Capacity
        {
            get => SelectedEvent?.Capacity ?? 0;
            set
            {
                if (SelectedEvent != null && SelectedEvent.Capacity != (int)value)
                {
                    SelectedEvent.Capacity = (int)value;
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

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasError));
            }
        }

        public Visibility HasError => !string.IsNullOrEmpty(ErrorMessage) ? Visibility.Visible : Visibility.Collapsed;

        private async Task UpdateEventAsync()
        {
            if (SelectedEvent == null)
                return;

            ErrorMessage = string.Empty;

            bool isValid = _validator.ValidateAll(Name, Location, (int?)Capacity);

            if (!isValid)
            {
                ErrorMessage = $"{_validator.NameError} {_validator.LocationError} {_validator.CapacityError}".Trim();
                return;
            }

            App.GlobalLoader.SetTimer(2000);

            var updatedEvent = new Core.Models.Event(
                name: Name,
                location: Location,
                country: Country,
                capacity: (int)Capacity
            );
            var response = await _eventService.UpdateEventAsync(updatedEvent);

            if (response?.Error?.Count > 0)
            {
                ErrorMessage = response.Error[0].Message;
            }
            else
            {
                if (response is ApiResponse<Core.Models.Event> apiResponse)
                {
                    SelectedEvent = apiResponse.Data;
                }
                ShowMessage("Event updated successfully!");
            }
        }

        private bool CanSave()
        {
            return SelectedEvent != null &&
                   !string.IsNullOrEmpty(SelectedEvent.Name) &&
                   !string.IsNullOrEmpty(SelectedEvent.Location) &&
                   !string.IsNullOrEmpty(SelectedEvent.Country) &&
                   string.IsNullOrEmpty(ErrorMessage);
        }

        private void ShowMessage(string message)
        {
            // Implementálhatod a visszajelzés megjelenítését (pl. MessageBox)
            //MessageBox.Show(message);
        }
    }

}
