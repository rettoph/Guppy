using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Microsoft.Xna.Framework;
using Standart.Hash.xxHash;
using System.Runtime.InteropServices;

namespace Guppy.Game.Common
{
    public abstract class Scene : IScene
    {
        private static Dictionary<Type, ushort> _count = new Dictionary<Type, ushort>();
        private static ulong CalculateId(Scene instance)
        {
            ref ushort count = ref CollectionsMarshal.GetValueRefOrAddDefault(_count, instance.GetType(), out bool exists);
            uint hash = xxHash32.ComputeHash(instance.GetType().AssemblyQualifiedName);

            return (ulong)hash << 32 | (ulong)count++;
        }

        private ILifetimeScope _scope;
        private IGuppyDrawable[] _drawComponents;
        private IGuppyUpdateable[] _updateComponents;

        public ISceneComponent[] Components { get; private set; }

        public virtual string Name => this.GetType().Name;

        public ulong Id { get; }

        public Scene()
        {
            _scope = null!;
            _drawComponents = Array.Empty<IGuppyDrawable>();
            _updateComponents = Array.Empty<IGuppyUpdateable>();

            this.Components = Array.Empty<ISceneComponent>();


            this.Id = CalculateId(this);
        }

        void IScene.Initialize(ILifetimeScope scope)
        {
            this.Initialize(scope);
        }

        protected virtual void Initialize(ILifetimeScope scope)
        {
            _scope = scope;

            this.Components = scope.Resolve<IFiltered<ISceneComponent>>().Sequence(InitializeSequence.Initialize).ToArray();

            foreach (ISceneComponent component in this.Components)
            {
                component.Initialize();
            }

            _drawComponents = this.Components.OfType<IGuppyDrawable>().Sequence(DrawSequence.Draw).ToArray();
            _updateComponents = this.Components.OfType<IGuppyUpdateable>().Sequence(UpdateSequence.Update).ToArray();
        }

        public virtual void Draw(GameTime gameTime)
        {
            foreach (IGuppyDrawable drawable in _drawComponents)
            {
                drawable.Draw(gameTime);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (IGuppyUpdateable updateable in _updateComponents)
            {
                updateable.Update(gameTime);
            }
        }

        public T Resolve<T>()
            where T : notnull
        {
            return _scope.Resolve<T>();
        }
    }
}
