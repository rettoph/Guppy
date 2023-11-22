using Autofac;
using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Enums;
using Guppy.MonoGame.Common;
using Guppy.MonoGame.Common.Enums;
using Microsoft.Xna.Framework;
using System;

namespace Guppy.MonoGame
{
    public abstract class MonoGameGuppy : IGuppy
    {
        private IDrawableComponent[] _drawComponents;
        private IUpdateableComponent[] _updateComponents;

        public IGuppyComponent[] Components { get; private set; }

        public virtual string Name => this.GetType().Name;

        public Guid Id { get; set; }

        public MonoGameGuppy()
        {
            _drawComponents = Array.Empty<IDrawableComponent>();
            _updateComponents = Array.Empty<IUpdateableComponent>();

            this.Components = Array.Empty<IGuppyComponent>();
            this.Id = Guid.NewGuid();
        }

        public event OnEventDelegate<IDisposable>? OnDispose;

        public virtual void Initialize(ILifetimeScope scope)
        {
            this.Components = scope.Resolve<IFiltered<IGuppyComponent>>().Instances.Sequence(InitializeSequence.Initialize).ToArray();

            _drawComponents = this.Components.OfType<IDrawableComponent>().Sequence(DrawSequence.Draw).ToArray();
            _updateComponents = this.Components.OfType<IUpdateableComponent>().Sequence(UpdateSequence.Update).ToArray();

            foreach (IGuppyComponent component in this.Components)
            {
                component.Initialize(this);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            foreach(IDrawableComponent drawable in _drawComponents)
            {
                drawable.Draw(gameTime);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (IUpdateableComponent updateable in _updateComponents)
            {
                updateable.Update(gameTime);
            }
        }

        public virtual void Dispose()
        {
            this.OnDispose?.Invoke(this);
        }
    }
}
