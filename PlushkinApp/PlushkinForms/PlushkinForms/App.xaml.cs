using PlushkinForms.Models;
using PlushkinForms.Services;
using PlushkinForms.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlushkinForms
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        async protected override void OnStart()
        {
            if (Current.Properties.ContainsKey("authToken"))
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            else
                await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
