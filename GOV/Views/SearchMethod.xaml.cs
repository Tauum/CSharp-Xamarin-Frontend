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

        async void BarcodeScan(object sender, EventArgs e) //using an external nuget package
        {
            //var SearchQr = BindingContext; //dont think i need this??
            var scanner = new ZXing.Mobile.MobileBarcodeScanner(); //localising nuget package
            var result = await scanner.Scan(); //link barcode scan to variable for searching

            if (result != null)
            {
                await Navigation.PushAsync(new SearchResults(result.Text, true)); //pushes manual search & bool to check version
            }
        }
        private async void ManualSearch(object sender, EventArgs e) //search string or nothing to next page
        {
            if (ProductInput.Text.IsNullOrEmpty()) await Navigation.PushAsync(new SearchResults()); //global function to check contents
            else await Navigation.PushAsync(new SearchResults(ProductInput.Text, false)); //pushes manual search & bool to check version
        }
    }
}
