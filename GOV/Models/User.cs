using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
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

        [JsonIgnore]
        private int _scoreTotal;

        public int ScoreTotal { get; set; }
        //public int ScoreTotal
        //{
        //    get => _scoreTotal;
        //    set
        //    {
        //        this._scoreTotal = Convert.ToInt32(CalculateScore(ID));
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScoreTotal)));
        //    }
        //}

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

                if (Admin != false) { string1 = outputString + " - " + "Type: " + "Admin"; }
                else { string1 = outputString + " - " + "Type: " + "User"; }

                return string1;
            }
        }

        public async Task<int> CalculateScore(int ID) //Newtonsoft.Json.JsonSerializationException: 'Error setting value to 'ScoreTotal' on 'GOV.User'.'
        {
            int value = 0;
            List<Product> productList;
            try
            {
                productList = await App.DataService.GetAllAsync<Product, int>(ID, "GetProductsForUser");
                foreach ( Product x in productList) { value += x.Score; }
            }
            catch { value = 0; }

            return value;
        }
    }
}
