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
        public string ViewStatus // instanciate shown/hide button
        {
            get { return _viewStatus; }
            set
            {
                _viewStatus = value;
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
            if (User == null) { await Navigation.PopToRootAsync(); }
            else if (Review == null) { await Navigation.PopAsync(); }
            else
            {                                                          //  <return type,input type>
                if (Product == null) { Product = await App.DataService.GetAsync<Product, int>(Review.ProductID); } //needed if accessing via my reviews tab

                if (Review.UserID == 0 || Review.UserID == null) { UsernameLabel.Text = User.Username.ToString(); } // if review doesnt exist show current username

                if (Review.Visible) { ViewStatus = "Shown"; } //button to show if review is shown or hidden
                else { ViewStatus = "Hidden"; }

                if (User.Admin == false) { ViewButtonName.IsEnabled = false; }

                BindingContext = this;
            }
        }

        private void ViewButton(object sender, EventArgs e) //contents isnt working
        {
            var btn = (Button)sender;
            if (Review.Visible) //setting button text state depending on review status
            {
                Review.Visible = false;
                btn.Text = "Hidden"; //change button text
            }
            else 
            {   
                Review.Visible = true;
                btn.Text = "Shown"; //change button text
            }
        }

        async void SaveButton(object sender, EventArgs e) // obvious
        {
            if (Review.ID == 0)
            {
                Review.ProductID = Product.ID;
                Review.Product = null; //needs to be null to prevent crash on webAPI end
                Review.User = null; //needs to be null to prevent crash on webAPI end
                Review.UserID = User.ID;
                Review.Visible = true; //sets new review to visable

              //  await App.DataService.UpdateAsync(User, User.ID);

                await App.DataService.InsertAsync(Review);
            }
            else  { await App.DataService.UpdateAsync(Review, Review.ID); }
            await Navigation.PopAsync();
        }

        async void DeleteButton(object sender, EventArgs e) //obvious
        {
            int productScore = Product.Score;
            if (Review.ID != 0)
            {
                await App.DataService.DeleteAsync(Review, Review.ID);
                if (User.ScoreTotal - productScore >= 0)
                {
                    User.ScoreTotal -= productScore;
                    await App.DataService.UpdateAsync(User, User.ID);
                }
                await Navigation.PopAsync();//kills page }
            }
            else { await DisplayAlert("Error", "This review doesnt exist", "X"); }
        }
    }
}