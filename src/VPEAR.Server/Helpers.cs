using System;
using System.Linq;

namespace VPEAR.Server
{
    internal static class Helpers
    {
        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+/=";
            var random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                .Select(c => c[random.Next(chars.Length)])
                .ToArray());
        }
    }
}
