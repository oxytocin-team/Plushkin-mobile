using PlushkinForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlushkinForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        ProfileViewModel viewModel;

        public ProfilePage()
        {
            InitializeComponent();
            viewModel = new ProfileViewModel();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            await viewModel.GetUser();
            base.OnAppearing();
        }

        private void EntryName_Completed(object sender, EventArgs e)
        {
            viewModel.ChangeName(((Entry)sender).Text);
        }

        private void EntryEmail_Completed(object sender, EventArgs e)
        {
            viewModel.ChangeEmail(((Entry)sender).Text);
        }

        private async void OnButtonClicked(object sender, System.EventArgs e)
        {
            bool result = await DisplayAlert("Подтвердить действие", "Вы точно хотите удалить свою учётную запись?", "Да, хочу удалить", "Я передумал");
            viewModel.DeleteUser();
        }
    }
}