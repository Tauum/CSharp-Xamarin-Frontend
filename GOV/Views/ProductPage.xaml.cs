using GOV.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductPage : ContentPage
    {
        public User User { get; set; } //recieve user object from preious page
        private Product _product;
        public Product Product
        {
            get => _product; //after deleting product is still instanciated here
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }
        public ProductPage(User user, Product product) //default 
        {
            User = user;
            Product = product;
            InitializeComponent();
            BindingContext = this;
        }
        public ProductPage() { }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (User == null) { await Navigation.PopToRootAsync(); }
            else if (Product == null) { await Navigation.PopAsync(); }
            else
            {
                if (User.Admin == false) { MenuItem1.IsEnabled = false; } // this works but half loads the visual element
                else { MenuItem1.IsEnabled = true; }
            }
            
        }
        public async void ReviewButton(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ReviewPage(User, Product)); // goes to review page
        }
        public async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductEntryPage(Product) );
        }
    }
}