using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.SimpleAudioPlayer;
using Xamarin.Forms.Shapes;
using GOV.Models;
using GOV.Helpers;
using GOV.Extensions;
using System.Diagnostics;

namespace GOV
{
    public partial class MainPage : ContentPage
    {
        private ISimpleAudioPlayer Player { get; }
        public MainPage()
        {
            Player = CrossSimpleAudioPlayer.Current;
            InitializeComponent();
            Device.SetFlags(new[] { "Brush_Experimental", "Shapes_Experimental", "SwipeView_Experimental", "CarouselView_Experimental", "IndicatorView_Experimental" });
            Task.Run(AnimateBackground);
        }

        private async void AnimateBackground()
        {
            Action<double> forward = input => bgGradient.AnchorY = input; //it complains about these being local
            Action<double> backward = input => bgGradient.AnchorY = input;

            while (true)
            {
                bgGradient.Animate(name: "forward", callback: forward, start: 0, end: 1, length: 10000, easing: Easing.SinIn);
                await Task.Delay(10000);
                bgGradient.Animate(name: "backward", callback: backward, start: 1, end: 0, length: 10000, easing: Easing.SinIn);
                await Task.Delay(10000);
            }
        }

        public void PlugClick(object sender, System.EventArgs e)
        {
            var buttonClicked = (Button)sender;
            var frameBtn = buttonClicked.FindByName("PlugBtn") as Path;
            if (frameBtn.Opacity == 0)
            {
                frameBtn.Opacity = 1;
                PlaySound("bell");
            }
            else frameBtn.Opacity = 0;
        }

        private async void LoginButton(object sender, EventArgs e)
        {
            if (EmailNameEntry.Text.IsNullOrEmpty() || PasswordEntry.Text.IsNullOrEmpty())
            {
                PlaySound("ding98");
                await DisplayAlert("Login", "Missing Field/s", "X");
            }
            else 
            {
                var users = await App.DataService.GetAllAsync<User>();

                string Hashword = Hashing.GetHash(PasswordEntry.Text);

                //Hashing.CheckHash(Hashword, u.Password);

               // if (users.FirstOrDefault(u => u.Email.ToLower() == EmailEntry.Text.ToLower() && u.Password == Hashword) is User user)

                    //if (users.FirstOrDefault(u => u.Email.ToLower() == EmailNameEntry.Text.ToLower() || u.Username.ToLower() == EmailNameEntry.Text.ToLower() && Hashing.CheckHash(Hashword, u.Password) == true) is User user)
                    if (users.SingleOrDefault(u => u.Email.ToLower() == EmailNameEntry.Text.ToLower() || u.Username.ToLower() == EmailNameEntry.Text.ToLower() && Hashing.CheckHash(Hashword, u.Password) == true) is User user)
                    {
                        Debug.WriteLine($" >>>>>>>>>>>>  LOGIN - CORRECT - ID: { user.ID} Email: {user.Email}  Password:  {user.Password}");
                        PlaySound("bell");
                        await DisplayAlert("Login", "Login Success", "X");
                        await Navigation.PushAsync(new HomePage(user));
                    }

                    else
                    {
                        await DisplayAlert("Login", "Incorrect Field/s", "X");
                        Debug.WriteLine($" Email: {EmailNameEntry.Text} >>>>>>>>>>>>  LOGIN - INCORRECT FIELDS");
                    }
            }
        }

        private async void ResetButton(object sender, EventArgs e)
        {
            if (EmailNameEntry.Text.IsNullOrEmpty())
            {
                PlaySound("ding98");
                await DisplayAlert("Login", "Missing Field/s", "X");
            }
            else
            {
                var Users = await App.DataService.GetAllAsync<User>();
                bool FoundEmail = Users.Any(x => x.Email == EmailNameEntry.Text.ToLower());

                if (FoundEmail)
                {
                    User user = Users.First(x => x.Email == EmailNameEntry.Text.ToLower());
                    Debug.WriteLine($"ID: { user.ID} Email: {user.Email} >>>>>>>>>>>>  RESET - CORRECT");
                    PlaySound("bell");
                    await DisplayAlert("Reset", "Reset Success", "X");
                    //put something here to send reset email!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                }
                else
                {
                    PlaySound("ding98");
                    await DisplayAlert("Reset", "Incorrect Email", "X");
                    Debug.WriteLine($" >>>>>>>>>>>>  RESET - FAIL");
                }
            }
        }

        private async void SignUpButton(object sender, EventArgs e)
        {
            if (EmailNameEntry.Text.IsNullOrEmpty() || PasswordEntry.Text.IsNullOrEmpty())
            {
                PlaySound("ding98");
                await DisplayAlert("Login", "Missing Field/s", "X");
            }
            else
            {
                var users = await App.DataService.GetAllAsync<User>();
                
                //bool foundEmail = users.Any(x => x.Email.ToLower() == EmailEntry.Text.ToLower());
                bool foundEmail = users.Any(u => u.Email.ToLower() == EmailNameEntry.Text.ToLower() || u.Username.ToLower() == EmailNameEntry.Text.ToLower());

                if (foundEmail)
                {
                    PlaySound("ding98");
                    await DisplayAlert("Sign up", "Email Taken", "X");
                    Debug.WriteLine($" >>>>>>>>>>>>  SIGN UP - FAIL");
                }
                else
                {
                    
                    string Hashword = Hashing.GetHash(PasswordEntry.Text);
                    var user = new User(EmailNameEntry.Text, EmailNameEntry.Text.Split('@')[0], Hashword) ;
                    await App.DataService.InsertAsync(user);

                    Debug.WriteLine($"ID: { user.ID} Email: {user.Email} Password: {user.Password} >>>>>>>>>>>>  SIGN UP - CORRECT");
                    PlaySound("bell");
                    await DisplayAlert("Sign up", "Sign up Success", "X");
                }
            }
        }
        private void PlaySound(string mp3)
        {
            Player.Load($"{mp3}.mp3");
            Player.Play();
        }
    }
}
