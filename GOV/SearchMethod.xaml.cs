using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GOV.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchMethod : ContentPage
    {
        public SearchMethod()
        {
            InitializeComponent();
        }

        async void BarcodeScan(object sender, EventArgs e) // using an external nuget package
        {
            //var SearchQR = BindingContext; // dont think i need this??
            var scanner = new ZXing.Mobile.MobileBarcodeScanner(); // localising external nuget package
            var result = await scanner.Scan(); //this links the barcode scan to the variable for searching with

            if (result != null)
            {
                await Navigation.PushAsync(new SearchResults(result.Text, true)); // this pushes the manual search and a bool through too to check version
            }
        }
        private async void ManualSearch(object sender, EventArgs e) // using a search string or nothing to next page
        {
            if (ProductInput.Text.IsNullOrEmpty()) await Navigation.PushAsync(new SearchResults()); // global function to check contents
            else await Navigation.PushAsync(new SearchResults(ProductInput.Text, false)); // this pushes the manual search and a bool through too to check version
        }
    }
}
