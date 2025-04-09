namespace Guppy.Core.Assets.Common.Configuration
{
    public class AssetPackEntryConfiguration
    {
        private static readonly Dictionary<string, string[]> _defaultImport = [];

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Dictionary<string, string[]> Import { get; set; } = _defaultImport;

        public AssetPackEntryConfiguration()
        {

        }
    }
}