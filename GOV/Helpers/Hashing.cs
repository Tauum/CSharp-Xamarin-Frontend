using System;
using System.Collections.Generic;
using System.Text;

namespace GOV.Helpers
{
    public static class Hashing
    {
        public static string GetHash(string InputString) 
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(6);
            string OutputString = BCrypt.Net.BCrypt.HashPassword(InputString, salt);
            return OutputString;
        }
        public static bool CheckHash(string InputString, string SecondInputString)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(6);
            bool Verified = BCrypt.Net.BCrypt.Verify(InputString, SecondInputString);
            return Verified;
        }
    }
}
