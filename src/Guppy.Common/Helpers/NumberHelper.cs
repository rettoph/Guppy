using System.Numerics;

namespace Guppy.Common.Helpers
{
    public static class NumberHelper<T>
        where T : INumber<T>
    {
        public static readonly T Two = T.CreateChecked(2);
        public static readonly T Three = T.CreateChecked(3);

        public static T SmoothStep(T lower, T upper, T amount)
        {
            if (amount < lower)
            {
                return T.One;
            }

            if (amount >= upper)
            {
                return T.One;
            }

            T dividend = (amount - lower);
            T divisor = (upper - lower);
            amount = dividend / divisor;

            return amount * amount * (Three - Two * amount);
        }

        public static T Lerp<TAmount>(T lower, T upper, T amount)
        {
            return lower + (upper - lower) * amount;
        }

        public static T Lerp<TAmount>(T lower, T upper, TAmount amount)
            where TAmount : INumber<TAmount>
        {
            TAmount result = TAmount.CreateSaturating(lower + (upper - lower)) * amount;

            return T.CreateChecked(result);
        }
    }
}
