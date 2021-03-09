using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GOV.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToolBarTest : ContentPage
    {
        public User User { get; set; }

        public ToolBarTest(User user)
        {
            User = user;
            BindingContext = this;

          //  if (User.Admin == 0) { MenuItem1.IsEnabled = false; }

          //  else { MenuItem1.IsEnabled = true;}

            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            if (User.Admin == 0) { MenuItem1.IsEnabled = false; } // this works but half loads the visual element??????????????????????????????
            else { MenuItem1.IsEnabled = true; }
            base.OnAppearing();
        }

        public ToolBarTest() { }

        private async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            if (User.Admin == 0) { await DisplayAlert("Admin Test", "False", "X"); }

            else { await DisplayAlert("Admin Test", "True", "X"); }
        }
        
    }

}