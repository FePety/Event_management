using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Event_management.Core.Models
{
    public class Event : INotifyPropertyChanged
    {
        private string _name;
        private string _location;
        private string _country;
        private int? _capacity;

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Event() { }

        public Event(string name, string location, string country, int? capacity)
        {
            Name = name;
            Location = location;
            Country = country;
            Capacity = capacity;
        }
    }
}
