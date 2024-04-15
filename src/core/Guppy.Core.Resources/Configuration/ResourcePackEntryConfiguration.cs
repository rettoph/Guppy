namespace Guppy.Core.Resources.Configuration
{
    public class ResourcePackEntryConfiguration
    {
        private static readonly Dictionary<string, string[]> _defaultImport = new Dictionary<string, string[]>();

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Dictionary<string, string[]> Import { get; set; } = _defaultImport;

        public ResourcePackEntryConfiguration()
        {

        }
    }
}
