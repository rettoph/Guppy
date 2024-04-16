namespace Guppy.Game.Input.Common.Services
{
    public interface IButtonService
    {
        IEnumerable<IInput> Update();
        void Clean(IEnumerable<IButton> buttons);
    }
}
