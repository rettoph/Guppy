namespace Guppy.Game.Input.Common.Services
{
    public interface IButtonService
    {
        IEnumerable<IInputMessage> Update();
        void Clean(IEnumerable<IButton> buttons);
    }
}