using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Event_management.Modules.Event.Messages
{
    public class NewEventMessage : ValueChangedMessage<Core.Models.Event>
    {
        public NewEventMessage(Core.Models.Event value) : base(value) { }
    }
}
