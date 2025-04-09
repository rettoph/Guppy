using Standart.Hash.xxHash;

namespace Guppy.Core.Assets.Common.Extensions.System
{
    public static class StringExtensions
    {
#pragma warning disable IDE1006 // Naming Styles
        public static unsafe Guid xxHash128(this string value)
#pragma warning restore IDE1006 // Naming Styles
        {
            uint128 hash = Standart.Hash.xxHash.xxHash128.ComputeHash(value);
            Guid* pHash = (Guid*)&hash;
            return pHash[0];
        }

#pragma warning disable IDE1006 // Naming Styles
        public static unsafe uint xxHash32(this string value)
#pragma warning restore IDE1006 // Naming Styles
        {
            uint hash = Standart.Hash.xxHash.xxHash32.ComputeHash(value);
            return hash;
        }
    }
}