using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using GOV.Helpers;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewPage : ContentPage
    {
        //public bool EnableHideButton
        //{
        //    get
        //    {
        //        if (User.Admin == 0) {return false; }
        //        else { return true; }
        //    }
        //}

        public Product Product { get; set; } //recieve user object from preious page
        public User User { get; set; } //recieve user object from preious page
        public ReviewType ReviewType { get; set; } //needed to check the type and input into loadlist
        public Review Review { get; set; }
        public ReviewPage(User user) // instanciates calling user
        {
            User = user;
            ReviewType = ReviewType.User;
            InitializeComponent();
            Device.SetFlags(new[] { "CarouselView_Experimental", "IndicatorView_Experimental" }); // this is needed to do special xamarin stuff
            BindingContext = this;
        }
        public ReviewPage(User user, Product product) // instanciates calling user and product
        {
            User = user;
            Product = product;
            ReviewType = ReviewType.Product;
            InitializeComponent();
            Device.SetFlags(new[] { "CarouselView_Experimental", "IndicatorView_Experimental" }); // this is needed to do special xamarin stuff
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            //if (User.Admin == 0) { HideButtonRef.IsEnabled = false; } // this doesnt work for some reason
            //else {HideButtonRef.IsEnabled = true; }
            base.OnAppearing();
            await LoadList();
        }

        async Task LoadList()
        {
            List<Review> reviewList;
            Expression<Func<Review, bool>> searchLambda = null; // instanciate searchLambda
            
            if (ReviewType == ReviewType.User) { searchLambda = x => x.UserID.ToString().Equals("SearchTerm"); }

            else if (ReviewType == ReviewType.Product) { searchLambda = x => x.ProductID.ToString().Equals("SearchTerm"); }

            if (searchLambda != null)
            {
                var searchID = ReviewType == ReviewType.Product ? Product.ID : User.ID;
                var stringLambda = searchLambda.ToString().Replace("SearchTerm", $"{searchID}");
                searchLambda = DynamicExpressionParser.ParseLambda<Review, bool>(new ParsingConfig(), true, stringLambda);
                reviewList = await App.DataService.GetAllAsync<Review>(searchLambda);
            }

            else { reviewList = await App.DataService.GetAllAsync<Review>(); }

            MainCarousel.ItemsSource = reviewList;
        }

        async void SaveButton(object sender, EventArgs e) // obvious
        {
            var selectedProduct = (Product)BindingContext; // maybe this is the error but needed for here >>>>>>>>>>>>>>>>>>>VVVVVVVV
            var revList = await App.DataService.GetAllAsync<Review>(x => x.ProductID.Equals(selectedProduct.ID));
            // this seems stupid double loading ^^^^^^^^
            bool foundReview = revList.Any(x => x.UserID == User.ID);

            if (foundReview)
            {
                Review.Description = ReviewInput.Text;
                //something here to add score to scoretotal from product to user object
                await App.DataService.UpdateAsync(Review, Review.ID);
                await DisplayAlert("Done", "updated existing review", "X");
            }
            else
            {
                BindingContext = new Review();
                Review.Description = ReviewInput.Text;
                Review.User = User;
                Review.UserID = User.ID;
                Review.Product = Product;
                Review.ProductID = Product.ID;
                await App.DataService.InsertAsync(Review);
                await DisplayAlert("Done", "Inserted new review", "X");
            }
        }

        private async void DeleteButton(object sender, EventArgs e) // obvious
        {
            var selectedReview = (Review)BindingContext;
            var revList = await App.DataService.GetAllAsync<Review>(x => x.ProductID.Equals(selectedReview.ProductID));
            bool foundReview = revList.Any(x => x.UserID == User.ID);

            if (foundReview)
            {
                var review = (Review)BindingContext; //binds product object to local variable
                await App.DataService.DeleteAsync(review, review.ID);
                //something here to remove score
                await Navigation.PopAsync();
            }
            else { await DisplayAlert("Error", "A review for this user does not exist", "X"); }
        }

        private async void HideButton(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var Review = (Review)BindingContext;
            if (btn != null)
            {
                if (Review.Visible)
                {
                    await App.DataService.UpdateAsync(Review, Review.ID);
                    btn.Text = "Show";
                }
                else
                {
                    await App.DataService.UpdateAsync(Review, Review.ID);
                    btn.Text = "Hide";
                }

                Review.Visible = !Review.Visible;
            }
        }

    }
}


