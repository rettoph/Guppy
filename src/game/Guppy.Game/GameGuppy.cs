using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.ImGui.Common;
using Microsoft.Xna.Framework;
using Standart.Hash.xxHash;
using System.Runtime.InteropServices;

namespace Guppy.Game
{
    public abstract class GameGuppy : IGuppy, IGameGuppy
    {
        private static Dictionary<Type, short> _count = new Dictionary<Type, short>();
        private static ulong CalculateId(GameGuppy instance)
        {
            ref short count = ref CollectionsMarshal.GetValueRefOrAddDefault(_count, instance.GetType(), out bool exists);
            uint hash = xxHash32.ComputeHash(instance.GetType().AssemblyQualifiedName);

            return (ulong)hash << 32 | (ulong)count++;
        }

        private IGuppyDrawable[] _drawComponents;
        private IGuppyUpdateable[] _updateComponents;
        private IImGuiComponent[] _imguiComponents;

        public IGuppyComponent[] Components { get; private set; }
        public ILifetimeScope Scope { get; private set; }

        public virtual string Name => this.GetType().Name;

        public ulong Id { get; set; }

        public GameGuppy()
        {
            _drawComponents = Array.Empty<IGuppyDrawable>();
            _updateComponents = Array.Empty<IGuppyUpdateable>();
            _imguiComponents = Array.Empty<IImGuiComponent>();

            this.Components = Array.Empty<IGuppyComponent>();
            this.Scope = null!;


            this.Id = CalculateId(this);
        }

        public event OnEventDelegate<IDisposable>? OnDispose;

        public virtual void Initialize(ILifetimeScope scope)
        {
            this.Components = scope.Resolve<IFiltered<IGuppyComponent>>().Sequence(InitializeSequence.Initialize).ToArray();

            _drawComponents = this.Components.OfType<IGuppyDrawable>().Sequence(DrawSequence.Draw).ToArray();
            _updateComponents = this.Components.OfType<IGuppyUpdateable>().Sequence(UpdateSequence.Update).ToArray();
            _imguiComponents = this.Components.OfType<IImGuiComponent>().Sequence(DrawSequence.Draw).ToArray();

            foreach (IGuppyComponent component in this.Components)
            {
                component.Initialize(this);
            }

            this.Scope = scope;
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

        public virtual void DrawGui(GameTime gameTime)
        {
            foreach (IImGuiComponent component in _imguiComponents)
            {
                component.DrawImGui(gameTime);
            }
        }

        public virtual void Dispose()
        {
            this.OnDispose?.Invoke(this);
        }
    }
}
