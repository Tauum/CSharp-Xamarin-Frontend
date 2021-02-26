using System;
using System.Collections.Generic;
using System.Text;

namespace GOV.Models
{
    public class Token // potentially for implementing a GIUD based interface
    {
        public int ID { set; get; }
        public string access_token { set; get; }
        public string error_description { set; get; }
        public DateTime expire_date { set; get; }
        public Token() { }

    }

}
