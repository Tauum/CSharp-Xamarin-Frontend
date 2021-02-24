using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GOV.Models;
using Xamarin.Essentials;
using System.IO;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductEntryPage : ContentPage
    {
        public ProductEntryPage()
        {
            InitializeComponent();
        }

        async void SaveButton(object sender, EventArgs e)
        {
            var product = (Product)BindingContext;

            if (product.Image != null && product.Image.ID == 0)
            {
                var image = await App.DataService.InsertAsync(product.Image);

                if (image == null) throw new Exception("Image - data service error");    

                product.Image = null;
                product.ImageId = image.ID;
            }

            if (product.ID == 0) await App.DataService.InsertAsync(product); 
            
            else await App.DataService.UpdateAsync(product, product.ID); 
            
            await Navigation.PopAsync();
        }

        async void DeleteButton(object sender, EventArgs e)
        {
            var product = (Product)BindingContext;
            await App.DataService.DeleteAsync(product, product.ID);
            await App.DataService.DeleteAsync(new Models.Image(), product.ImageId);
            await Navigation.PopAsync();
        }

        async void BarcodeScan(object sender, EventArgs e)
        {
            var product = (Product)BindingContext;
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();

            if (result != null) product.PRef = result.Text;

        }
        async void TakePictureButton(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"TakePictureButton {ex.Message}");
            }
        }
        async void ChoosePictureButton(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                await LoadPhotoAsync(photo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ChoosePictureButton {ex.Message}");
            }
        }
        async Task LoadPhotoAsync(FileResult photo)
        {
            if (photo == null) return;

            var product = (Product)BindingContext;
            using (var memoryStream = new MemoryStream())
            {
                using (var inputStream = await photo.OpenReadAsync())
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
                        throw new Exception("Data service error");
                    }
                    product.Image = image;
                }
            }
            
        }
        public async void ReviewButton(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ReviewPage());
        }
    }
}
