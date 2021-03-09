using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductPage : ContentPage
    {
        public ProductPage() //default 
        {
            InitializeComponent(); 
        }
        public async void ReviewButton(object sender, System.EventArgs e)
        {
            var product = (Product) BindingContext;
            await Navigation.PushAsync(new ReviewPage(product)); // goes to review page
        }
        public async void EditProductButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductEntryPage()  // goes to product entry page
            { 
                BindingContext = BindingContext // obvious
            });
        }
    }
}

