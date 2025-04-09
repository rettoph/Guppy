namespace Guppy.Game.Input.Common.Services
{
    public interface IInputService
    {
        void Publish(IInputMessage input);
    }
}