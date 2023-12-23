namespace Guppy.Common.Utilities
{
    public class BlockList
    {
        public static readonly BlockList AllowAll = new BlockList(null, Enumerable.Empty<string>());
        public static readonly BlockList BlockAll = new BlockList(Enumerable.Empty<string>(), null);

        public readonly IEnumerable<string>? Whitelist;
        public readonly IEnumerable<string>? Blacklist;

        public BlockList(IEnumerable<string>? whitelist, IEnumerable<string>? blacklist)
        {
            this.Whitelist = whitelist;
            this.Blacklist = blacklist;
        }

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
