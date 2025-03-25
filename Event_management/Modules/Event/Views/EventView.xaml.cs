using Event_management.Core.Models;
using Event_management.Core.Services;
using Event_management.Modules.Event.ViewModels;
using Event_management.Modules.Shared;
using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Event_management.Modules.Event.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventView : Page
    {
        private readonly MockEventService _eventService = MockEventService.Instance;


        // The EventViewModel is initialized and assigned to the DataContext of the EventView.
        public EventViewModel ViewModel { get; } = new EventViewModel();

        public EventView()
        {
            this.InitializeComponent();

            // Binds the View to the ViewModel for data binding
            this.DataContext = ViewModel;
        }

        // Event handler that adjusts the layout of the EventGridView when its size changes.
        private void EventGridView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Checks if the ItemsPanelRoot is an ItemsWrapGrid 
            if (EventGridView.ItemsPanelRoot is ItemsWrapGrid itemsWrapGrid)
            {
                // Calculate the available width by subtracting 30px from the total width
                double availableWidth = e.NewSize.Width - 30;

                // Define the minimum width for each grid item
                double minItemWidth = 200;

                // Calculate the number of columns by dividing the available width by the minimum item width
                int columns = Math.Max(1, (int)(availableWidth / minItemWidth));

                // Set the item width based on the available width divided by the number of columns
                itemsWrapGrid.ItemWidth = availableWidth / columns;

                // Set a fixed height of 200px for each item
                itemsWrapGrid.ItemHeight = 200;
            }
        }

        // When the pointer hovers over the panel, it makes the Edit and Delete buttons visible.
        private void EventBorder_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            // Check if the sender is a Border.
            if (sender is Border border)
            {
                // Find the Edit and Delete buttons within the Border by their names
                var editButton = border.FindName("EditingEvent") as Button;
                var deleteButton = border.FindName("RemoveEvent") as Button;

                // If the buttons are found, set their opacity to 1 (fully visible)
                if (editButton != null) editButton.Opacity = 1;
                if (deleteButton != null) deleteButton.Opacity = 1;
            }
        }

        // When the pointer leaves the Border, the opacity of the Edit and Delete buttons is set to 0 (invisible).
        private void EventBorder_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Border border)
            {
                // Find the Edit and Delete buttons within the Border by their names
                var editButton = border.FindName("EditingEvent") as Button;
                var deleteButton = border.FindName("RemoveEvent") as Button;

                // If the buttons are found, set their opacity to 0 (invisible)
                if (editButton != null) editButton.Opacity = 0;
                if (deleteButton != null) deleteButton.Opacity = 0;
            }
        }

        // Retrieves the associated event from the button's DataContext and updates the EventPanel with the event details.
        private void EditingEvent_Click(object sender, RoutedEventArgs e)
        {
            // Retrieves the 'Event' object associated with the button's DataContext.
            Core.Models.Event editingEvent = (sender as Button).DataContext as Core.Models.Event;

            // If the editingEvent is null, exit the method
            if (editingEvent == null) return;

            // Set the EventPanel's DataContext to the selected event
            EventPanel.DataContext = editingEvent;

            // Select the event in the EventGridView
            EventGridView.SelectedItem = editingEvent;

            // Enable textboxes for editing in the EventPanel
            EventPanel.ViewModel.IsTextBoxEnabled = true;

            // Set to false, as the next modification will create a new event.
            EventPanel.ViewModel.IsNewEvent = false;
        }

        private void EventGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Updates the EventPanel's DataContext with the selected event
            EventPanel.DataContext = (sender as GridView).SelectedItem;

            // Set to false, as the next modification will create a new event.
            EventPanel.ViewModel.IsNewEvent = false;
        }

        // Resets the EventPanel values to empty for creating a new event.
        private void NewEventButton_Click(object sender, RoutedEventArgs e)
        {
            EventPanel.ViewModel.ResetEventForm();
        }

        // Handles the Click event for the RemoveEvent button, which deletes a selected event after confirmation.
        private async void RemoveEvent_Click(object sender, RoutedEventArgs e)
        {
            // Disable textboxes
            EventPanel.ViewModel.IsTextBoxEnabled = false;

            // Prevent creating a new event during the next update
            EventPanel.ViewModel.IsNewEvent = false;

            // Check if the sender is a Button and its DataContext is an Event object
            if (sender is Button button && button.DataContext is Core.Models.Event deleteEvent)
            {
                // Find the event to remove by matching properties
                var eventToRemove = ViewModel.Events?
                    .FirstOrDefault(ev =>
                        ev.Name == deleteEvent.Name &&
                        ev.Location == deleteEvent.Location &&
                        ev.Country == deleteEvent.Country &&
                        ev.Capacity == deleteEvent.Capacity);

                string editEvent = "Delete event";
                var dialog = new MessageDialog(editEvent);

                // Show the confirmation dialog and wait for the result
                var result = await dialog.ShowAsync();

                // If the user confirms the deletion, remove the event from the list
                if (result == ContentDialogResult.Primary)
                {
                    if (eventToRemove != null)
                    {
                        // Show loader while adding the event and simulate server response delay for 2 seconds
                        App.GlobalLoader.SetTimer(2000);

                        // Remove event
                        var response = await _eventService.DeleteEventAsync(eventToRemove);
                        if (response?.Error?.Count > 0)
                        {
                            // Display an error message if there was a problem updating the event.
                            EventPanel.ViewModel.ResponseMessage = response.Error[0].Message;
                            EventPanel.ViewModel.ResponseTextColor = new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            // Set the success message from the response and display it
                            EventPanel.ViewModel.ResponseMessage = response.Info[0].Message;

                            EventPanel.ViewModel.ResponseMessageVisible = Visibility.Visible;

                            // Set the text color for the response message to green (indicating success)
                            EventPanel.ViewModel.ResponseTextColor = new SolidColorBrush(Colors.Green);

                            // Disable the input fields after the event is successfully updated
                            EventPanel.ViewModel.IsTextBoxEnabled = false;
                        }
                        ViewModel.Events.Remove(eventToRemove); // Remove the event from the list
                    }
                }
            }
        }

        // Disables input fields, prevents creating a new event, and shows a sign-out confirmation dialog.
        private async void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            EventPanel.ViewModel.IsTextBoxEnabled = false; // Disable input fields

            // Set to false, as the next modification will create a new event.
            EventPanel.ViewModel.IsNewEvent = false;

            string isSignOut = "Sign Out";
            MessageDialog messageDialog = new MessageDialog(isSignOut);
            await messageDialog.ShowAsync(); // Show the dialog asynchronously
        }
    }
}
