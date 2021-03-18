using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input;
using GOV.Models;
using GOV.Helpers;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResults : ContentPage
    {
        public User User { set; get; }
        public string SearchTerm { get; set; }
        private SearchType SearchType { get; set; } //setting the seach type

        public SearchResults() { }

        public SearchResults(User user, SearchType searchType = SearchType.None, string searchTerm = default) // checks to see if search term has contents
        {
            InitializeComponent();
            User = user;
            SearchType = searchType; // setting input as local
            SearchTerm = searchTerm; // setting input as local
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            if (User.Admin == false) { MenuItem1.IsEnabled = false; } // obvious
            else { MenuItem1.IsEnabled = true; }

            base.OnAppearing();
            listView.BeginRefresh();
        }

        async Task LoadList()
        {
            List<Product> productList;
            Expression<Func<Product, bool>> searchLambda = null; // instanciate searchLambda

            if (SearchType == SearchType.QrCode) { searchLambda = x => x.PRef.Contains("SearchTerm"); }

            else if (SearchType == SearchType.Manual) { searchLambda = x => x.Name.Contains("SearchTerm"); }

            if (searchLambda != null)
            {
                var stringLambda = searchLambda.ToString().Replace("SearchTerm", $"{SearchTerm}");
                searchLambda = DynamicExpressionParser.ParseLambda<Product, bool>(new ParsingConfig(), true, stringLambda);
                productList = await App.DataService.GetAllAsync<Product>(searchLambda);
            }
            else { productList = await App.DataService.GetAllAsync<Product>(); }

            listView.ItemsSource = productList;
        }

        async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null) await Navigation.PushAsync(new ProductPage(User) { BindingContext = e.SelectedItem as Product });
        }
        async void AddProductButton(object sender, EventArgs e) { await Navigation.PushAsync(new ProductEntryPage { BindingContext = new Product() }); }

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
    }
}