using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using GOV.Models;
using Newtonsoft.Json;

namespace GOV
{
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged; //this does event things
        public int ID { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int ScoreTotal { get; set; }
        public bool Admin { get; set; }

        public User(int id, string email, string username, string password, int scoreTotal, bool admin)
        {
            ID = id;
            Email = email;
            Username = username;
            Password = password;
            ScoreTotal = scoreTotal;
            Admin = admin;
        }
        public User(string email, string username, string password) //used for login because ID, score and admin shouldnt be generated front end
        {
            Email = email;
            Username = username;
            Password = password;
        }
        public User() { }

        [JsonIgnore]
        public string BasicInfo
        {
            get
            {
                var outputString = "ID: " + ID.ToString() + " - " + "Score Total: " + ScoreTotal.ToString();
                string string1;

                if (Admin != false) { string1 = outputString + " - " + "Type: " + "Admin"; } // doesnt even fucking work
                else { string1 = outputString + " - " + "Type: " + "User"; }

                return string1;
            }
        }


    }
}
