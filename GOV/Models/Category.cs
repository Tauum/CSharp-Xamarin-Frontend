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

        public int Id { get; set; }
        public string Name { get; set; }

        private ObservableCollection<Object> _products;
        public ObservableCollection<Object> Products
        {
            get { return this._products; }
            set
            {
                if (this._products != value) { this._products = value; }
            }
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) { return false; }

            else
            {
                Category g = (Category)obj;
                return (Id == g.Id) && (Name == g.Name);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Category()
        {
            this.Products = new ObservableCollection<object>();
        }
    }
}