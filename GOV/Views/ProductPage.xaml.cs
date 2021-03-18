﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductPage : ContentPage
    {
        public User User { get; set; } //recieve user object from preious page
        public ProductPage(User user) //default 
        {
            User = user;
            InitializeComponent();
            BindingContext = this;
        }
        public ProductPage() { }
        protected override void OnAppearing()
        {
            if (User.Admin == false) { MenuItem1.IsEnabled = false; } // this works but half loads the visual element??????????????????????????????
            else { MenuItem1.IsEnabled = true; }
            base.OnAppearing();
        }
        public async void ReviewButton(object sender, System.EventArgs e)
        {
            var product = (Product)BindingContext;
            await Navigation.PushAsync(new ReviewPage(User, product)); // goes to review page
        }
        public async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductEntryPage() { BindingContext = BindingContext });
        }
    }
}