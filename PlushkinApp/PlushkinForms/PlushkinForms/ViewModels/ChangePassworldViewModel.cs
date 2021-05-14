using PlushkinForms.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace PlushkinForms.ViewModels
{
    class ChangePassworldViewModel : BaseViewModel
    {
        public Command ChangePassworldCommand { get; }
        public Command ShowRegisterPageCommand { get; }

        public ChangePassworldViewModel()
        {
            ChangePassworldCommand = new Command(OnChangePassworldClicked);
            ShowRegisterPageCommand = new Command(OnShowRegisterPageClicked);
        }

        private async void OnChangePassworldClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }

        private async void OnShowRegisterPageClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }
    }
}
