using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GOV
{
    public partial class SwipeTest : ContentPage
    {
        public SwipeTest()
        {
            InitializeComponent();
            Device.SetFlags(new[] { "SwipeView_Experimental" });
        }

        private async void button1(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "1", "X"); ;
        }
        
    }
}
