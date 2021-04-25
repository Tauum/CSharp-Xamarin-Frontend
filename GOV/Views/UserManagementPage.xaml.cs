using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GOV.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserManagementPage : ContentPage
    {
        public User User { get; set; } //recieve user object from preious page
        public UserManagementPage() { }
        public UserManagementPage(User user)
        {
            User = user;
            InitializeComponent();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            listView.BeginRefresh(); //skips going to icommand refresh list
        }


        async Task<List<User>> LoadList()
        {
            List<User> userList;
            userList = await App.DataService.GetAllAsync<User>();
            listView.ItemsSource = userList;

            return userList;
        }

        async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new ProfilePage(e.SelectedItem as User));
            }
        }

        public ICommand RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
            await this.LoadList();
            IsRefreshing = false;
        });

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
    }
}