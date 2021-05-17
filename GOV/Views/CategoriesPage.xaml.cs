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

namespace GOV.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriesPage : ContentPage
    {
        public User User { get; set; }
        public List<Category> CategoriesList { get; set; }
        public string Selected { get; set; }
        public CategoriesPage() { }

        private const string SortByTitleAsc = "Title ASC";
        private const string SorByTiteDesc = "Title DESC";
        private const string SortByIDAsc = "ID ASC";
        private const string SortByIDDesc = "ID DESC";

        public CategoriesPage(User user)
        {
            InitializeComponent();
            SortBy2.Items.Add(SortByTitleAsc); //puts itms into sort by list in xaml
            SortBy2.Items.Add(SorByTiteDesc);
            SortBy2.Items.Add(SortByIDAsc);
            SortBy2.Items.Add(SortByIDDesc);
            User = user;
            BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (User == null) { await Navigation.PopToRootAsync(); }
            else
            {
                MenuItem1.IsEnabled = User.Admin;
                listView.BeginRefresh();
            }
        }

        public void SortByChanged(object sender, EventArgs e)
        {
            Selected = SortBy2.Items[SortBy2.SelectedIndex]; //reads selected option
            SortList(Selected);
        }

        public void SortList(string Selected) //obvious
        {
            if (Selected == SortByTitleAsc) { listView.ItemsSource = CategoriesList.OrderBy(x => x.Name); }
            else if (Selected == SorByTiteDesc) { listView.ItemsSource = CategoriesList.OrderByDescending(x => x.Name); }
            else if (Selected == SortByIDAsc) { listView.ItemsSource = CategoriesList.OrderBy(x => x.ID); }
            else if (Selected == SortByIDDesc) { listView.ItemsSource = CategoriesList.OrderByDescending(x => x.ID); }
            else { listView.ItemsSource = CategoriesList; }
        }

        async Task LoadList(string Selected)
        {
            CategoriesList = await App.DataService.GetAllAsync<Category>();
            if (Selected != null) { SortList(Selected); }
            else { listView.ItemsSource = CategoriesList; }
        }

        async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null && User.Admin) await Navigation.PushAsync(new CategoryEntryPage(e.SelectedItem as Category));
        }

        async void AddCategoryButton(object sender, EventArgs e) { await Navigation.PushAsync(new CategoryEntryPage { Category = new Category() }); }

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