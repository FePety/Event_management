using Event_management.Core.Services;
using System;
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
            ViewModel.LoginSuccessful += ViewModel_LoginSuccessful;
        }

        private void Email_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Password.Focus(FocusState.Programmatic);
            }
        }

        private void Password_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                ViewModel.SignInCommand.Execute(null);
            }
        }

        private void ViewModel_LoginSuccessful(object sender, EventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
