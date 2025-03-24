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

        public EventPanelViewModel ViewModel { get; } = new EventPanelViewModel();

        public EventPanel()
        {
            this.InitializeComponent(); 
            Trace.WriteLine(this.DataContext);
        }

        public Core.Models.Event Event
        {
            get { return (Core.Models.Event)GetValue(EventProperty); }
            set { SetValue(EventProperty, value); }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
