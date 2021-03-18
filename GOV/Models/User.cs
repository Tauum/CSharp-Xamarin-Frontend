using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using GOV.Models;

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

        public User()
        {
        }

        public void Total()
        {
            Console.WriteLine("${UID} + {Username} + {ScoreTotal} + {Review} + {Review2}"); //used within list view
        }
    }
}
