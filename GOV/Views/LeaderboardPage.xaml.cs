using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;
using GOV.Models;// needed to build th user objct
using GOV.Helpers; // might be needed
using Microcharts; //needed for generating charts
using Entry = Microcharts.ChartEntry; //needed and minimises referencing
using SkiaSharp; // used to specify different special colours

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeaderboardPage : ContentPage
    {
        public User User; //instanciate user object
        List<Entry> entries = new List<Entry>(); //prevent referencing error
        public async Task<List<Entry>> GenerateElements()
        {
           // var users = await App.DataService.GetAllAsync<User>(); // grabs all users from data service

            //foreach (User User in users) // obvious
            //{
            //    Entry user = new Entry(User.ScoreTotal) // generate user entry within list with value being their score total
            //    {
            //        Color = SKColor.Parse($"#{(User.ScoreTotal * 3).ToString().PadLeft(2, '0')}25"), // sets the color of the bar
            //        Label = User.Username.ToString(), // sets the label to match users username
            //        ValueLabel = User.ScoreTotal.ToString() // sets the value label to match users score total
            //    };
            //    entries.Add(user);
            //}

            for (int i = 1; i < 5; i++)
            {
                Entry x = new Entry(i)
                {
                    Color = SKColor.Parse($"#{(i * 3).ToString().PadLeft(2, '0')}25"),
                    Label = i.ToString(),
                    ValueLabel = i.ToString()
                };
                entries.Add(x);
            }
            return entries;
        }

        public LeaderboardPage()
        {
            entries = GenerateElements().Result; // calls generate elements function
            InitializeComponent();
            BindingContext = this; // binding needed for the chart
            Chart1.Chart = new BarChart // decleration for the chart is done
            {
                Entries = entries,  // specifying chart will consist of above information
                IsAnimated = true, // makes the chart loading animated
                LabelTextSize = 45, // sets the font size because its unreadable
                AnimationDuration = TimeSpan.FromSeconds(2) // lenghtens the animation duration\
            };
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadList(); // calls load list method on opening page
        }

        async Task LoadList() // obvously loading all seperate updates
        {
            var UserList = await App.DataService.GetAllAsync<User>(); //grabbing all users on load
            listView.ItemsSource = UserList.OrderByDescending(u => u.ScoreTotal); // assigning users to list on liad
            await GenerateElements(); // calling generate elements on load
        }
        public ICommand RefreshCommand => new Command(async () => // obvious refreshing screen and list contents function
        {
            IsRefreshing = true;
            await this.LoadList();
            IsRefreshing = false;
        });

        private bool isRefreshing = false;
        public bool IsRefreshing // more refreshing contents
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
    }
}