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
    class ProfileViewModel : INotifyPropertyChanged
    {
        public User User { get; set; }

        private readonly UserService userService = new UserService();

        public ProfileViewModel() {}

        public async System.Threading.Tasks.Task GetUser()
        {
            User = await userService.GetUser();
            OnPropertyChanged(nameof(User));
        }

        public void ChangeName(object param)
        {
            User.first_name = param.ToString();
            OnPropertyChanged(nameof(User));
            _ = userService.Update(User);
        }

        public void ChangeEmail(object param)
        {
            User.email = param.ToString();
            OnPropertyChanged(nameof(User));
            _ = userService.Update(User);
        }

        public async void DeleteUser()
        {
            _ = userService.Delete();
            if (Application.Current.Properties.ContainsKey("authToken"))
                Application.Current.Properties.Remove("authToken");
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }

        public ICommand LogOutCommand => new Command(async () =>
        {
            if (Application.Current.Properties.ContainsKey("authToken"))
                Application.Current.Properties.Remove("authToken");
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        });

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
