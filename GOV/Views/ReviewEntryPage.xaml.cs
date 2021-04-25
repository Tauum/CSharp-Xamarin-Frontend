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
        public ReviewEntryPage(User user, Product product, Review review )
        {
            User = user;
            Product = product;
            Review = review;
            InitializeComponent();
           // BindingContext = this;
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
        }
        async void SaveButton(object sender, EventArgs e) // obvious
        {       
            //var review = (Review)BindingContext; //review = null here?

            if (Review.ID == 0) 
            {
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