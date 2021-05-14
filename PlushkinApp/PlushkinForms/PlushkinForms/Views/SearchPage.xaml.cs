using PlushkinForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static PlushkinForms.Services.BookmarkService;

namespace PlushkinForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        ApplicationViewModel viewModel;
        public SearchPage()
        {
            InitializeComponent();
            viewModel = new ApplicationViewModel();
            BindingContext = viewModel;
        }

        private void EntrySearch_Completed(object sender, EventArgs e)
        {
            _ = viewModel.BookmarksSearch(((Entry)sender).Text);
        }
    }
}