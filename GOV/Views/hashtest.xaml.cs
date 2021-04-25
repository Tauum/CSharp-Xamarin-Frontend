using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using GOV.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace GOV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class hashtest : ContentPage
    {
        public hashtest()
        {
            var model = new Model();
            BindingContext = model;
            InitializeComponent();
        }
        private async void ButtonPressed(object sender, EventArgs e)
        {
            if (InputString.Text != null)
            {
                var model = BindingContext as Model;
                model.A = InputString.Text;
                model.B = Hashing.GetHash(model.A);
            }
            else { await DisplayAlert("Error", "Missing Field/s", "X"); }
        }
        private async void ButtonPressed2(object sender, EventArgs e)
        {
            if (InputString.Text != null && InputString2.Text != null)
            {
                var model = BindingContext as Model;
                model.A = InputString.Text;
                model.B = Hashing.GetHash(model.A);
                model.C = InputString2.Text;
                model.D = Hashing.CheckHash(model.C, model.B);
            }
            else { await DisplayAlert("Error", "Missing Field/s", "X"); }
        }
        /// <summary>
        //i hate this shit. I have no fing idea why this has to be done
        // because this stupid fing code doesnt bind without it
        /// </summary>
        public class Model : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public string a;
            public string b;
            public string c;
            public bool d;

            public string A
            {
                get => a;
                set => Setter(value, ref a, nameof(A));
            }
            public string B
            {
                get => b;
                set => Setter(value, ref b, nameof(B));
            }
            public string C
            {
                get => c;
                set => Setter(value, ref c, nameof(C));
            }
            public bool D
            {
                get => d;
                set => Setter(value, ref d, nameof(D));
            }

            public void Setter<T>(T value, ref T field, string name)
            {
                if (!object.Equals(value, field))
                {
                    field = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                }
            }
        }
    }
}