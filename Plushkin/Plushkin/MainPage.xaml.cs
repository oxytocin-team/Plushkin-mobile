using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Plushkin
{
    class Reg_form
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }

    class Au_form
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Tab_Clicked(object sender, EventArgs e)
        {
            formsignup.IsVisible = !(formsignup.IsVisible);
            formsignin.IsVisible = !(formsignin.IsVisible);

            signup.IsEnabled = !(signup.IsEnabled);
            signin.IsEnabled = !(signin.IsEnabled);
        }

        private async void Go_up_Clicked(object sender, EventArgs e)
        {
            var reg_form = new Reg_form() { username = login_up.Text, email = email.Text, password = pass_up.Text };
            string json = JsonConvert.SerializeObject(reg_form);

            try
            {
                HttpClient client = new HttpClient();
                var response = await client.PostAsync("http://188.226.96.115:8000/core/user_registration/", new StringContent(json, Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode(); // выброс исключения, если произошла ошибка

                await Navigation.PushModalAsync(new Home());
            }
            catch (Exception ex)
            {

            }
        }

        private async void Go_in_Clicked(object sender, EventArgs e)
        {

            var reg_form = new Au_form() { username = login_in.Text, password = pass_in.Text };
            string json = JsonConvert.SerializeObject(reg_form);

            try
            {
                HttpClient client = new HttpClient();
                var response = await client.PostAsync("http://188.226.96.115:8000/core/get_token/", new StringContent(json, Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode(); // выброс исключения, если произошла ошибка

                await Navigation.PushModalAsync(new Home());
            }
            catch (Exception ex)
            {

            }

        }
    }
}
