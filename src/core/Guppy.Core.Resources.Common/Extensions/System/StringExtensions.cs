using Standart.Hash.xxHash;

namespace Guppy.Core.Resources.Common.Extensions.System
{
    public static class StringExtensions
    {
        public static unsafe Guid xxHash128(this string value)
        {
            uint128 hash = Standart.Hash.xxHash.xxHash128.ComputeHash(value);
            Guid* pHash = (Guid*)&hash;
            return pHash[0];
        }

        public static unsafe uint xxHash32(this string value)
        {
            uint hash = Standart.Hash.xxHash.xxHash32.ComputeHash(value);
            return hash;
        }
    }
}
