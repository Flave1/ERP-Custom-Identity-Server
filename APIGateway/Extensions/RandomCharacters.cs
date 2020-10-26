using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Extensions
{
    public static class RandomCharacters
    {
        private static Random random = new Random();
        public static string GenerateByAnyLength(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GeneratePassword()
        {
            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var upperCase = new string(Enumerable.Repeat(upperChars, 2).Select(s => s[random.Next(s.Length)]).ToArray());

            const string lowersChars = "qwertyuiopasdfghjklzxcvbnm";
            var lowerCase = new string(Enumerable.Repeat(lowersChars, 5).Select(s => s[random.Next(s.Length)]).ToArray());

            const string numbers = "0123456789";
            var Numbers = new string(Enumerable.Repeat(numbers, 1).Select(s => s[random.Next(s.Length)]).ToArray());

            const string symbols = "@";
            var Symbols = new string(Enumerable.Repeat(symbols, 1).Select(s => s[random.Next(s.Length)]).ToArray());
            return upperCase + lowerCase + Symbols + Numbers;
        }
    }
}
