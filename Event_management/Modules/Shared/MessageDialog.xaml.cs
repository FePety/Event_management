using Event_management.Core.Services;
using Event_management.Modules.Authentication.ViewModels;
using Event_management.Modules.Event.Views;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Event_management.Modules.Shared
{
    public sealed partial class MessageDialog : ContentDialog
    {
        private MockAuthService _mockAuthService;
        private string _message;

        public MessageDialog(string message)
        {
            this.InitializeComponent();
            _mockAuthService = new MockAuthService();
            _message = message;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            switch (_message)
            {
                case "Sign Out":
                    await _mockAuthService.LogoutAsync();
                    var rootFrame = new Frame();
                    rootFrame.Navigate(typeof(LoginView));
                    Window.Current.Content = rootFrame;
                    Window.Current.Activate();
                    break;
                case "Delete event":
                    Console.WriteLine("Guest user, no logout needed.");
                    break;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
