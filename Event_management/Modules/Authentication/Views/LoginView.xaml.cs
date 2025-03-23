using Event_management.Core.Contracts;
using Event_management.Core.Models;
using Event_management.Core.Services;
using Event_management.Modules.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
