using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PassGen.Core
{
    public class PasswordOptions
    {
        public int Length { get; set; } = 16;
        public bool UseLower { get; set; } = true;
        public bool UseUpper { get; set; } = true;
        public bool UseDigits { get; set; } = true;
        public bool UseSymbols { get; set; } = true;
    }

    public static class PasswordGenerator
    {
        private const string Lower = "abcdefghijkmnopqrstuvwxyz";
        private const string Upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        private const string Digits = "23456789";
        private const string Symbols = "!@#$%^&*()-_=+[]{}:,.?";

        public static string BuildPool(PasswordOptions o)
        {
            var sb = new StringBuilder();
            if (o.UseLower) sb.Append(Lower);
            if (o.UseUpper) sb.Append(Upper);
            if (o.UseDigits) sb.Append(Digits);
            if (o.UseSymbols) sb.Append(Symbols);
            return sb.ToString();
        }

        public static string Generate(PasswordOptions o)
        {
            if (o == null) throw new ArgumentNullException("o");

            string pool = BuildPool(o);
            if (pool.Length == 0)
                throw new InvalidOperationException("Select at least one character set.");

            var result = new char[o.Length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < o.Length; i++)
                    result[i] = pool[NextIndex(rng, pool.Length)];
            }
            return new string(result);
        }

        private static int NextIndex(RandomNumberGenerator rng, int exclusiveMax)
        {
            var buf = new byte[4];
            long span = (long)uint.MaxValue + 1;
            long cap = span - (span % exclusiveMax);

            while (true)
            {
                rng.GetBytes(buf);
                uint value = BitConverter.ToUInt32(buf, 0);
                if (value < cap) return (int)(value % (uint)exclusiveMax);
            }
        }
    }
}
