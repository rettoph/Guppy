namespace Guppy.Resources.Configuration
{
    internal class ResourcePackConfiguration
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public Dictionary<string, string[]> Import { get; init; } = new Dictionary<string, string[]>();
    }
}
