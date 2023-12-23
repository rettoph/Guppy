namespace Guppy.Game.MonoGame.Providers
{
    public interface IMenuProvider
    {
        void Add(Menu menu);

        Menu Get(string name);
    }
}
