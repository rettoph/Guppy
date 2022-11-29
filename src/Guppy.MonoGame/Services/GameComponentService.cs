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
        private DrawableCollection _drawables;
        private UpdateableCollection _updatables;
        private CollectionManager<IGameComponent> _components;

        public IEnumerable<IDrawable> Drawables => _drawables;

        public IEnumerable<IUpdateable> Updateables => _updatables;

        public GameComponentService(IFiltered<IGameComponent> components)
        {
            _drawables = new DrawableCollection();
            _updatables = new UpdateableCollection();
            _components = new CollectionManager<IGameComponent>(components.Items, _drawables, _updatables);

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
            _drawables.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            _updatables.Update(gameTime);
        }

        private IEnumerable<T> GetAll<T>()
        {
            foreach (IGameComponent component in _components)
            {
                if (component is T casted)
                {
                    yield return casted;
                }
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
