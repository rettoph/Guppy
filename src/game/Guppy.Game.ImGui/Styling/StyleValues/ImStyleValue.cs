namespace Guppy.Game.ImGui.Styling.StyleValueResources
{
    internal abstract class ImStyleValue : IDisposable
    {
        public string? Key { get; }

        public ImStyleValue(string? key)
        {
            this.Key = key;
        }

        public abstract void Push();

        public abstract void Pop();

        public void Dispose()
        {
            this.Pop();
        }
    }
}
