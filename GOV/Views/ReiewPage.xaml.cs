using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewPage : ContentPage
    {
        public ReviewPage()
        {
            InitializeComponent();
            Device.SetFlags(new[] { "CarouselView_Experimental", "IndicatorView_Experimental" }); // this is needed to do special xamarin stuff
            
            List<test> fuckit = new List<test>(); //basic test slightly more advanced
            fuckit.Add(new test { ID = 1, Username = "aaa", ProductID = 1, Description = "desc1"});
            fuckit.Add(new test { ID = 2, Username = "bbb", ProductID = 1, Description = "desc2"});
            fuckit.Add(new test { ID = 3, Username = "ccc", ProductID = 1, Description = "desc3"});
            MainCarousel.ItemsSource = fuckit; //ensures the carousels item list is based from this c# list
        }

        private async void SaveButton(object sender, EventArgs e) // obvious
        {
            Console.WriteLine("fucking shoot double time");
        }
        private async void DeleteButton(object sender, EventArgs e) // obvious
        {
            Console.WriteLine("fucking shoot me");
        }
        public class test //temporary testing
        {
            public int ID { get; set; }
            public string Username { get; set; }
            public int ProductID { get; set; }
            public string Description { get; set; }
            public int PointGiven { get; set; }
        }



    }
}

