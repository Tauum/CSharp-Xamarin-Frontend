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
            var SearchQR = BindingContext;
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan(); //this links the barcode scan to the variable for searching with
            var ManualSearch = ProductInput.Text;
            if (result != null)
            {
                SearchQR = result.Text;
                await Navigation.PushAsync(new SearchResults(ManualSearch, true)); // this pushes the manual search and a bool through too to check version
            }
        }
        private async void ManualSearch(object sender, EventArgs e) // using a search string or nothing to next page
        {
            string SearchMan = ProductInput.Text;
            if (SearchMan.IsNullOrEmpty()) await Navigation.PushAsync(new SearchResults()); // global functionto check contents
            else await Navigation.PushAsync(new SearchResults(SearchMan, false)); // this pushes the manual search and a bool through too to check version
        }
    }
}
