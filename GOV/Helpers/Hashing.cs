using System;
using System.Collections.Generic;
using System.Text;

namespace GOV.Helpers
{
    public static class Hashing
    {
        public static string GetHash(string InputString) 
        {
            string OutputString = BCrypt.Net.BCrypt.HashPassword(InputString);
            return OutputString; 
        }
        public static bool CheckHash(string InputString, string SecondInputString)
        {
            bool Verified = BCrypt.Net.BCrypt.Verify(InputString, SecondInputString);
            return Verified;
        }
    }
}
