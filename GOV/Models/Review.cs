using System;
using System.Collections.Generic;
using System.Text;

namespace GOV
{
    public class Review
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public int ProductID { get; set; }
        public string Description { get; set; }

        public Review(int id, string username, int productID, string description)
        {
            ID = id;
            Username = Username;
            ProductID = productID;
            Description = description;
        }
    }
}
