using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewPage : ContentPage
    {
        public enum ReviewType //gives constants as numbers
        {
            Product, //this would be 0
            User, // this would be 1
            None // this would be 2
        }

        public Product Product { get; set; }
        public User User { get; set; }
        public ReviewType InputMethod { get; set; } //needed to check the type and input into loadlist

        public ReviewPage(User user)
        {
            User = user;
            InputMethod = ReviewType.User;
            InitializeComponent();
            Device.SetFlags(new[] { "CarouselView_Experimental", "IndicatorView_Experimental" }); // this is needed to do special xamarin stuff
            BindingContext = this;
        }

        public ReviewPage(Product product)
        {
            Product = product;
            InputMethod = ReviewType.Product;
            InitializeComponent();
            Device.SetFlags(new[] { "CarouselView_Experimental", "IndicatorView_Experimental" }); // this is needed to do special xamarin stuff
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadList();
        }

        async Task LoadList()
        {
            List<Review> reviewList;

            if (InputMethod == ReviewType.User) // equal to 0
            {
                ParameterExpression param = Expression.Parameter(typeof(Review), "x");
                Expression boby = Expression.Equal(Expression.PropertyOrField(param, "UserID"),
                    Expression.Constant(User.ID, typeof(int)));
                Expression<Func<Review, bool>> filter = Expression.Lambda<Func<Review, bool>>(boby, param);
                reviewList = await App.DataService.GetAllAsync<Review>(filter);

                //reviewList = await App.DataService.GetAllAsync<Review>(x => x.UserID.Equals(User.ID));
                
            }
            else if (InputMethod == ReviewType.Product) // equal to 1
            {

                ParameterExpression param = Expression.Parameter(typeof(Review), "x");
                Expression boby = Expression.Equal(Expression.PropertyOrField(param, "ProductID"),
                    Expression.Constant(Product.ID, typeof(int)));
                Expression<Func<Review, bool>> filter = Expression.Lambda<Func<Review, bool>>(boby, param);
                reviewList = await App.DataService.GetAllAsync<Review>(filter);
            }
            else // equal to 2
            {
                throw new Exception("ReviewType is null."); //prevent crash
            }

            //  if (ReferenceEquals(typeof(User), InputMethod)) { reviewList = await App.DataService.GetAllAsync<Review>(x => x.UserID.Equals(InputMethod)); }
            //  else if (ReferenceEquals(typeof(Product), InputMethod)) { reviewList = await App.DataService.GetAllAsync<Review>(x => x.ProductID.ToString().Contains(InputMethod.ToString())); }

            //   if (InputMethod != null) reviewList = await App.DataService.GetAllAsync<Review>(x => x.UserID.Equals(CallingUser)); //potentially sneaky way to get around single type limiter?
            //  else if InputMethod != null) reviewList = await App.DataService.GetAllAsync<Review>(x => x.ProductID.ToString().Contains(CallingProduct.ToString()));

            MainCarousel.ItemsSource = reviewList;
        }

        private async void SaveButton(object sender, EventArgs e) // obvious
        {
            Console.WriteLine("fucking shoot double");
            //get all reviews for single product

            var selectedReview = (Review)BindingContext;
           
            var revList = await App.DataService.GetAllAsync<Review>(reviewSearch => reviewSearch.ProductID.Equals(selectedReview.ProductID));

            if (revList.Any(x => x.UserID == User.ID))
            {
             //   review.Description = ReviewInput.Text;

                //something here to add score to scoretotal from product to user object

             //   await App.DataService.UpdateAsync(review, review.ID);
            }
            else
            {
                User user = new User();
                Product product = new Product();
                var id = 1; // do i need aditional constructor to negate this?
                var userid = 1;
                var productid = 1;
                var desc = "blah";
                var vis = 1;
                await App.DataService.InsertAsync(new Review(id, userid, user, productid, product, desc, vis));
            }

        }
        private async void DeleteButton(object sender, EventArgs e) // obvious
        {
            Console.WriteLine("shoot me");
            // check if a review already exists from the user
            // if not dont do anything
        }
    }
}

