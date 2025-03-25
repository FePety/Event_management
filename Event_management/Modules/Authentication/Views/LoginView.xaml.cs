using Event_management.Core.Services;
using System;
using Event_management.Modules.Event.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Event_management.Modules.Authentication.ViewModels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginView : Page
    {
        public LoginViewModel ViewModel { get; set; }

        public LoginView()
        {
            this.InitializeComponent();
            
            ViewModel = new LoginViewModel(new MockAuthService());
            DataContext = ViewModel;

            // Subscribe to the LoginSuccessful event to handle successful logins.
            ViewModel.LoginSuccessful += ViewModel_LoginSuccessful;
        }

        // Moves focus to the password input field when the Enter key is pressed in the email field.
        private void Email_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Password.Focus(FocusState.Programmatic);
            }
        }

        // Executes the SignIn command when the Enter key is pressed in the password field.
        private void Password_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                ViewModel.SignInCommand.Execute(null);
            }
        }

        // Navigates to the EventView page when the login is successful.
        private void ViewModel_LoginSuccessful(object sender, EventArgs e)
        {
            Frame.Navigate(typeof(EventView));
        }
    }
}
