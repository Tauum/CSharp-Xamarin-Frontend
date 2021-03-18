using GOV.Data;
using GOV.Models;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GOV
{
    public partial class App : Application
    {
        static DataService2 dataService;
        public static DataService2 DataService
        {
            get
            {
                if (dataService == null)
                {
                    dataService =
                    new DataService2("https://192.168.0.101:5001")  //change this if ip address changes
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
          //  MainPage = new NavigationPage(new MainPage()); // load main page
             MainPage = new NavigationPage(new HomePage(new User(1, "a", "a", "a",500,true)));
           //   MainPage = new NavigationPage(new HomePage(new User(2, "b", "b", "b", 500, 0)));
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
