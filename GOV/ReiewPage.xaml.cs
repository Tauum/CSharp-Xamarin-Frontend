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
            Device.SetFlags(new[] { "CarouselView_Experimental", "IndicatorView_Experimental" });


            var test0 = new List<string>
            {
                "abc","def","ghi"
            };


            List<test> fuckit = new List<test>();

            fuckit.Add(new test { ID = 1, Username = "aaa", ProductID = 1, Description = "desc1", PointGiven = 15 });
            fuckit.Add(new test { ID = 2, Username = "bbb", ProductID = 1, Description = "desc2", PointGiven = 30 });
            fuckit.Add(new test { ID = 3, Username = "ccc", ProductID = 1, Description = "desc3", PointGiven = 45 });
            MainCarousel.ItemsSource = fuckit;
        }

        private async void SaveButton(object sender, EventArgs e)
        {
            Console.WriteLine("fucking shoot double time");
        }
        private async void DeleteButton(object sender, EventArgs e)
        {
            Console.WriteLine("fucking shoot me");
        }
        public class test
        {
            public int ID { get; set; }
            public string Username { get; set; }
            public int ProductID { get; set; }
            public string Description { get; set; }
            public int PointGiven { get; set; }
        }



    }
}

