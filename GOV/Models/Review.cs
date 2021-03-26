using System;
using System.Collections.Generic;
using System.Text;
using GOV.Models;//maybe needed?
using Newtonsoft.Json;

namespace GOV
{
    public class Review
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public string Description { get; set; }
        public bool Visible { get; set; }

        public Review(int id, int userID, User user, int productID, Product product, string description, bool visible) //constructor 
        {
            ID = id;
            UserID = userID;
            User = user;
            ProductID = productID;
            Product = product;
            Description = description;
            Visible = visible;
        }
        public Review(){ }
    }
}
