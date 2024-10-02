namespace Guppy.Core.Common.Utilities
{
    public class BlockList(IEnumerable<string>? whitelist, IEnumerable<string>? blacklist)
    {
        public static readonly BlockList AllowAll = new(null, []);
        public static readonly BlockList BlockAll = new([], null);

        public readonly IEnumerable<string>? Whitelist = whitelist;
        public readonly IEnumerable<string>? Blacklist = blacklist;

        public bool Allows(IEnumerable<string> values)
        {
            bool result = true;

            if (this.Whitelist is not null)
            {
                result &= values.Intersect(this.Whitelist).Any();
            }

            if (this.Blacklist is not null)
            {
                result &= !values.Intersect(this.Blacklist).Any();
            }

            return result;
        }

        public static BlockList CreateWhitelist(params string[] values)
        {
            return new BlockList(values, null);
        }

        public static BlockList CreateBlacklist(params string[] values)
        {
            return new BlockList(null, values);
        }
    }
}
