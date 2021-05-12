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
using GOV.Views;
using GOV.Helpers;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResultsPage : ContentPage
    {
        public User User { set; get; }
        public string SearchTerm { get; set; }
        private SearchType SearchType { get; set; } //setting the seach type
        public string Selected { get; set; }
        public List<Product> ProductList { get; set; }
        public SearchResultsPage() { }

        public SearchResultsPage(User user, SearchType searchType = SearchType.None, string searchTerm = default) // checks to see if search term has contents
        {
            InitializeComponent();
            User = user;
            SearchType = searchType; // setting input as local
            SearchTerm = searchTerm; // setting input as local
            BindingContext = this;
        }
        public SearchResultsPage(User user, SearchType searchType = SearchType.User) // checks to see if search term has contents
        {
            InitializeComponent();
            User = user;
            SearchType = searchType; // setting input as local
            SearchTerm = User.ID.ToString();
            BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (User == null) { await Navigation.PopToRootAsync(); }
            else
            {
                SortBy.Items.Add("Title ASC"); //puts itms into sort by list in xaml
                SortBy.Items.Add("Title DESC");
                SortBy.Items.Add("Score ASC");
                SortBy.Items.Add("Score DESC");

                MenuItem1.IsEnabled = User.Admin;
                listView.BeginRefresh();
            }
        }
        public void SortByChanged(object sender, EventArgs e)
        {
            Selected = SortBy.Items[SortBy.SelectedIndex]; //reads selected option
            SortList(Selected);
        }

        public void SortList(string Selected) //obvious
        {
            if (Selected == "Title ASC") { listView.ItemsSource = ProductList.OrderBy(x => x.Name); }
            else if (Selected == "Title DESC") { listView.ItemsSource = ProductList.OrderByDescending(x => x.Name); }
            else if (Selected == "Score ASC") { listView.ItemsSource = ProductList.OrderBy(x => x.Score); }
            else if (Selected == "Score DESC") { listView.ItemsSource = ProductList.OrderByDescending(x => x.Score); }
            else { listView.ItemsSource = ProductList; }
        }

        async Task LoadList(string Selected)
        {
            Expression<Func<Product, bool>> searchLambda = null; // instanciate searchLambda

            if (SearchType == SearchType.QrCode) { searchLambda = x => x.PRef.Contains("SearchTerm"); }

            else if (SearchType == SearchType.Manual) { searchLambda = x => x.Name.Contains("SearchTerm"); }

            else if (SearchType == SearchType.User)
            {
                ProductList = await App.DataService.GetAllAsync<Product, int>(User.ID, "GetProductsForUser");
            }

            if (searchLambda != null && SearchType == SearchType.Manual || SearchType == SearchType.QrCode)
            {
                var stringLambda = searchLambda.ToString().Replace("SearchTerm", $"{SearchTerm}");
                searchLambda = DynamicExpressionParser.ParseLambda<Product, bool>(new ParsingConfig(), true, stringLambda);
                ProductList = await App.DataService.GetAllAsync<Product>(searchLambda, "GetProductsWithRelatedData");
            }

            else if (searchLambda == null && SearchType != SearchType.User)
            {
                ProductList = await App.DataService.GetAllAsync<Product>(null, "GetProductsWithRelatedData");
            }

            if (Selected != null) { SortList(Selected); } //calls sort function if element selected
            else { listView.ItemsSource = ProductList; } //default loading list displayed
        }

        async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null) await Navigation.PushAsync(new ProductPage(User, e.SelectedItem as Product));
        }
        async void AddProductButton(object sender, EventArgs e) { await Navigation.PushAsync(new ProductEntryPage { Product = new Product() }); }

        async void CategoriesButton(object sender, EventArgs e) { await Navigation.PushAsync(new CategoriesPage(User)); }

        public ICommand RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
            await this.LoadList(Selected);
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