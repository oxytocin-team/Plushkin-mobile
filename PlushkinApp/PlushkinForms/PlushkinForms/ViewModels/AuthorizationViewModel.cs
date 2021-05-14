using PlushkinForms.Models;
using PlushkinForms.Services;
using PlushkinForms.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using ValidationsXFSample.Validators;
using ValidationsXFSample.Validators.Rules;
using Xamarin.Forms;

namespace PlushkinForms.ViewModels
{
    class AuthorizationViewModel : INotifyPropertyChanged
    {
        public List<string> Errors { get; set; } = new List<string>();
        public bool IsNotValid { get; set; } = true;

        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Password { get; set; } = new ValidatableObject<string>();


        UserService userService = new UserService();
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
                OnPropertyChanged("IsLoaded");
            }
        }
        public bool IsLoaded
        {
            get { return !isBusy; }
        }

        public AuthorizationViewModel() 
        {
            AddValidationRules();
        }

        public ICommand PasswordRecoveryCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync($"//{nameof(PasswordRecoveryPage)}");
        });

        public ICommand ShowRegisterPageCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        });

        public ICommand LoginCommand => new Command(async () =>
        {
            if (AreFieldsValid())
            {
                User user = new User
                {
                    email = Email.Value,
                    username = Email.Value,
                    password = Password.Value
                };

                IsBusy = true;
                AuthToken authToken = await userService.GetAuthToken(user);
                if (authToken != null)
                {
                    Application.Current.Properties["authToken"] = authToken.token;
                    await Application.Current.SavePropertiesAsync();

                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }
                IsBusy = false;
            }
        });

        private void AddValidationRules()
        {
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Неверный Email" });
            Password.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = "Неверный пароль" });
        }

        bool AreFieldsValid()
        {
            Email.Value = Email.Value?.Replace(" ", "");
            bool isEmailValid = Email.Validate();
            bool isPasswordValid = Password.Validate();

            IsNotValid = !(isEmailValid && isPasswordValid);

            Errors.Clear();
            Errors.AddRange(Email.Errors);
            Errors.AddRange(Password.Errors);

            OnPropertyChanged("Errors");
            OnPropertyChanged("IsNotValid");

            return isEmailValid && isPasswordValid;
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
