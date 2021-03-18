using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using GOV.Models;
using Newtonsoft.Json;

namespace GOV
{
    public class Product : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int ID { get; set; }
        public string Name { get; set; }
        public int ReleaseYear { get; set; }
        public string Description { get; set; }
        public int Score { get; set; }

        private string _pRef;//need for below
        public string PRef // this is for setting the product reference 
        {
            get => _pRef;
            set => Setter(value, ref _pRef, nameof(PRef));//reduces line-age
        }

        public int? ImageId { get; set; }// this can be null. Prevent crash

        private Image _image; //need for below
        public Image Image
        {
            get => _image;
            set
            {
                this._image = value;
                if (this.Image != null)
                {
                    this.ImageId = this.Image.ID;
                }
                else
                {
                    this.ImageId = null;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
              // Setter(value, ref image, nameof(Image));
            }
        }

        [JsonIgnore]
        public string BasicInfo => $"{Name} - {ReleaseYear} - {Score}"; 

        public Product(int id, string name, int releaseYear, string description, int score, string pref)// this is a full product
        {
            ID = id;
            Name = name;
            ReleaseYear = releaseYear;
            Description = description;
            Score = score;
            PRef = pref;
        }
        public Product() // default
        {
        }

        public void Setter<T>(T value, ref T field, string name) // nice setter to reduce line-age
        {
            if (!object.Equals(value, field))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
