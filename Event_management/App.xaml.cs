using Event_management.Core.Contracts;
using Event_management.Core.Services;
using Event_management.Modules.Authentication.ViewModels;
using Event_management.Modules.Event.Views;
using Event_management.Modules.Shared;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Event_management
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private IAuthService _authService;
        public static LoadingOverlay GlobalLoader { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;

            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                var mainGrid = new Grid();
                GlobalLoader = new LoadingOverlay { Visibility = Visibility.Visible };

                mainGrid.Children.Add(rootFrame);
                mainGrid.Children.Add(GlobalLoader);

                Window.Current.Content = mainGrid;
                Window.Current.Activate();

                _authService = new MockAuthService();
                await CheckLoginStatusAsync(rootFrame);
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(EventView), e.Arguments);
                }
                Window.Current.Activate();
            }
        }

        private async Task CheckLoginStatusAsync(Frame rootFrame)
        {
            // Ellenőrizzük, hogy a felhasználó be van-e jelentkezve
            bool isLoggedIn = await _authService.IsUserLoggedInAsync();

            // Ha be van jelentkezve, irányítsuk a főoldalra, egyébként pedig a bejelentkezési oldalra
            if (isLoggedIn)
            {
                rootFrame.Navigate(typeof(EventView)); // Bejelentkezett felhasználó, tehát a főoldalra navigálunk
            }
            else
            {
                rootFrame.Navigate(typeof(LoginView)); // Nincs bejelentkezve, tehát a Login oldalra navigálunk
            }
            GlobalLoader.Visibility = Visibility.Collapsed; // Rejtse el az animációt
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to load Page: {e.SourcePageType.FullName}");
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }
        /*void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }*/

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
