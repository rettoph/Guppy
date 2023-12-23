namespace Guppy.Game.Input.Providers
{
    public interface IButtonProvider
    {
        IEnumerable<IInput> Update();
        void Clean(IEnumerable<IButton> buttons);
    }
}
