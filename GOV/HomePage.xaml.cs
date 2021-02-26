using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GOV.Models;// know how to build object

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public User User { get; set; } //needed to recieve user object from preious page
        public HomePage(User user)//this is when a user is passed in
        {
            User = user;
            BindingContext = this;
            InitializeComponent();
        }
        public HomePage() // default to prevent crash if a user is not passed, maybe smart or dumb considering if a user escaped passing in
        {
            InitializeComponent();
        }
        private async void ViewButton(object sender, EventArgs e) //obvious
        {
            await Navigation.PushAsync(new SearchMethod());
        }
        private async void MyProfileButton(object sender, EventArgs e)//obvious
        {
             await Navigation.PushAsync(new ProfilePage(User));
        }
        private async void LeaderboardButton(object sender, EventArgs e)//obvious
        {
            await Navigation.PushAsync(new LeaderboardPage());
        }
        private async void SwipetestButton(object sender, EventArgs e)//obvious
        {
            await Navigation.PushAsync(new SwipeTest());
        }
        private async void ChartTestButton(object sender, EventArgs e)//obvious
        {
            await Navigation.PushAsync(new ChartTest());
        }
        private async void HashTestButton(object sender, EventArgs e)//obvious
        {
            await Navigation.PushAsync(new hashtest());
        }
    }
}