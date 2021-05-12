using System;
using System.Collections.Generic; // importnat stuff
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using GOV.Models;
using Plugin.SimpleAudioPlayer; //play mp3
using Xamarin.Forms.Shapes;
using GOV.Helpers; //for hashing functionality and other stuff
using GOV.Extensions; //used to check if strings contain stuff
using System.Diagnostics; //debug menu?
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace GOV
{
    public partial class MainPage : ContentPage
    {
        private ISimpleAudioPlayer Player { get; }
        public MainPage()
        {
            Player = CrossSimpleAudioPlayer.Current; //binds player variable to nuget package
            InitializeComponent();
            Device.SetFlags(new[] { "Brush_Experimental", "Shapes_Experimental", "SwipeView_Experimental" }); //need to be assigned to do other stuff
            Task.Run(AnimateBackground); //this is enabling the background
        }

        private async void AnimateBackground() //obvious
        {
            Action<double> forward = input => BgGradient.AnchorY = input; //sets the forwards transition
            Action<double> backward = input => BgGradient.AnchorY = input; // sets the backwards transition

            while (true) // obvious
            {
                BgGradient.Animate(name: "forward", callback: forward, start: 0, end: 1, length: 9000, easing: Easing.SinIn); //modify gradient with action 
                await Task.Delay(10000);//delay on forward to backwards
                BgGradient.Animate(name: "backward", callback: backward, start: 1, end: 0, length: 9000, easing: Easing.SinIn); //modify gradient with action 
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
                User loginAttempt = await GetUserRequest(EmailEntry.Text.ToLower()); //check username inputted

                if (loginAttempt != null && Hashing.CheckHash(PasswordEntry.Text, loginAttempt.Password) == true) // <<< pass user into the loop
                {
                    User user = loginAttempt as User;
                    PlaySound("bell");
                    await DisplayAlert("Login", "Login Success", "X");
                    await Navigation.PushAsync(new HomePage(user));//pass User from loop into homePage
                }
                else { await DisplayAlert("Login", "Incorrect Field/s", "X"); }
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
                if (await GetUserRequest(EmailEntry.Text.ToLower()) != null)
                {
                    PlaySound("bell");
                    await DisplayAlert("Reset", "Reset Success - check email", "X");
                    //something here to send reset email!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                }
                else
                {
                    PlaySound("ding98");
                    await DisplayAlert("Reset", "Incorrect Email", "X");
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
                if (await GetUserRequest(EmailEntry.Text.ToLower()) == null)
                {
                    string hashword = Hashing.GetHash(PasswordEntry.Text);// nice global class for generating & checking Bcrypt package hash
                    User user = new User(EmailEntry.Text, EmailEntry.Text.Split('@')[0], hashword); //nice split to set username same as email on default but can be changed in profile
                    await App.DataService.InsertAsync(user);//obvious
                    PlaySound("bell");
                    await DisplayAlert("Sign up", "Sign up Success", "X");
                }
                else
                {
                    PlaySound("ding98");
                    await DisplayAlert("Sign up", "Email Taken", "X");
                }
            }
        }

        private async Task<User> GetUserRequest(string email) // check username inputted against list
        {
            string SearchTerm = EmailEntry.Text.ToLower();
            Expression<Func<User, bool>> searchLambda = null;
            searchLambda = x => x.Email.Contains("SearchTerm");
            var stringLambda = searchLambda.ToString().Replace("SearchTerm", $"{SearchTerm}");
            searchLambda = DynamicExpressionParser.ParseLambda<User, bool>(new ParsingConfig(), true, stringLambda);

            var LoginAttempt = await App.DataService.GetAllAsync<User>(searchLambda); // getting only a list where email is stored

            if (LoginAttempt.Count == 0) { return null; } // if the list is empty return null
            else if (LoginAttempt.Count <= 1) { return LoginAttempt[0]; } // if there is an instance return the 1st
            else { return null; } //this is here because if not it cries not all paths return a value
        }

        private void PlaySound(string mp3) //mp3 fucntions reference this 2 line reduce
        {
            Player.Load($"{mp3}.mp3");
            Player.Play();
        }

    }
}
