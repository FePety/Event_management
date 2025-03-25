using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Event_management.Modules.Event.Messages
{
    // Defines a message class that inherits from ValueChangedMessage with a specific type of Event.
    // This class is used to send an event-related message that contains the event value of type Event.
    public class NewEventMessage : ValueChangedMessage<Core.Models.Event>
    {
        // Constructor that initializes the base class with the event value.
        public NewEventMessage(Core.Models.Event value) : base(value) { }
    }
}
