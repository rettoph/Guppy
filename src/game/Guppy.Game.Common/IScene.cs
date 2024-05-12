using Autofac;
using Guppy.Game.Common.Components;

namespace Guppy.Game.Common
{
    public interface IScene : IGuppyUpdateable, IGuppyDrawable
    {
        ulong Id { get; }
        string Name { get; }

        ISceneComponent[] Components { get; }

        void Initialize(ILifetimeScope scope);

        T Resolve<T>() where T : notnull;
    }
}
