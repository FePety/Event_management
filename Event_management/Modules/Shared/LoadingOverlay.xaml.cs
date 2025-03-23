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

        public void SetTimer(int time)
        {
            this.Visibility = Visibility.Visible;

            aTimer = new Timer(time);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }

        private async void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            aTimer.Stop();
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                this.Visibility = Visibility.Collapsed;
            });
        }
    }
}
