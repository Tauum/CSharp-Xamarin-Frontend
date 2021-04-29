﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GOV.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GOV.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryEntryPage : ContentPage
    {
        private Category _category;
        public Category Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }
        public CategoryEntryPage() { InitializeComponent(); }
        public CategoryEntryPage(Category category)
        {
            Category = category;
            InitializeComponent();
        }

        async void SaveButton(object sender, EventArgs e) //obvious
        {

            Category.Name = CategoryNameInput.Text.ToString();

            if (Category.ID == 0)  await App.DataService.InsertAsync(Category); //sends to data service 
            else await App.DataService.UpdateAsync(Category, Category.ID);

            await Navigation.PopAsync();//kills page
        }

        async void DeleteButton(object sender, EventArgs e) //obvious
        {
            if (Category.ID != 0)
            {
                try
                {
                    await App.DataService.DeleteAsync(Category, Category.ID);

                    //await App.DataService.DeleteAsync()
                    //maybe make a deleteallasync like getallasync?
                    //or linq to grab all reviews associated to product then delete in forloop?
                    //then remove score from user accounts that have item
                }
                catch (System.Exception ex) { await DisplayAlert("Error", ex.ToString(), "X"); }
            }
            else { await DisplayAlert("Error", "This product doesnt exist", "X"); }
            await Navigation.PopAsync();//kills page
        }
    }
}