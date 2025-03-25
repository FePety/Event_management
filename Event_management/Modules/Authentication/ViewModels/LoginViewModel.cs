using Event_management.Core.Contracts;
using Event_management.Core.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace Event_management.Modules.Authentication.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        // Event that triggers when a property value changes.
        public event PropertyChangedEventHandler PropertyChanged;

        // Notifies listeners of a property change.
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //The event notifies subscribers when a property changes
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly IAuthService _authService;
        private readonly LoginValidator _validator;
        private string _email;
        private string _password;
        private string _emailError;
        private string _passwordError;
        private string _errorMessage;

        // Command that is executed to sign in the user.
        public RelayCommand SignInCommand { get; }

        // Event triggered when the login is successful.
        public event EventHandler LoginSuccessful;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            _validator = new LoginValidator();

            // Initializes the SignIn command with its execution and condition methods.
            SignInCommand = new RelayCommand(SignInAsync, CanSignIn);
        }

        // Sets the email, validates it, updates the error message, and triggers UI updates.
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                _validator.ValidateEmail(value);
                EmailError = _validator.EmailError;
                OnPropertyChanged();
                SignInCommand.RaiseCanExecuteChanged();
            }
        }

        // Sets the password, validates it, updates the password error, and triggers UI updates.
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                _validator.ValidatePassword(value);
                PasswordError = _validator.PasswordError;
                OnPropertyChanged();
                SignInCommand.RaiseCanExecuteChanged();
            }
        }

        // Sets the email error message and triggers UI updates for both the error message and its opacity.
        public string EmailError
        {
            get => _emailError;
            set
            {
                _emailError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EmailErrorOpacity));
            }
        }

        // Sets the password error message and triggers UI updates for error display.
        public string PasswordError
        {
            get => _passwordError;
            set
            {
                _passwordError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PasswordErrorOpacity)); // Updates the opacity based on error state.
            }
        }

        // Sets the general error message and triggers UI updates for error display.
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasError)); // Updates the visibility of the error indicator.
            }
        }

        // Determines if an error message is present and controls its visibility in the UI.
        public Visibility HasError => !string.IsNullOrEmpty(ErrorMessage) ? Visibility.Visible : Visibility.Collapsed;

        // Controls the opacity of the email error message. If there is no error, it is hidden.
        public double EmailErrorOpacity => string.IsNullOrEmpty(EmailError) ? 0 : 1;

        // Controls the opacity of the password error message. If there is no error, it is hidden.
        public double PasswordErrorOpacity => string.IsNullOrEmpty(PasswordError) ? 0 : 1;


        private async void SignInAsync()
        {
            //Clears the error message
            ErrorMessage = string.Empty;

            // Validate email and password.
            if (!_validator.ValidateEmail(Email) || !_validator.ValidatePassword(Password))
                return;

            // Show loader while adding the event and simulate server response delay for 3 seconds
            App.GlobalLoader.SetTimer(3000);

            // Attempt to log in with the provided credentials.
            BaseResponse response = await _authService.LoginAsync(Email, Password);

            // Check if the response contains errors:
            // 1. Ensure the response is not null.
            // 2. Ensure the Error property of the response is not null.
            // 3. Ensure there is at least one error in the Error collection.
            if (response?.Error?.Count > 0)
            {
                // Display the first error message received from the server.
                ErrorMessage = response.Error[0].Message;
            }
            else
            {
                // If no errors, trigger the LoginSuccessful event.
                LoginSuccessful?.Invoke(this, EventArgs.Empty);
            }
        }

        // Checks if both the email and password are valid, determining whether sign-in is allowed.
        private bool CanSignIn()
        {
            return _validator.ValidateEmail(Email) &&
                   _validator.ValidatePassword(Password);
        }
    }
}
