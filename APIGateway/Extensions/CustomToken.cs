using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GODPCloud.Helpers.Extensions
{
    public static class CustomToken
    {
        private static Random random = new Random();
        public static string Generate()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789QWERTYUIO";
            return new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}