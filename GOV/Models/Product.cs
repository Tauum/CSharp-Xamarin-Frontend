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

        private string _pRef;
        public string PRef
        {
            get => _pRef;
            set
            {
                this._pRef = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PRef)));
            }
        }
        public int? ImageID { get; set; } // this can be null
        private Image _image;
        public Image Image
        {
            get => _image;
            set
            {
                this._image = value;
                if (this.Image != null) { this.ImageID = this.Image.ID; }
                else { this.ImageID = null; }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
            }
        }
        public int? CategoryID => this.Category?.ID;

        private Category _category;
        public Category Category
        {
            get => _category;
            set
            {
                this._category = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Category)));
            }
        }

        [JsonIgnore]
        public string BasicInfo
        {
            get
            {
                var outputString = ReleaseYear.ToString() + " - " + Score.ToString();
                string string1;
                if (Category != null) { string1 = outputString + " - " + Category?.Name; } // doesnt even fucking work
                else { string1 = outputString; }

                return string1;
            }
        }

        public Product() { } //default 
        public Product(int id, string name, int releaseYear, string description, int score, string pref)// this is a full product
        {
            ID = id;
            Name = name;
            ReleaseYear = releaseYear;
            Description = description;
            Score = score;
            PRef = pref;
        }
    }
}
