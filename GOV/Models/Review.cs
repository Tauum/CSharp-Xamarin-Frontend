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

        public Review(int ID, string UserID, User User, int ProductID, Product Product, string Description)//constructor 
        {
            ID = ID;
            UserID = UserID;
            User = User;
            ProductID = ProductID;
            Product = Product;
            Description = Description;
        }
    }
}
