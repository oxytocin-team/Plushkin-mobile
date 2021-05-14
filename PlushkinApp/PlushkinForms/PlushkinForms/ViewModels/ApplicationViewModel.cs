using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using PlushkinForms.Models;
using PlushkinForms.Services;
using static PlushkinForms.Services.BookmarkService;
using Xamarin.Essentials;

namespace PlushkinForms.ViewModels
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        Bookmark selectedBookmark;
        string selectedMenuItem;
        private bool isBusy;    // идет ли загрузка с сервера
        public List<string> Menu { get; set; }

        public ObservableCollection<Bookmark> Bookmarks { get; set; }
        BookmarkService bookmarkService = new BookmarkService();
        public event PropertyChangedEventHandler PropertyChanged;


        public INavigation Navigation { get; set; }

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


        public ApplicationViewModel()
        {
            Menu = new List<string> { "Недавнее", "Все закладки", "Любимое", "Корзина" };
            SelectedMenuItem = Menu[0];
            Bookmarks = new ObservableCollection<Bookmark>();
            IsBusy = false;
        }


        public string SelectedMenuItem
        {
            get { return selectedMenuItem; }
            set
            {
                if (selectedMenuItem != value)
                {
                    selectedMenuItem = value;
                    OnPropertyChanged("SelectedMenuItem");

                    switch (selectedMenuItem)
                    {
                        case "Недавнее":
                            new Task(async () => { await GetBookmarks(TypeFilter.Unsorted); }).Start();
                            break;
                        case "Все закладки":
                            new Task(async () => { await GetBookmarks(TypeFilter.Empty); }).Start();
                            break;
                        case "Любимое":
                            new Task(async () => { await GetBookmarks(TypeFilter.Liked); }).Start();
                            break;
                        case "Корзина":
                            new Task(async () => { await GetBookmarks(TypeFilter.Trash); }).Start();
                            break;

                    }

                    //App.Current.MainPage.DisplayAlert(selectedMenuItem, "Test2", "OK");
                    //Navigation.PushAsync(new BookmarkPage(tempBookmark, this));
                }
            }
        }

        public Bookmark SelectedBookmark
        {
            get { return selectedBookmark; }
            set
            {
                if (selectedBookmark != value)
                {
                    Bookmark tempBookmark = new Bookmark()
                    {
                        id = value.id,
                        type = value.type,
                        title = value.title,
                        url = value.url,
                        date = value.date,
                        user = value.user
                    };
                    selectedBookmark = null;
                    OnPropertyChanged("SelectedBookmark");
                    //Navigation.PushAsync(new BookmarkPage(tempBookmark, this));
                }
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public async Task GetBookmarks(TypeFilter filter)
        {
            IsBusy = true;
            IEnumerable<Bookmark> bookmarks = await bookmarkService.Get(filter);

            while (Bookmarks.Any())
                Bookmarks.RemoveAt(Bookmarks.Count - 1);

            foreach (Bookmark f in bookmarks)
                Bookmarks.Add(f);
            IsBusy = false;
            OnPropertyChanged("SelectedMenuItem");
            OnPropertyChanged("Bookmarks");
        }

        public async Task BookmarksSearch(string filter)
        {
            IsBusy = true;
            IEnumerable<Bookmark> bookmarks = await bookmarkService.Search(filter);

            while (Bookmarks.Any())
                Bookmarks.RemoveAt(Bookmarks.Count - 1);

            foreach (Bookmark f in bookmarks)
                Bookmarks.Add(f);
            IsBusy = false;
            OnPropertyChanged("SelectedMenuItem");
            OnPropertyChanged("Bookmarks");
        }

        public async void SaveBookmark(object bookmarkObject)
        {
            if (bookmarkObject is Bookmark bookmark)
                await bookmarkService.Add(bookmark);
        }

        public ICommand ShareBookmarkCommand => new Command(async (object bookmarkObject) => 
        {
            if (bookmarkObject is Bookmark bookmark)
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Uri = bookmark.url,
                    Title = "Share " + bookmark.title
                });
            }
        });

        public ICommand LikeBookmarkCommand => new Command(async (object bookmarkObject) =>
        {
            if (bookmarkObject is Bookmark bookmark)
            {
                bookmark.type = "L";
                Bookmark updatedBookmark = await bookmarkService.Update(bookmark);

                IEnumerable<Bookmark> bookmarks = await bookmarkService.Get(TypeFilter.Unsorted);

                while (Bookmarks.Any())
                    Bookmarks.RemoveAt(Bookmarks.Count - 1);

                foreach (Bookmark f in bookmarks)
                    Bookmarks.Add(f);

                OnPropertyChanged("Bookmarks");
            }
        });

        public ICommand DeleteBookmarkCommand => new Command(async (object bookmarkObject) =>
        {
            if (bookmarkObject is Bookmark bookmark)
            {
                bookmark.type = "T";
                Bookmark updatedBookmark = await bookmarkService.Update(bookmark);


                IEnumerable<Bookmark> bookmarks = await bookmarkService.Get(TypeFilter.Unsorted);

                while (Bookmarks.Any())
                    Bookmarks.RemoveAt(Bookmarks.Count - 1);

                foreach (Bookmark f in bookmarks)
                    Bookmarks.Add(f);

                OnPropertyChanged("Bookmarks");
            }
        });

        public ICommand OpenUrlkCommand => new Command(async (object urlObject) =>
        {
            if (urlObject is string url)
            {
                await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
            }
        });

        public ICommand RegisterCommand => new Command(async () =>
        {
            var ss = "ss";
        });
    }
}
