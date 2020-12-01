using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.System
{
    public static class SingleExtensions
    {
        /// <summary>
        /// Save division that will return the <paramref name="fallback"/>
        /// value if <paramref name="denominator"/> is equal to 0.
        /// If <paramref name="fallback"/> is null, then <paramref name="numerator"/>
        /// will be returned instead.
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        static public Single SafeDivision(this Single numerator, Single denominator, Single? fallback = null)
        {
            return (denominator == 0) ? numerator : numerator / denominator;
        }
    }
}
