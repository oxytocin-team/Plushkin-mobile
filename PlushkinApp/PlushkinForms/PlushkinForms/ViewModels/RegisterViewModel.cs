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
    class RegisterViewModel : INotifyPropertyChanged
    {
        private bool isBusy;    // идет ли загрузка с сервера

        public List<string> Errors { get; set; } = new List<string>();
        public bool IsNotValid { get; set; } = true;

        public ValidatableObject<string> FirstName { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Password { get; set; } = new ValidatableObject<string>();

        UserService userService = new UserService();


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

        public RegisterViewModel()
        {
            //if (Application.Current.Resources.ContainsKey("authToken"))
            //    Shell.Current.GoToAsync($"//{nameof(HomePage)}");

            AddValidationRules();
        }

        public ICommand RegisterCommand => new Command(async () =>
        {
            if (AreFieldsValid())
            {
                User user = new User
                {
                    email = Email.Value,
                    username = Email.Value,
                    first_name = FirstName.Value,
                    password = Password.Value
                };

                IsBusy = true;

                UserErrorMessage userError = await userService.Registration(user);
                if (userError != null)
                {
                    IsNotValid = true;

                    Errors.Add(userError.username);

                    OnPropertyChanged("Errors");
                    OnPropertyChanged("IsNotValid");
                }
                else
                {
                    AuthToken authToken = await userService.GetAuthToken(user);

                    Application.Current.Properties["authToken"] = authToken.token;
                    await Application.Current.SavePropertiesAsync();

                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }

                IsBusy = false;
            }
        });

        public ICommand RegisterGoogleCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        });

        public ICommand LoginCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync($"//{nameof(AuthorizationPage)}");
        });

        private void AddValidationRules()
        {
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Неверный Email" });
            Password.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = "Пароль должен содержать не менее одной строчной буквы, одной заглавной буквы, одной цифровой цифры и одного специального символа." });
            FirstName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "" });
        }

        bool AreFieldsValid()
        {
            bool isFirstNameValid = FirstName.Validate();

            Email.Value = Email.Value?.Replace(" ", "");
            bool isEmailValid = Email.Validate();
            bool isPasswordValid = Password.Validate();

            IsNotValid = !(isFirstNameValid && isEmailValid && isPasswordValid);

            Errors.Clear();
            Errors.AddRange(FirstName.Errors);
            Errors.AddRange(Email.Errors);
            Errors.AddRange(Password.Errors);

            OnPropertyChanged("Errors");
            OnPropertyChanged("IsNotValid");

            return isFirstNameValid && isEmailValid && isPasswordValid;
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
