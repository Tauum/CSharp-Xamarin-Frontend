using System;
using System.Collections.Generic;
using System.Text;
using GOV.Models;//maybe needed?

namespace GOV
{
    public class Review
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }// foreign key  this points to the user but is not used
        public int ProductID { get; set; }
        public Product Product { get; set; } // foreign key  this points to the product but is not used
        public string Description { get; set; }
        public int Visible { get; set; }

        public Review(int id, int userID, User user, int productID, Product product, string description, int visible)//constructor 
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
