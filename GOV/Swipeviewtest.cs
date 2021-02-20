using System;

using Xamarin.Forms;

namespace GOV
{
    public class Swipeviewtest : ContentPage
    {
        public Swipeviewtest()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

