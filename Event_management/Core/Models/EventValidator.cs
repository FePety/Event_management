using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_management.Core.Models
{
    public class EventValidator
    {
        public string NameError { get; private set; }
        public string LocationError { get; private set; }
        public string CapacityError { get; private set; }

        public bool ValidateName(string name)
        {
            NameError = string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                NameError = "The name field cannot be empty.";
                return false;
            }

            return true;
        }

        public bool ValidateLocation(string location)
        {
            LocationError = string.Empty;

            if (string.IsNullOrWhiteSpace(location))
            {
                LocationError = "The location field cannot be empty.";
                return false;
            }

            if (location.Length > 100)
            {
                LocationError = "The location cannot be longer than 100 characters.";
                return false;
            }

            return true;
        }

        public bool ValidateCapacity(int? capacity)
        {
            CapacityError = string.Empty;

            if (capacity.HasValue && capacity <= 0)
            {
                CapacityError = "Capacity must be a positive number.";
                return false;
            }

            return true;
        }

        public bool ValidateAll(string name, string location, int? capacity)
        {
            bool isValidName = ValidateName(name);
            bool isValidLocation = ValidateLocation(location);
            bool isValidCapacity = ValidateCapacity(capacity);

            return isValidName && isValidLocation && isValidCapacity;
        }
    }
}
