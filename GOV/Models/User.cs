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

        //public int ScoreTotal
        //{
        //    get
        //    {
        //        return Convert.ToInt32(CalculateScore(ID));
        //        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScoreTotal)));
        //    }

        //}
        private int _scoreTotal;
        public int ScoreTotal
        {
            get => _scoreTotal;
            set
            {
                this._scoreTotal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScoreTotal)));
            }
        }

        public bool Admin { get; set; }
        public ICollection<UserProduct> UserProducts { get; set; }

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

        public User(int id, string email, string username, string password,  bool admin)//int scoreTotal,
        {
            ID = id;
            Email = email;
            Username = username;
            Password = password;
            //ScoreTotal = scoreTotal;

            Admin = admin;
        }

        public User(string email, string username, string password) //used for login because ID, score and admin shouldnt be generated front end
        {
            Email = email;
            Username = username;
            Password = password;
        }
        public User() { }
        
        //public async Task<int> CalculateScore(int ID)
        //{
        //    int value = 0;
        //    List<Product> productList;
        //    try
        //    {
        //        productList = await App.DataService.GetAllAsync<Product, int>(ID, "GetProductsForUser");
        //        foreach ( Product x in productList) { value += x.Score; }
        //    }
        //    catch (SystemException) { value = 0; }

        //    return value;
        //} //System.Reflection.TargetInvocationException: 'Exception has been thrown by the target of an invocation.'
    }
}
