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
        private string pRef;

        public string PRef
        {
            get => pRef;
            set => Setter(value, ref pRef, nameof(PRef));
        }

        public int? ImageId { get; set; }

        private Image image;
        public Image Image
        {
            get => image;
            set => Setter(value, ref image, nameof(Image));
        }

        [JsonIgnore]
        public string BasicInfo => $"{Name} - {ReleaseYear} - {Score}";

        public Product(int id, string name, int releaseYear, string description, int score, string pref)
        {
            ID = id;
            Name = name;
            ReleaseYear = releaseYear;
            Description = description;
            Score = score;
            PRef = pref;
        }
        public Product()
        {
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
