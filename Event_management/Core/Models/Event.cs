using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Event_management.Core.Models
{
    // Represents an event with basic properties and change notification.
    // Implements INotifyPropertyChanged to notify subscribers when a property changes.
    public class Event : INotifyPropertyChanged
    {
        private string _name;
        private string _location;
        private string _country;
        private int? _capacity;

        // Event name with change notification.
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        // Event location with change notification.
        public string Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }

        // Event country with change notification.
        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged();
                }
            }
        }

        // Event capacity with change notification.
        public int? Capacity
        {
            get => _capacity;
            set
            {
                if (_capacity != value)
                {
                    _capacity = value;
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

        // Default constructor for Event.
        public Event() { }

        // Constructor for Event with parameters to set properties.
        public Event(string name, string location, string country, int? capacity)
        {
            Name = name;
            Location = location;
            Country = country;
            Capacity = capacity;
        }
    }
}
