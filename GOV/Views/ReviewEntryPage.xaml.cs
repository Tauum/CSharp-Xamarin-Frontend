using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GOV.Models;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

using Plugin.SimpleAudioPlayer;

namespace GOV.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewEntryPage : ContentPage
    {
        private ISimpleAudioPlayer Player { get; }
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
        private UserProduct _userProduct;
        public UserProduct UserProduct
        {
            get => _userProduct;
            set
            {
                _userProduct = value;
                OnPropertyChanged(nameof(UserProduct));
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

        public ReviewEntryPage(User user, Product product, Review review)
        {
         
            InitializeComponent();
            Player = CrossSimpleAudioPlayer.Current; //binds player variable to nuget package
            User = user;
            Product = product;
            Review = review;
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
                Review.ProductID = Product.ID; Review.Product = null; Review.User = null; //needs to be null to prevent crash on webAPI end
                Review.UserID = User.ID; Review.Visible = true; //sets new review to visable
                await App.DataService.InsertAsync(Review);
                UserProduct userProduct = new UserProduct { Product = null, ProductID = Product.ID, User = null, UserID = User.ID }; //generate new instanc
                await App.DataService.InsertAsync(userProduct);
            }
            else { await App.DataService.UpdateAsync(Review, Review.ID); }
            await Navigation.PopAsync();
            PlaySound("Whoosh"); //BUT THIS ONE S
        }

        async void DeleteButton(object sender, EventArgs e) //obvious
        {
            string SearchTerm1; string SearchTerm2;
            if (Review.ID != 0)
            {
                try
                {
                    if (UserProduct == null) //find instance
                    {
                        Expression<Func<UserProduct, bool>> searchLambda = x => x.UserID.ToString().Equals("SearchTerm1") && x.ProductID.ToString().Equals("SearchTerm2"); // instanciate searchLambda
                        if (searchLambda != null)
                        {
                            var stringLambda = searchLambda.ToString().Replace("SearchTerm1", $"{User.ID}").Replace("SearchTerm2", $"{Product.ID}");
                            searchLambda = DynamicExpressionParser.ParseLambda<UserProduct, bool>(new ParsingConfig(), true, stringLambda);
                            var UserProductList = await App.DataService.GetAllAsync<UserProduct>(searchLambda);
                            UserProduct = UserProductList[0];
                        }
                    }
                    await App.DataService.DeleteAsync(Review, Review.ID);
                    await App.DataService.DeleteAsync(UserProduct, UserProduct.ID);
                    await Navigation.PopAsync(); //kills page 
                    PlaySound("Bin");
                }
                catch (Exception a) { await DisplayAlert("Error", a.Message.ToString(), "x"); }
            }
            else { await DisplayAlert("Error", "This review doesnt exist", "X"); }
            
        }
        private void PlaySound(string mp3) //mp3 fucntions reference this 2 line reduce
        {
            Player.Load($"{mp3}.mp3");
            Player.Play();
        }
    }
}

