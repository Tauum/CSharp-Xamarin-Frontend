using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GOV.Models; //to know how objects are supposed to me modeled
using Xamarin.Essentials;
using System.Linq;
using System.Windows.Input;
using System.Text;
using System.IO;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductEntryPage : ContentPage
    {
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

        public Category _selCategory;
        public Category SelCategory
        {
            get => _selCategory;
            set
            {
                _selCategory = value;
                OnPropertyChanged(nameof(SelCategory));
            }
        }

        private IList<Category> _categories;
        public IList<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
        public ProductEntryPage()
        {
            InitializeComponent();
        }
        public ProductEntryPage(Product product) 
        {
            Product = product; // this is null
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Categories == null)
            {
                var _categories = await App.DataService.GetAllAsync<Category>();
                Categories = _categories;
            }
            BindingContext = this;
        }

        async void SaveButton(object sender, EventArgs e) //obvious
        {
            if (Product.Image != null && Product.ImageID == 0)
            {
                var image = await App.DataService.InsertAsync(Product.Image); //sends to data service
                if (image == null) throw new Exception("Image - data service error");

                Product.Image = null;
                Product.ImageID = image.ID;
            }

            if (Product.ID == 0) await App.DataService.InsertAsync(Product); //sends to data service
            else await App.DataService.UpdateAsync(Product, Product.ID); 
            
            await Navigation.PopAsync();//kills page
        }

        async void DeleteButton(object sender, EventArgs e) //obvious
        {
            if (Product.ID != 0)
            {
                await App.DataService.DeleteAsync(Product, Product.ID);
                await App.DataService.DeleteAsync(new Models.Image(), Product.ImageID);
                await Navigation.PopAsync();//kills page
            }
            else { await DisplayAlert("Error", "This product doesnt exist", "X"); }
        }

        async void BarcodeScan(object sender, EventArgs e)// this uses a nuget package to work [not xamarin]
        {
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();
            if (result != null) Product.PRef = result.Text;
        }
        async void TakePictureButton(object sender, EventArgs e) //obviouis
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(); //makes phone function to local variable
                await LoadPhotoAsync(photo);
            }
            catch (Exception ex) { Console.WriteLine($"TakePictureButton {ex.Message}"); } //prevent crash
        }
        async void ChoosePictureButton(object sender, EventArgs e)//obvious
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();//makes phone function to local variable
                await LoadPhotoAsync(photo);
            }
            catch (Exception ex) { Console.WriteLine($"ChoosePictureButton {ex.Message}"); } //prevent crash
        }
        async Task LoadPhotoAsync(FileResult photo)//obvious
        {
            if (photo == null) return;
            using (var memoryStream = new MemoryStream()) //makes a memory stream
            {
                using (var inputStream = await photo.OpenReadAsync()) //this does other stuff
                {
                    await inputStream.CopyToAsync(memoryStream);
                    var image = new Models.Image()
                    {
                        Information = memoryStream.ToArray(),
                        TypeUsed = photo.ContentType,
                        Name = photo.FileName
                    };

                    Product.Image = image; //sets local object image
                }
            }
        }

        private void ScoreStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            lblDisplay.Text = ScoreStepper.Value.ToString();
            Product.Score = Convert.ToInt32(ScoreStepper.Value);
           // Console.WriteLine($" name: {Product.Name.ToString()} score: {Product.Score.ToString()} year: {Product.ReleaseYear.ToString()} Desc: {Product.Description.ToString()}");
        }
    }
}
