using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GOV.Models; //to know how objects are supposed to me modeled
using Xamarin.Essentials;
using System.IO;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductEntryPage : ContentPage
    {
        public ProductEntryPage() //default
        {
            InitializeComponent();
        }

        async void SaveButton(object sender, EventArgs e) //obvious
        {
            var product = (Product)BindingContext;

            if (product.Image != null && product.Image.ID == 0)
            {
                var image = await App.DataService.InsertAsync(product.Image); //sends to data service

                if (image == null) throw new Exception("Image - data service error"); //ensures app doesnt crash and provides error

                product.Image = null;
                product.ImageId = image.ID;
            }

            if (product.ID == 0) await App.DataService.InsertAsync(product); //sends to data service
            
            else await App.DataService.UpdateAsync(product, product.ID); 
            
            await Navigation.PopAsync();//kills page
        }

        async void DeleteButton(object sender, EventArgs e) //obvious
        {
            var product = (Product)BindingContext;//binds product object to local variable
            await App.DataService.DeleteAsync(product, product.ID);
            await App.DataService.DeleteAsync(new Models.Image(), product.ImageId);
            await Navigation.PopAsync();//kills page
        }

        async void BarcodeScan(object sender, EventArgs e)// this uses a nuget package to work [not xamarin]
        {
            var product = (Product)BindingContext;//bind product to local variable
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();

            if (result != null) product.PRef = result.Text;
        }
        async void TakePictureButton(object sender, EventArgs e) //obviouis
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(); //makes phone function to local variable
                await LoadPhotoAsync(photo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TakePictureButton {ex.Message}"); //prevent crash
            }
        }
        async void ChoosePictureButton(object sender, EventArgs e)//obvious
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();//makes phone function to local variable
                await LoadPhotoAsync(photo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ChoosePictureButton {ex.Message}");//prevent crash
            }
        }
        async Task LoadPhotoAsync(FileResult photo)//obvious
        {
            if (photo == null) return;

            Product product = (Product)BindingContext; //bind select local
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

                    image = await App.DataService.InsertAsync(image);
                    if (image == null)
                    {
                        throw new Exception("Data service error"); //prevent crash
                    }
                    product.Image = image; //sets local object image
                }
            }
        }
        public async void ReviewButton(object sender, System.EventArgs e) //obvious
        {
            await Navigation.PushAsync(new ReviewPage());
        }

    }
}
