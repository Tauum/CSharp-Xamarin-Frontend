using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GOV.Models;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public User Human { get; set; }
        public HomePage(User human)
        {
            Human = human;
            BindingContext = this;
            InitializeComponent();
        }
        public HomePage()
        {
            InitializeComponent();
        }
        private async void ViewButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchMethod());
        }
        private async void MyProfileButton(object sender, EventArgs e)
        {
             await Navigation.PushAsync(new ProfilePage());
        }
        private async void LeaderboardButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LeaderboardPage());
        }
        private async void SwipetestButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SwipeTest());
        }
        private async void ChartTestButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChartTest());
        }
        private async void HashTestButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new hashtest());
        }
    }
}