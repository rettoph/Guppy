using Standart.Hash.xxHash;

namespace Guppy.Resources.Extensions.System
{
    public static class StringExtensions
    {
        public static unsafe Guid xxHash128(this string value)
        {
            uint128 nameHash = Standart.Hash.xxHash.xxHash128.ComputeHash(value);
            Guid* pNameHash = (Guid*)&nameHash;
            return pNameHash[0];
        }
    }
}
