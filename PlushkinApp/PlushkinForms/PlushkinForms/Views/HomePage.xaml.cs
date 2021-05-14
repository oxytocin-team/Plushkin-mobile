using PlushkinForms.Models;
using PlushkinForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static PlushkinForms.Services.BookmarkService;

namespace PlushkinForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        ApplicationViewModel viewModel;
        public HomePage()
        {
            InitializeComponent();
            viewModel = new ApplicationViewModel() { Navigation = this.Navigation }; 
            BindingContext = viewModel;
        }
        protected override async void OnAppearing()
        {
            await viewModel.GetBookmarks(TypeFilter.Unsorted);
            base.OnAppearing();


            MessagingCenter.Subscribe<object, string[]>(this, "AddItem", (sender, arg) =>
            {
                string url = arg[1];

                WebClient x = new WebClient();
                string source = x.DownloadString(arg[1]);
                string siteName = Regex.Match(url, @"^([^/]*/){2}([^/]*)").Groups[2].ToString();
                string siteTitle = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                viewModel.SaveBookmark(new Bookmark() { type = "U", title = siteTitle, siteName = siteName, url = arg[1] });
                MessagingCenter.Unsubscribe<object, string[]>(this, "AddItem");
            });
        }
    }
}