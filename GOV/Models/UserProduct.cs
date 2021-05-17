using System;
using System.Collections.Generic;
using System.Text;

namespace GOV.Models
{
    public class UserProduct
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
