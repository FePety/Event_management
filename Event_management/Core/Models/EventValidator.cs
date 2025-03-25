namespace Event_management.Core.Models
{
    // This class is responsible for validating the event data (Name, Location, and Capacity) 
    public class EventValidator
    {
        public string NameError { get; private set; }
        public string LocationError { get; private set; }
        public string CapacityError { get; private set; }

        // Returns true if the name is valid (not empty), otherwise false.
        public bool ValidateName(string name)
        {
            NameError = string.Empty;

            // Checks if the name is null, empty, or consists only of whitespace
            if (string.IsNullOrWhiteSpace(name))
            {
                NameError = "The name field cannot be empty.";
                return false;
            }

            return true;
        }

        // Returns true if the location is valid, otherwise false.
        public bool ValidateLocation(string location)
        {
            LocationError = string.Empty;

            // Checks if the location is null, empty, or consists only of whitespace
            if (string.IsNullOrWhiteSpace(location))
            {
                LocationError = "The location field cannot be empty.";
                return false;
            }

            // Checks if the location exceeds 100 characters
            if (location.Length > 100)
            {
                LocationError = "The location cannot be longer than 100 characters.";
                return false;
            }

            return true;
        }

        // Returns true if the capacity is valid (positive number), otherwise false.
        public bool ValidateCapacity(int? capacity)
        {
            CapacityError = string.Empty;

            // Validate that capacity is positive if provided
            if (capacity.HasValue && capacity <= 0)
            {
                CapacityError = "Capacity must be a positive number.";
                return false;
            }

            return true;
        }

        // Returns true if all fields are valid, otherwise returns false.
        public bool ValidateAll(string name, string location, int? capacity)
        {
            // Validates the name field
            bool isValidName = ValidateName(name);

            // Validates the location field
            bool isValidLocation = ValidateLocation(location);

            // Validates the capacity field
            bool isValidCapacity = ValidateCapacity(capacity);

            // Returns true if all fields are valid, otherwise returns false
            return isValidName && isValidLocation && isValidCapacity;
        }
    }
}
