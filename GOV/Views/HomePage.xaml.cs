using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GOV.Models;
using GOV.Views;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public User User { get; set; } //recieve user object from preious page
        public HomePage(User user)//instanciate passing in user
        {
            User = user;
            BindingContext = this;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (User.Admin == false) { UserManagement.IsEnabled = false; } // obvious
            else { UserManagement.IsEnabled = true; }
            base.OnAppearing();
        }

        public HomePage() { }
        private async void ViewButton(object sender, EventArgs e) { await Navigation.PushAsync(new SearchMethodPage(User));}
        private async void MyProfileButton(object sender, EventArgs e) { await Navigation.PushAsync(new ProfilePage(User)); }
        private async void LeaderboardButton(object sender, EventArgs e) { await Navigation.PushAsync(new LeaderboardPage());}
        private async void SwipetestButton(object sender, EventArgs e) { await Navigation.PushAsync(new SwipeTest()); }
        private async void HashTestButton(object sender, EventArgs e) { await Navigation.PushAsync(new hashtest()); }
        private async void UserManagemntButton(object sender, EventArgs e) { await Navigation.PushAsync(new UserManagementPage(User)); }

    }
}