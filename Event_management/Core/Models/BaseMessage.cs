namespace Event_management.Core.Models
{
    // Represents a base class for messages, containing key, message, and description properties.
    public class BaseMessage
    {
        public string Key { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }

        // Constructor initializing the properties with optional default values.
        public BaseMessage(string key = default, string message = default, string description = default)
        {
            Key = key;
            Message = message;
            Description = description;
        }
    }
}
