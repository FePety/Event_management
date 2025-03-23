using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Event_management.Modules.Shared
{
    public sealed partial class LoadingOverlay : UserControl
    {
       /* public LoadingOverlay()
        {
            this.InitializeComponent();
        }

        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }

        public void ShowWithTimeout(int milliseconds)
        {
            Trace.WriteLine("ShowWithTimeout");
            Show();
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(milliseconds);
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                Hide();
            };
            timer.Start();
        }*/
        public Timer aTimer;

        public LoadingOverlay()
        {
            this.InitializeComponent();
        }

        public void SetTimer(int time)
        {
            Trace.WriteLine("SetTimer");

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
                Trace.WriteLine("OnTimedEvent");
                this.Visibility = Visibility.Collapsed;
            });
        }
    }
}
