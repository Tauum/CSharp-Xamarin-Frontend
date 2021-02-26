using System;
using System.Collections.Generic;
using System.Text;

namespace GOV.Helpers
{
    public static class Hashing //this functionality can be called globaly with using.
    {
        public static string GetHash(string InputString)  //obvious
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(4); //make sure they are all the same because prototype but obviously expanded in RL scenario
            string OutputString = BCrypt.Net.BCrypt.HashPassword(InputString, salt); //puts input and salt together to ensure consistent 
            return OutputString;
        }
        public static bool CheckHash(string InputString, string SecondInputString) //obvious
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(4); //makes sure they are all the same because prototype but obviously expanded in RL scenario
            bool Verified = BCrypt.Net.BCrypt.Verify(InputString, SecondInputString); // obvious
            return Verified;
        }
    }
}
