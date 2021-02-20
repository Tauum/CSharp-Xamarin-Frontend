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
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }
        async void SaveButton(object sender, EventArgs e)
        {
            var user = (Product)BindingContext;
            if (user.ID == 0)
            {
             //im not sure what to replace here because user should already exist if having access?
            }
            else
            {
                await App.DataService.UpdateAsync(user, user.ID);
            }
            await Navigation.PopAsync();
        }



    }
}