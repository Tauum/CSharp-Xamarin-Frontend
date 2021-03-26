using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text;
using GOV.Helpers;
using GOV.Views;
using System.ComponentModel;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewPage : ContentPage
    {
        public Product Product { get; set; } //recieve user object from preious page
        public User User { get; set; } //recieve user object from preious page
        public ReviewType ReviewType { get; set; } //needed to check the type and input into loadlist
        public Review Review { get; set; }
        public List<Review> reviewList { get; set; }
        public ReviewPage(User user) // instanciates calling user
        {
            User = user;
            ReviewType = ReviewType.User;
            InitializeComponent();
            Device.SetFlags(new[] { "CarouselView_Experimental", "IndicatorView_Experimental"}); // this is needed to do special xamarin stuff
            BindingContext = this;
        }
        public ReviewPage(User user, Product product) // instanciates calling user and product
        {
            User = user;
            Product = product;
            ReviewType = ReviewType.Product;
            InitializeComponent();
            Device.SetFlags(new[] { "CarouselView_Experimental", "IndicatorView_Experimental"}); // this is needed to do special xamarin stuff
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            if (ReviewType == ReviewType.Product) { MenuItem1.IsEnabled = true; }
            else { MenuItem1.IsEnabled = false; }
            base.OnAppearing();
            await LoadList();
            DisplayList();
        }

        public async Task<List<Review>> LoadList()
        {
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

            return reviewList;
        }

        public void DisplayList() { MainCarousel.ItemsSource = reviewList; }

        public async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            if (reviewList.Any(x => x.UserID == User.ID)) { 
                await DisplayAlert("Error", "Edit/Delete your existing review", "X"); }
            else { await Navigation.PushAsync( new ReviewEntryPage(User, Product) { Review  = new Review() }); }
        }

        public async void EditReviewButton(object sender, EventArgs e)
        {
            var clickedReview = (Review)((Button)sender).BindingContext;

            if (clickedReview.UserID == User.ID || User.Admin) 
            {
                await Navigation.PushAsync(new ReviewEntryPage(User, Product) { BindingContext = clickedReview as Review }); 
            }
            else { await DisplayAlert("Error", "You do not have permission to edit this review", "X"); }
        }
    }
}