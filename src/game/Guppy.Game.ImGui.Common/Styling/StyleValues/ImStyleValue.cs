namespace Guppy.Game.ImGui.Common.Styling.StyleValueResources
{
    internal abstract class ImStyleValue(string? key) : IDisposable
    {
        public string? Key { get; } = key;

        public abstract void Push();

        public abstract void Pop();

        public void Dispose()
        {
            this.Pop();
        }
    }
}
