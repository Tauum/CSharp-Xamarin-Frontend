using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace GOV.Models
{
    public class Category : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int ID { get; set; }
        public string Name { get; set; }

        private ObservableCollection<Object> _products;
        public ObservableCollection<Object> Products
        {
            get => _products;

            set { if (this._products != value) { this._products = value; }}
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) { return false; }

            else
            {
                Category g = (Category)obj;
                return (ID == g.ID) && (Name == g.Name);
            }
        }

        public override int GetHashCode() => base.GetHashCode();

        public Category() { this.Products = new ObservableCollection<object>(); }
    }
}