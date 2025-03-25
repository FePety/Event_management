using System;
using System.Timers;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Event_management.Modules.Shared
{
    public sealed partial class LoadingOverlay : UserControl
    {
        public Timer aTimer;

        public LoadingOverlay()
        {
            this.InitializeComponent();
        }

        // Sets the timer and displays the loading overlay for the specified time (in milliseconds)
        public void SetTimer(int time)
        {
            this.Visibility = Visibility.Visible; // Show the loading overlay

            aTimer = new Timer(time); // Initialize the timer with the specified time
            aTimer.Elapsed += OnTimedEvent; // Register the event handler for when the timer elapses
            aTimer.AutoReset = false; // Ensure the timer only runs once
            aTimer.Enabled = true; // Start the timer
        }

        // Event handler that is triggered when the timer elapses
        private async void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            aTimer.Stop(); // Stop the timer once it elapses

            // Use the dispatcher to update the UI on the main thread
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                this.Visibility = Visibility.Collapsed; // Hide the loading overlay
            });
        }
    }
}
