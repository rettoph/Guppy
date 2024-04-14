namespace Guppy.Game.Input.Services
{
    public interface IButtonService
    {
        IEnumerable<IInput> Update();
        void Clean(IEnumerable<IButton> buttons);
    }
}
