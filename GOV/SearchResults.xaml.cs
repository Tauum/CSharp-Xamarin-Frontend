using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;
using Xamarin.Forms;
using GOV.Models;
using GOV.Helpers;
using System.Diagnostics;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResults : ContentPage
    {
        public ICommand RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
            await this.LoadList();
            IsRefreshing = false;
        });

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
        public string SearchQR { get; private set; }
        public string SearchMan { get; private set; }
        public SearchResults(string searchTerm, bool isQR)
        {
            if (isQR) { SearchQR = searchTerm; Debug.WriteLine(">>>>>>>>>>>>>>> manual search passed"); } //this set true by calling method
            else { SearchMan = searchTerm; Debug.WriteLine(">>>>>>>>>>>>>>> qr code search passed"); }
            InitializeComponent();
            BindingContext = this;
        }
        public SearchResults(User Human)
        {
            //somehow check if calling function has inputed a user??
            Debug.WriteLine(">>>>>>>>>>>>>>> user has been passed");
            InitializeComponent();
            BindingContext = this;
        }
        public SearchResults()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async Task RefreshProducts() // just put load list in here???
        { 
            listView.ItemsSource = await App.DataService.GetAllAsync<Product>(); 
        }

        async Task LoadList()
        {
            List<Product> ProductList;

            if (SearchQR != null) ProductList = await App.DataService.GetAllAsync<Product>(x => x.PRef.Contains(SearchQR));
            else if (SearchMan != null) ProductList = await App.DataService.GetAllAsync<Product>(x => x.Name.Contains(SearchMan)); //this doesnt work

           // else if (SearchMan != null) ProductList = await App.DataService.GetAllAsync<Product>(x => x.Name.Contains(SearchMan));
           // loaded with a user??? //grab all products contained in reviewes where = to userid

            else ProductList = await App.DataService.GetAllAsync<Product>();

            //ProductList = await ((SearchQR != null)
            //? App.DataService.GetAllAsync<Product>(x => x.PRef.Contains(SearchQR))
            //: (SearchMan != null)
            //? App.DataService.GetAllAsync<Product>(x => x.Name.Contains(SearchMan))
            //: App.DataService.GetAllAsync<Product>());

            //string SearchQr;
            //ProductList = await App.DataService.GetAllAsync<Product>(
            //SearchQr is not null
            //? x => x.PRef.Contains(SearchQR)
            //: SearchMan != null
            //? x => x.Name.Contains(SearchMan)
            //: x => true
            //);

            listView.ItemsSource = ProductList;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadList();
        }

        async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null) await Navigation.PushAsync(new ProductPage { BindingContext = e.SelectedItem as Product });
        }
        async void AddProductButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductEntryPage{ BindingContext = new Product()});
        }
    }
}