namespace Guppy.MonoGame.UI.Utilities
{
    public abstract class ImGuiPushValue<TWhat>
        where TWhat : struct, Enum
    {
        public readonly TWhat What;

        internal ImGuiPushValue(TWhat what)
        {
            What = what;
        }

        public abstract void Push();
    }

    public abstract class ImGuiPushValue<TWhat, TValue> : ImGuiPushValue<TWhat>
        where TWhat : struct, Enum
        where TValue : struct
    {
        public TValue Value;

        protected ImGuiPushValue(TWhat what, ref TValue value) : base(what)
        {
            Value = value;
        }
    }
}
