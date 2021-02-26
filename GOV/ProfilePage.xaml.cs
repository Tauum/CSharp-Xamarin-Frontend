using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GOV.Models;
using Xamarin.Essentials;
using System.IO;
using Plugin.SimpleAudioPlayer;
using GOV.Extensions;
using GOV.Helpers;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public User User { set;  get; }
        private ISimpleAudioPlayer Player { get; }
        public ProfilePage(User user)
        {
            User = user;
            Player = CrossSimpleAudioPlayer.Current;
            BindingContext = this;
            InitializeComponent();
        }
        public ProfilePage()
        {
            InitializeComponent();
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
                    await App.DataService.UpdateAsync(User, User.ID);
                    PlaySound("bell");
                    await DisplayAlert("Profile", "Update Success", "X");
                }
                else { await DisplayAlert("Error", "An error occured.\n Please try again later", "X"); }
            }
        }

        async void MyProductsButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchResults(User));
        }

        async void PlaySound(string mp3)
        {
            Player.Load($"{mp3}.mp3");
            Player.Play();
        }
    }
}