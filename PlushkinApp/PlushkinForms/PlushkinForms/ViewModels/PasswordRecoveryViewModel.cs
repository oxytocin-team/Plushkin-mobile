using PlushkinForms.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace PlushkinForms.ViewModels
{
    class PasswordRecoveryViewModel : BaseViewModel
    {

        public Command SendLinkCommand { get; }
        public Command ShowRegisterPageCommand { get; }

        public PasswordRecoveryViewModel() 
        {
            SendLinkCommand = new Command(OnSendLinkClicked);
            ShowRegisterPageCommand = new Command(OnShowRegisterPageClicked);
        }

        private async void OnSendLinkClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(ChangePassworldPage)}");
        }

        private async void OnShowRegisterPageClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }
    }
}
