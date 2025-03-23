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
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly IAuthService _authService;
        private readonly LoginValidator _validator;
        private string _email;
        private string _password;
        private string _emailError;
        private string _passwordError;
        private string _errorMessage;

        public RelayCommand SignInCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler LoginSuccessful;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            _validator = new LoginValidator();
            SignInCommand = new RelayCommand(SignInAsync, CanSignIn);
        }

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

        public string PasswordError
        {
            get => _passwordError;
            set
            {
                _passwordError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PasswordErrorOpacity));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasError));
            }
        }

        public Visibility HasError => !string.IsNullOrEmpty(ErrorMessage) ? Visibility.Visible : Visibility.Collapsed;
        public double EmailErrorOpacity => string.IsNullOrEmpty(EmailError) ? 0 : 1;
        public double PasswordErrorOpacity => string.IsNullOrEmpty(PasswordError) ? 0 : 1;

        private async void SignInAsync()
        {
            ErrorMessage = string.Empty;

            if (!_validator.ValidateEmail(Email) || !_validator.ValidatePassword(Password))
                return;

            App.GlobalLoader.SetTimer(3000);
            BaseResponse response = await _authService.LoginAsync(Email, Password);

            // 1. The response is not null
            // 2. The Error property of the response is not null
            // 3. There is at least one error in the Error collection
            if (response?.Error?.Count > 0)
            {
                ErrorMessage = response.Error[0].Message;
            }
            else
            {
                LoginSuccessful?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool CanSignIn()
        {
            return _validator.ValidateEmail(Email) &&
                   _validator.ValidatePassword(Password);
        }
    }
}
