using Event_management.Core.Services;
using Event_management.Modules.Event.ViewModels;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Event_management.Modules.Event.Views
{
    public sealed partial class EventPanel : UserControl
    {
        public static readonly DependencyProperty EventProperty =
            DependencyProperty.Register("Event", typeof(Core.Models.Event), typeof(EventPanel), new PropertyMetadata(null));

        public EventPanelViewModel ViewModel { get; } = new EventPanelViewModel(new MockEventService());

        public EventPanel()
        {
            this.InitializeComponent(); 
            Trace.WriteLine(this.DataContext);
        }

        private void UserControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Core.Models.Event Event = sender.DataContext as Core.Models.Event;
            if (Event != null && ViewModel != null)
            {
                ViewModel.SelectedEvent = Event;
            }
        }
    }
}
