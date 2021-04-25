using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System;
using System.IO;


namespace GOV.Models
{
    public class Image // obvious
    {
        public int ID { get; set; } 
        public string Name { get; set; }
        public byte[] Data { get; set; } // creates a byte stream for the jpg
        public string TypeUsed { get; set; }
        public DateTime DateChanged { get; set; } // for jpg info
        [JsonIgnore]
        public string Extension => Path.GetExtension(Name);

        public Image() { this.DateChanged = DateTime.Now; }
    }
}
