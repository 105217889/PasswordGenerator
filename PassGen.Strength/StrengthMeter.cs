using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassGen.Core;

namespace PassGen.Strength
{
    public static class StrengthMeter
    {
        public static double BitsOfEntropy(PasswordOptions o)
        {
            int pool = PasswordGenerator.BuildPool(o).Length;
            return pool <= 1 ? 0 : o.Length * Math.Log(pool, 2);
        }

        public static string Rate(double bits)
        {
            if (bits < 45) return "Weak";
            if (bits < 65) return "Fair";
            if (bits < 90) return "Strong";
            return "Very strong";
        }
    }
}
