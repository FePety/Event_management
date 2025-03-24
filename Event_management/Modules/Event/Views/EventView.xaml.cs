using Event_management.Modules.Event.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Event_management.Modules.Event.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventView : Page
    {
        public EventViewModel ViewModel { get; } = new EventViewModel();

        public EventView()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;

        }

        private void EventGridView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (EventGridView.ItemsPanelRoot is ItemsWrapGrid itemsWrapGrid)
            {
                double availableWidth = e.NewSize.Width - 30;
                double minItemWidth = 200;
                int columns = Math.Max(1, (int)(availableWidth / minItemWidth));

                itemsWrapGrid.ItemWidth = availableWidth / columns;
                itemsWrapGrid.ItemHeight = 200;
            }
        }

        private void EventBorder_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                var editButton = border.FindName("EditingEvent") as Button;
                var deleteButton = border.FindName("RemoveEvent") as Button;

                if (editButton != null) editButton.Opacity = 1;
                if (deleteButton != null) deleteButton.Opacity = 1;
            }
        }

        private void EventBorder_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                var editButton = border.FindName("EditingEvent") as Button;
                var deleteButton = border.FindName("RemoveEvent") as Button;

                if (editButton != null) editButton.Opacity = 0;
                if (deleteButton != null) deleteButton.Opacity = 0;
            }
        }
    }
}
