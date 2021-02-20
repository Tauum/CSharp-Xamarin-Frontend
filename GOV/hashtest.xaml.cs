﻿using System;
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
        string a { get; set; }
        string c { get; set; } = "dog";
        public hashtest()
        {
            var model = new Model();
            BindingContext = model;
            InitializeComponent();
        }
        private async void ButtonPressed(object sender, EventArgs e)
        {
            var model = BindingContext as Model;
            model.A = InputString.Text;
            model.B = Hashing.GetHash(model.A);
        }
        private async void ButtonPressed2(object sender, EventArgs e)
        {
            var model = BindingContext as Model;
            model.A = InputString.Text;
            model.B = Hashing.GetHash(model.A);

            model.C = InputString2.Text;
            model.D = Hashing.GetHash(model.C);
            model.E = Hashing.CheckHash(model.B, model.D); //this doesnt work? maybe using different salt????
        }
        /// <summary>
        //i hate this shit. I have no fucking idea why this has to be done
        // because this stupid fucking code doesnt bind without it
        /// </summary>
        public class Model : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public string a;
            public string b;
            public string c;
            public string d;
            public bool e;

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
            public string D
            {
                get => d;
                set => Setter(value, ref d, nameof(D));
            }
            public bool E
            {
                get => e;
                set => Setter(value, ref e, nameof(E));
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