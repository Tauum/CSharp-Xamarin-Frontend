using System;
using System.Collections.Generic; // importnat stuff
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.SimpleAudioPlayer; //play mp3
using Xamarin.Forms.Shapes;
using GOV.Models;
using GOV.Helpers; //for hashing functionality and other stuff
using GOV.Extensions; //used to check if strings contain stuff
using System.Diagnostics; //debug menu?

namespace GOV
{
    public partial class MainPage : ContentPage
    {
        private ISimpleAudioPlayer Player { get; }
        public MainPage()
        {
            Player = CrossSimpleAudioPlayer.Current; //binds player variable to nuget package
            InitializeComponent();
            Device.SetFlags(new[] { "Brush_Experimental", "Shapes_Experimental", "SwipeView_Experimental", "CarouselView_Experimental", "IndicatorView_Experimental" }); //need to be assigned to do other stuff
            Task.Run(AnimateBackground); //this is enabling the background
        }

        private async void AnimateBackground() //obvious
        {
            Action<double> forward = input => bgGradient.AnchorY = input; //sets the forwards transition
            Action<double> backward = input => bgGradient.AnchorY = input; // sets the backwards transition

            while (true) // obvious
            {
                bgGradient.Animate(name: "forward", callback: forward, start: 0, end: 1, length: 9000, easing: Easing.SinIn); //modify gradient with action 
                await Task.Delay(10000);//delay on forward to backwards
                bgGradient.Animate(name: "backward", callback: backward, start: 1, end: 0, length: 9000, easing: Easing.SinIn); //modify gradient with action 
                await Task.Delay(10000);//delay on backwards to forwards
            }
        }

        public void PlugClick(object sender, System.EventArgs e) //change opacity of a path in a button
        {
            var buttonClicked = (Button)sender; //bind button to local variable xaml
            var frameBtn = buttonClicked.FindByName("PlugBtn") as Path; //plugBtn button named in xaml into a type
            if (frameBtn.Opacity == 0)
            {
                frameBtn.Opacity = 1;
                PlaySound("bell");
            }
            else frameBtn.Opacity = 0;
        }

        private async void LoginButton(object sender, EventArgs e) //Obvious
        {
            if (EmailEntry.Text.IsNullOrEmpty() || PasswordEntry.Text.IsNullOrEmpty()) //cool global function to reduce line-age
            {
                PlaySound("ding98");//class function to reduce line-age
                await DisplayAlert("Login", "Missing Field/s", "X");
            }
            else
            {
                var users = await App.DataService.GetAllAsync<User>(); //using data servic to request all users ||||||   V From user objct list only grab 1 result where an email AND password matches
                if (users.SingleOrDefault(u => u.Email.ToLower() == EmailEntry.Text.ToLower() && Hashing.CheckHash(PasswordEntry.Text, u.Password) == true) is User user) // <<< pass user into the loop
                {
                    Debug.WriteLine($" >>>>>>>>>>>>  LOGIN - CORRECT - ID: { user.ID} Email: {user.Email}  Password:  {user.Password}");
                    PlaySound("bell");
                    await DisplayAlert("Login", "Login Success", "X");
                    await Navigation.PushAsync(new HomePage(user));//pass User from loop into homePage
                }

                else
                {
                    await DisplayAlert("Login", "Incorrect Field/s", "X");
                    Debug.WriteLine($" Email: {EmailEntry.Text} >>>>>>>>>>>>  LOGIN - INCORRECT FIELDS");
                }
            }
        }

        private async void ResetButton(object sender, EventArgs e) //obvious
        {
            if (EmailEntry.Text.IsNullOrEmpty()) //global
            {
                PlaySound("ding98");//class function to reduce line-age
                await DisplayAlert("Login", "Missing Field/s", "X");
            }
            else
            {
                var users = await App.DataService.GetAllAsync<User>(); //get all users from data service
                bool foundEmail = users.Any(x => x.Email == EmailEntry.Text.ToLower()); //nice line-age reduce

                if (foundEmail)//windex clean boys
                {
                    User user = users.First(x => x.Email == EmailEntry.Text.ToLower()); //Only grab 1st because only 1 entry is needed
                    Debug.WriteLine($"ID: { user.ID} Email: {user.Email} >>>>>>>>>>>>  RESET - CORRECT");
                    PlaySound("bell");
                    await DisplayAlert("Reset", "Reset Success", "X");
                    //something here to send reset email!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                }
                else
                {
                    PlaySound("ding98");
                    await DisplayAlert("Reset", "Incorrect Email", "X");
                    Debug.WriteLine($" >>>>>>>>>>>>  RESET - FAIL");
                }
            }
        }

        private async void SignUpButton(object sender, EventArgs e) //obvious
        {
            if (EmailEntry.Text.IsNullOrEmpty() || PasswordEntry.Text.IsNullOrEmpty())//global
            {
                PlaySound("ding98");//reduce line-age
                await DisplayAlert("Login", "Missing Field/s", "X");
            }
            else
            {
                var users = await App.DataService.GetAllAsync<User>(); //grab all users from data service

                bool foundEmail = users.Any(u => u.Email.ToLower() == EmailEntry.Text.ToLower()); //nice reduce using global

                if (foundEmail) //windex clean boys
                {
                    PlaySound("ding98");
                    await DisplayAlert("Sign up", "Email Taken", "X");
                    Debug.WriteLine($" >>>>>>>>>>>>  SIGN UP - FAIL");
                }
                else
                {
                    string hashword = Hashing.GetHash(PasswordEntry.Text);// nice global class for generating & checking Bcrypt package hash
                    User user = new User(EmailEntry.Text, EmailEntry.Text.Split('@')[0], hashword); //nice split to set username same as email on default but can be changed in profile
                    await App.DataService.InsertAsync(user);//obvious
                    Debug.WriteLine($"ID: { user.ID} Email: {user.Email} Password: {user.Password} >>>>>>>>>>>>  SIGN UP - CORRECT");
                    PlaySound("bell");
                    await DisplayAlert("Sign up", "Sign up Success", "X");
                }
            }
        }
        private void PlaySound(string mp3) //mp3 fucntions reference this 2 line reduce
        {
            Player.Load($"{mp3}.mp3");
            Player.Play();
        }
    }
}
