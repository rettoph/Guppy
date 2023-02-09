using Guppy.Common;
using Guppy.Common.Collections;
using Guppy.MonoGame.Collections;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Services
{
    internal sealed class GameComponentService : IGameComponentService, IDisposable
    {
        private ICollectionManager<IGameComponent> _components;

        public IEnumerable<IDrawable> Drawables => _components.Collection<IDrawable>();

        public IEnumerable<IUpdateable> Updateables => _components.Collection<IUpdateable>();

        public GameComponentService(IFiltered<IGameComponent> components)
        {
            _components = new CollectionManager<IGameComponent>()
                .Attach(new DrawableCollection())
                .Attach(new UpdateableCollection())
                .AddRange(components.Instances);

            foreach(IGameComponent component in _components)
            {
                component.Initialize();
            } 
        }

        public void Dispose()
        {
            _components.Clear();
        }

        public void Draw(GameTime gameTime)
        {
            foreach(var drawable in _components.Collection<IDrawable>())
            {
                drawable.Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var updatable in _components.Collection<IUpdateable>())
            {
                updatable.Update(gameTime);
            }
        }

        public IEnumerator<IGameComponent> GetEnumerator()
        {
            return ((IEnumerable<IGameComponent>)_components).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
