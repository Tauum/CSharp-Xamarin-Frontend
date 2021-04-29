using System;
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
        private User _user;
        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }
        private Review _review;
        public Review Review
        {
            get => _review;
            set
            {
                _review = value;
                OnPropertyChanged(nameof(Review));
            }
        }
        private Product _product;
        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }

        private string _viewStatus;
        public string ViewStatus
        {
            get { return _viewStatus; } // equal to null on load??
            set
            {
               
                this._viewStatus = value;
                OnPropertyChanged(nameof(ViewStatus));
            }
        }
        public ReviewEntryPage() { }
        public ReviewEntryPage(User user, Product product, Review review )
        {
            User = user;
            Product = product;
            Review = review;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Review.UserID == 0 || Review.UserID == null) { UsernameLabel.Text = User.Username.ToString(); }

            if (Review.Visible)
            {
                _viewStatus = "Hide";
            }
            else
            {
                _viewStatus = "Show";
            }
            if (User.Admin == false) { ViewButtonName.IsEnabled = false; }
            else { ViewButtonName.IsEnabled = true; }
            //something here to declare viewstatus????
            BindingContext = this;
        }
        private void ViewButton(object sender, EventArgs e) //contents isnt working
        {
            var btn = (Button)sender;
            if (Review.Visible)//setting button text state depending on review status
            {
                Review.Visible = false;
                btn.Text = "Show";
            }

            else 
            { Review.Visible = true;
                btn.Text = "Hide";
            }
        }
        async void SaveButton(object sender, EventArgs e) // obvious
        {
            //var review = (Review)BindingContext; //review = null here?

            if (Review.ID == 0)
            {
                Review.Product = Product;
                Review.ProductID = Product.ID;
                Review.User = User;
                Review.UserID = User.ID;

                // object reference is not set to an instance here VVVVV
                Console.WriteLine($" id: {Review.ID.ToString()} desc: {Review.Description.ToString()} Vis: {Review.Visible.ToString()}");
                Console.WriteLine($"userid: { Review.UserID.ToString()}");
                Console.WriteLine($"prodid: { Review.ProductID.ToString()}");

                // set visability to true by default?

                // VVVV object is not set to a reference of an object
                
                await App.DataService.InsertAsync(Review); // internal server error
            }
            else 
            {
                Console.WriteLine($" id: {Review.ID.ToString()} desc: {Review.Description.ToString()} Vis: {Review.Visible.ToString()}");
                Console.WriteLine($"userid: { Review.UserID.ToString()}");
                Console.WriteLine($"prodid: { Review.ProductID.ToString()}");
                await App.DataService.UpdateAsync(Review, Review.ID); 
            }

            await Navigation.PopAsync();
        }
        async void DeleteButton(object sender, EventArgs e) //obvious
        {
          //  var review = (Review)BindingContext;//binds product object to local variable
            if (Review.ID != 0)
            {
                await App.DataService.DeleteAsync(Review, Review.ID);
                await Navigation.PopAsync();//kills page }
            }
            else { await DisplayAlert("Error", "This review doesnt exist", "X"); }
        }

    }
}