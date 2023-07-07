using OtpNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BVH.FB.Common
{
    public static class Utilities
    {
        private static Random rand = new Random();
        public static string RandomString(int length)
        {
            //const string pool = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string poolLower = "abcdefghijklmnopqrstuvwxyz";
            const string poolNum = "0123456789";
            const string poolUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string poolSymbol = "@$!";
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                int selectPool = i > 3 ? rand.Next(0, 5) % 4 : i;
                var c = new char();
                switch (selectPool)
                {
                    case 1: c = poolNum[rand.Next(0, poolNum.Length)]; break;
                    case 2: c = poolUpper[rand.Next(0, poolUpper.Length)]; break;
                    case 3: c = poolSymbol[rand.Next(0, poolSymbol.Length)]; break;
                    default: c = poolLower[rand.Next(0, poolLower.Length)]; break;
                }
                builder.Append(c);
            }
            return builder.ToString();
        }
        public static void CreateFile(string path, string initString)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.AppendAllText(path, initString);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static string GetCookie(CookieContainer container, string url, string cookieName)
        {
            var result = String.Empty;

            IEnumerable<Cookie> responseCookies = container.GetCookies(new Uri(url)).Cast<Cookie>();
            if (responseCookies != null)
            {
                var cookie = responseCookies.FirstOrDefault(_ => _.Name == cookieName);
                if (cookie != null)
                {
                    result = cookie.Value;
                }

            }

            return result;
        }

        public static string Get2FACode(string secretKey)
        {
            // Create an instance of the TOTP generator
            var totp = new Totp(Base32Encoding.ToBytes(secretKey));
            return totp.ComputeTotp();
        }

        private static string[] _allNames;
        public static string RandomName()
        {
            _allNames = _allNames ?? File.ReadAllLines("Assets/name.txt");
            Random rnd1 = new Random();
            return _allNames[rnd1.Next(_allNames.Length)];
        }

        public static string RandomFile(string path)
        {
            var rand = new Random();
            var files = Directory.GetFiles(path);
            return files[rand.Next(files.Length)];
        }
    }
}
