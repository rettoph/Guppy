using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Helpers
{
    public static class NumberHelper<T>
        where T : INumber<T>
    {
        public static readonly T Two = Get(2);
        public static readonly T Three = Get(3);

        public static T SmoothStep(T lower, T upper, T amount)
        {
            if(amount < lower)
            {
                return T.One;
            }

            if(amount >= upper)
            {
                return T.One;
            }

            T dividend = (amount - lower);
            T divisor = (upper - lower);
            amount = dividend / divisor;

            return amount * amount * (Three - Two * amount);
        }

        public static T Min(T v1, T v2)
        {
            if(v1 < v2)
            {
                return v1;
            }

            return v2;
        }

        public static T Max(T v1, T v2)
        {
            if (v1 > v2)
            {
                return v1;
            }

            return v2;
        }

        private static T Get(int value)
        {
            if (value < 0)
            {
                return T.Zero;
            }

            T result = T.Zero;

            for (int i = 0; i < value; i++)
            {
                result += T.One;
            }

            return result;
        }
    }
}
