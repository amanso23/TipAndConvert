using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipAndConvert.Logic
{
    internal static class TipCalculator
    {
        public static float CalculateTip(float total, float percentage)
        {
            return total * (percentage / 100);
        }
    }
}
