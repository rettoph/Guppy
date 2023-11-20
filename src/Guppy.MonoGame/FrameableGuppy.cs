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
    public abstract class FrameableGuppy : IGuppy, IGuppyDrawable, IGuppyUpdateable
    {
        private IGuppyDrawable[] _drawComponents;
        private IGuppyUpdateable[] _updateComponents;

        public IGuppyComponent[] Components { get; private set; }


        public FrameableGuppy()
        {
            _drawComponents = Array.Empty<IGuppyDrawable>();
            _updateComponents = Array.Empty<IGuppyUpdateable>();

            this.Components = Array.Empty<IGuppyComponent>();
        }

        public event OnEventDelegate<IDisposable>? OnDispose;

        public virtual void Initialize(ILifetimeScope scope)
        {
            this.Components = scope.Resolve<IFiltered<IGuppyComponent>>().Instances.Sequence(InitializeSequence.Initialize).ToArray();

            _drawComponents = this.Components.OfType<IGuppyDrawable>().Sequence(DrawSequence.Draw).ToArray();
            _updateComponents = this.Components.OfType<IGuppyUpdateable>().Sequence(UpdateSequence.Update).ToArray();

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

        public virtual void Dispose()
        {
            this.OnDispose?.Invoke(this);
        }
    }
}
