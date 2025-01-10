namespace Guppy.Game.ImGui.Common.Styling.StyleValues
{
    public abstract class ImStyleValue(string? key) : IDisposable
    {
        public string? Key { get; } = key;

        public abstract void Push();

        public abstract void Pop();

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose() => this.Pop();
    }
}