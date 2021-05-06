using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using System.IO;
using Plugin.SimpleAudioPlayer;
using GOV.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GOV.Models;
using GOV.Helpers;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private User _user;
        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }
        public User AdminUser { get; set; }
        private ISimpleAudioPlayer Player { get; }
        private string _viewStatus;
        public string ViewStatus
        {
            get { return _viewStatus; }
            set
            {
                _viewStatus = value;
                OnPropertyChanged(nameof(ViewStatus));
            }
        }

        public ProfilePage() { InitializeComponent(); }

        public ProfilePage(User user)
        {
            User = user;
            Player = CrossSimpleAudioPlayer.Current;
            BindingContext = this;
            InitializeComponent();
        }

        public ProfilePage(User adminUser, User user )
        {
            AdminUser = adminUser;
            User = user;
            Player = CrossSimpleAudioPlayer.Current;
            BindingContext = this;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (User == null) { await Navigation.PopToRootAsync(); }
            else
            {
                if (User.Admin) { ViewStatus = "Admin"; }
                else { ViewStatus = "User"; }

                if (AdminUser == null) { AccountStatus.IsEnabled = false; }

                BindingContext = this;
            }
        }

        private void AccountStatusButton(object sender, EventArgs e) //contents isnt working
        {
            var btn = (Button)sender;
            if (User.Admin)//setting button text state depending on review status
            {
                User.Admin = false;
                btn.Text = "User";
            }

            else
            {
                User.Admin = true;
                btn.Text = "Admin";
            }
        }

        async void SaveButton(object sender, EventArgs e)
        {
            if (UsernameInput.Text.IsNullOrEmpty() || PasswordInput.Text.IsNullOrEmpty() || PasswordConfirmInput.Text.IsNullOrEmpty())
            {
                PlaySound("ding98");
                await DisplayAlert("Error", "Missing Field/s", "X");
            }
            else if (PasswordInput.Text != PasswordConfirmInput.Text)
            {
                PlaySound("ding98");
                await DisplayAlert("Error", "Unmatching Passwords", "X");
            }
            else
            {
                if (User.ID != 0)
                { 
                    User.Password = Hashing.GetHash(PasswordInput.Text);
                    User.Username = UsernameInput.Text;
                    await App.DataService.UpdateAsync(User, User.ID);
                    PlaySound("bell");
                    await DisplayAlert("Profile", "Update Success", "X");
                    Navigation.PopAsync(); // return to old page
                }
                //else if (User.ID != 0 && User.Username == UsernameInput.Text.ToString())
                //{
                //    User.Password = Hashing.GetHash(PasswordInput.Text);
                //    User.Username = UsernameInput.Text;
                //    await App.DataService.UpdateAsync(User, User.ID);
                //    PlaySound("bell");
                //    await DisplayAlert("Profile", "Update Success", "X");
                //}
                else { await DisplayAlert("Error", "An error occured.", "X"); }
            }
        }

        async void MyReviewsButton(object sender, EventArgs e) { await Navigation.PushAsync(new ReviewPage(User)); }

        async void PlaySound(string mp3)
        {
            Player.Load($"{mp3}.mp3");
            Player.Play();
        }
    }
}