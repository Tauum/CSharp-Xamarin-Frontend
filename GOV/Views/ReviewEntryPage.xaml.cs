﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GOV.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewEntryPage : ContentPage
    {
        public User User { get; set; }
        public Review Review { get; set; }
        public Product Product { get; set; }

        private string _viewStatus;
        public string ViewStatus
        {
            get { return _viewStatus; } // equal to null on load??
            set
            {
                if (Review.Visible) { ViewStatus = "Hide"; }
                else { ViewStatus = "Show"; }
                this._viewStatus = value;
                OnPropertyChanged(nameof(ViewStatus));
            }
        }
        public ReviewEntryPage() { }
        public ReviewEntryPage(User user, Product product)
        {
            User = user;
            Product = product;
            InitializeComponent();
            BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            if (User.Admin == false) { ViewButtonName.IsEnabled = false; }
            else { ViewButtonName.IsEnabled = true; }
            //something here to declare viewstatus????
            base.OnAppearing();
        }
        private void ViewButton(object sender, EventArgs e) //contents isnt working
        {
            var btn = (Button)sender;
            if (Review.Visible) //error object reference not set to an instance of an object????
            {
                Review.Visible = false;
                btn.Text = "Show";
            }

            else 
            { Review.Visible = true;
                btn.Text = "Hide";
            }
         //   if (Review.Visible) { Review.Visible = false; }
          //  else { Review.Visible = true; }
        }
        async void SaveButton(object sender, EventArgs e) // obvious
        {       
            //var review = (Review)BindingContext; //review = null here?

            if (Review.ID == 0) 
            {
                //Review.UserID = User.ID; // object reference is null
                //Review.User = User;
                //Review.ProductID = Product.ID;
                //Review.Product = Product;
                //Review.Description = DescrptionInput.Text;
                //Review.Visible = true;
                 Console.WriteLine($" id: {Review.ID.ToString()} desc: {Review.Description.ToString()} Vis: {Review.Visible.ToString()} userid: {Review.UserID.ToString()} user: {Review.User.ToString()} prodid: {Review.ProductID.ToString()} prod: {Review.Product.ToString()} ");
                await App.DataService.InsertAsync(Review); // internal server error
            }
            else 
            { 
                await App.DataService.UpdateAsync(Review, Review.ID); 
            }

            await Navigation.PopAsync();
        }
        async void DeleteButton(object sender, EventArgs e) //obvious
        {
            var review = (Review)BindingContext;//binds product object to local variable
            await App.DataService.DeleteAsync(review, review.ID);
            await Navigation.PopAsync();//kills page
        }

    }
}