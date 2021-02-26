using GOV.Data;
using GOV.Models;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GOV
{
    public partial class App : Application
    {
        static DataService dataService;
        public static DataService DataService
        {
            get
            {
                if (dataService == null)
                {
                    dataService =
                    new DataService("https://192.168.0.101:5001")  //change this if ip address changes
                    .AddEntityModelEndpoint<Product>("api/Products")
                    .AddEntityModelEndpoint<Models.Image>("api/Images") //naming violation boycotted
                    .AddEntityModelEndpoint<User>("api/Users")
                    .AddEntityModelEndpoint<Review>("api/Reviews"); //these are obvious
                }
                return dataService;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage()); // load main page
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
