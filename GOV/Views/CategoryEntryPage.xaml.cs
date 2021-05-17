using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GOV.Models;
using Plugin.SimpleAudioPlayer;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GOV.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryEntryPage : ContentPage
    {
        private ISimpleAudioPlayer Player { get; }
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
            InitializeComponent();
            Player = CrossSimpleAudioPlayer.Current; //binds player variable to nuget package
            Category = category;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Category == null) { await Navigation.PopAsync(); }
            else { BindingContext = this; }
        }
        
        async void SaveButton(object sender, EventArgs e) //obvious
        {
            Category.Name = CategoryNameInput.Text.ToString();
            if (Category.ID == 0)
            {
                await App.DataService.InsertAsync(Category); //sends to data service 
                await Navigation.PopAsync(); // return to old page
            }
            else
            {
                await App.DataService.UpdateAsync(Category, Category.ID);
                await Navigation.PopAsync(); // return to old page

                PlaySound("Whoosh");
            }
            //PlaySound("Whoosh"); // AGAIN CRASHES PROGRAM FOR LITERALLY NO REASON
        }

        async void DeleteButton(object sender, EventArgs e) //obvious
        {
            if (Category.ID != 0)
            {
                try { await App.DataService.DeleteAsync(Category, Category.ID); }

                catch (System.Exception ex) { await DisplayAlert("Error", ex.ToString(), "X"); }
            }
            else { await DisplayAlert("Error", "This category doesnt exist", "X"); }
            PlaySound("Bin");
            await Navigation.PopAsync(); // return to old page

        }
        private void PlaySound(string mp3) //mp3 fucntions reference this 2 line reduce
        {
            Player.Load($"{mp3}.mp3");
            Player.Play();

        }
    }
}