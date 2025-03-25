using Event_management.Core.Services;
using Event_management.Modules.Event.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Event_management.Modules.Event.Views
{
    public sealed partial class EventPanel : UserControl
    {
        // Initializes the ViewModel with a new instance of Event Panel ViewModel, using MockEventService for testing.
        public EventPanelViewModel ViewModel { get; } = new EventPanelViewModel(new MockEventService());

        public EventPanel()
        {
            this.InitializeComponent(); 
        }

        // Handles the DataContext change and updates the ViewModel's OriginalEvent if the new DataContext is a valid Event.
        private void UserControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Core.Models.Event Event = sender.DataContext as Core.Models.Event;

            // Check if the Event and ViewModel are not null
            if (Event != null && ViewModel != null)
            {
                // Set the OriginalEvent in the ViewModel to the selected Event
                ViewModel.OriginalEvent = Event;
            }
        }
    }
}
