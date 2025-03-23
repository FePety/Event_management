using Event_management.Core.Contracts;
using Event_management.Core.Services;
using Event_management.Modules.Authentication.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Event_management
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IAuthService _authService;

        public MainPage()
        {
            this.InitializeComponent();
            _authService = new MockAuthService();

        }




        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await _authService.LogoutAsync();

            // Kijelentkezés után vissza a bejelentkezési oldalra
            Frame.Navigate(typeof(LoginView));
        }
    }
}
