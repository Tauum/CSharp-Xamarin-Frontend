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

        async void BarcodeScan(object sender, EventArgs e)
        {
            var SearchQR = BindingContext;
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();
            var ManualSearch = ProductInput.Text;
            if (result != null)
            {
                SearchQR = result.Text;
                await Navigation.PushAsync(new SearchResults(ManualSearch, true));
            }
        }
        private async void ManualSearch(object sender, EventArgs e)
        {
            string SearchMan = ProductInput.Text;
            if (SearchMan.IsNullOrEmpty()) await Navigation.PushAsync(new SearchResults());
            else await Navigation.PushAsync(new SearchResults(SearchMan, false));
        }
    }
}
