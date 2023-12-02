using Autofac;
using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Enums;
using Guppy.Game;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.GUI;
using Microsoft.Xna.Framework;
using System;

namespace Guppy.Game
{
    public abstract class GameGuppy : IGuppy, IGameGuppy
    {
        private IGuppyDrawable[] _drawComponents;
        private IGuppyUpdateable[] _updateComponents;
        private IGuiComponent[] _guiComponents;

        public IGuppyComponent[] Components { get; private set; }

        public virtual string Name => this.GetType().Name;

        public Guid Id { get; set; }

        public GameGuppy()
        {
            _drawComponents = Array.Empty<IGuppyDrawable>();
            _updateComponents = Array.Empty<IGuppyUpdateable>();
            _guiComponents = Array.Empty<IGuiComponent>();

            this.Components = Array.Empty<IGuppyComponent>();
            this.Id = Guid.NewGuid();
        }

        public event OnEventDelegate<IDisposable>? OnDispose;

        public virtual void Initialize(ILifetimeScope scope)
        {
            this.Components = scope.Resolve<IFiltered<IGuppyComponent>>().Instances.Sequence(InitializeSequence.Initialize).ToArray();

            _drawComponents = this.Components.OfType<IGuppyDrawable>().Sequence(DrawSequence.Draw).ToArray();
            _updateComponents = this.Components.OfType<IGuppyUpdateable>().Sequence(UpdateSequence.Update).ToArray();
            _guiComponents = this.Components.OfType<IGuiComponent>().Sequence(DrawSequence.Draw).ToArray();

            foreach (IGuppyComponent component in this.Components)
            {
                component.Initialize(this);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            foreach(IGuppyDrawable drawable in _drawComponents)
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
            foreach(IGuiComponent component in _guiComponents)
            {
                component.DrawGui(gameTime);
            }
        }

        public virtual void Dispose()
        {
            this.OnDispose?.Invoke(this);
        }
    }
}
