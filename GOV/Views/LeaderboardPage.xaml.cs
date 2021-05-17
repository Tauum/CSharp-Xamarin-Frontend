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
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Plugin.SimpleAudioPlayer;
namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeaderboardPage : ContentPage
    {
        ISimpleAudioPlayer Player { get; }

        private const string OrderByScoreDesc = "Score DESC";
        private const string OrderByScoreAsc = "Score ASC";
        private const string OrderByUsernameDesc = "Username DESC";
        private const string OrderByUsernameAsc = "Username ASC";
        /// <summary>
        /// User object
        /// </summary>
        public User User;
        
        public List<Entry> Entries = new();  //revent referencing error
        // public List<User> UserList { get; } = new();
        public string Selected { get; set; }
        public List<User> UserList { get; set; }
        public LeaderboardPage()
        {
            InitializeComponent();
            SortBy.Items.Add(OrderByUsernameAsc);
            SortBy.Items.Add(OrderByUsernameDesc);
            SortBy.Items.Add(OrderByScoreAsc);
            SortBy.Items.Add(OrderByScoreDesc);

            BindingContext = this; // binding needed for the chart

            Chart1.Chart = new BarChart // decleration for the chart is done
            {
                Entries = Entries,  // specifying chart will consist of above information
                IsAnimated = true, // makes the chart loading animated
                LabelTextSize = 45, // sets the font size because its unreadable
                AnimationDuration = TimeSpan.FromSeconds(3) // lenghtens the animation duration
            };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listView.BeginRefresh();
            //listView.EndRefresh();
            //Entries = GenerateElements(); // jumps from 86 to here & user list is null?????
        }

        public void SortByChanged(object sender, EventArgs e)
        {
            Selected = SortBy.Items[SortBy.SelectedIndex];
            SortList(Selected);
        }

        public void SortList(string selected)
        {
            if (selected == OrderByUsernameAsc) { listView.ItemsSource = UserList.OrderBy(x => x.Username); }//System.ArgumentNullException: 'Value cannot be null.Parameter name: source'
            else if (selected == OrderByUsernameDesc) { listView.ItemsSource = UserList.OrderByDescending(x => x.Username); }
            else if (selected == OrderByScoreAsc) { listView.ItemsSource = UserList.OrderBy(x => x.ScoreTotal); }
            else if (selected == OrderByScoreDesc) { listView.ItemsSource = UserList.OrderByDescending(x => x.ScoreTotal); }  // user list is null here
            else { listView.ItemsSource = UserList; }
        }

        async Task LoadList(string selected) // obvously loading all seperate updates
        {
            string searchTerm = "";
            Expression<Func<User, bool>> searchLambda = x => x.Username.Contains("SearchTerm"); // instanciate searchLambda

            string stringLambda = searchLambda.ToString().Replace("SearchTerm", $"{searchTerm}"); //crashes here
            searchLambda = DynamicExpressionParser.ParseLambda<User, bool>(new ParsingConfig(), true, stringLambda);
            UserList = await App.DataService.GetAllAsync<User>(searchLambda, "GetUsersWithRelatedData");

            if (selected != null) { SortList(selected); }
            else { listView.ItemsSource = UserList.OrderByDescending(u => u.ScoreTotal); }

            Entries = GenerateElements();

            //PlaySound("Pop"); AGAIN CRASHES APPLICATION DOGSHIT PLUGIN
        }

        public ICommand RefreshCommand => new Command(async () => // obvious refreshing screen and list function
        {
            IsRefreshing = true;
            await this.LoadList(Selected);
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
        public List<Entry> GenerateElements()
        {
            //UserList.OrderByDescending(x => x.ScoreTotal); //DOESNT RE-ORDER LIST WHYYYYYYY??????????
            if (Entries.Count > 0) { Entries.Clear(); }

            foreach (User User in UserList) // obvious
            {
                Entry user = new Entry(User.ScoreTotal) // generate user entry within list with value being their score total
                {
                    Color = SKColor.Parse("ff3f7f"),
                    //Color = SKColor.Parse($"#{(User.ScoreTotal * 3).ToString().PadLeft(2, '0')}25"), // sets the color of the bar
                    Label = User.Username.ToString(), // sets the label to match users username
                    ValueLabel = User.ScoreTotal.ToString() // sets the value label to match users score total
                };
                Entries.Add(user);
            }
            return Entries;

                //for (int i = 1; i < 5; i++)
                //{
                //    Entry x = new Entry(i)
                //    {
                //        Color = SKColor.Parse($"#{(i * 3).ToString().PadLeft(2, '0')}25"),
                //        Label = i.ToString(),
                //        ValueLabel = i.ToString()
                //    };
                //    Entries.Add(x);
                //}
                //return Entries;
            }

        //private void PlaySound(string mp3) //mp3 fucntions reference this 2 line reduce
        //{
        //    Player.Load($"{mp3}.mp3");
        //    Player.Play();
        //}
    }
}