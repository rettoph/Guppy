using Guppy.Common;
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
        private IGameComponent[] _components;
        private List<IDrawable> _drawables;
        private List<IUpdateable> _updatables;
        private bool _dirtyDraws;
        private bool _dirtyUpdates;

        public IEnumerable<IDrawable> Drawables => _drawables;

        public IEnumerable<IUpdateable> Updateables => _updatables;

        public GameComponentService(IFiltered<IGameComponent> components)
        {
            _components = components.Items.ToArray();
            _drawables = this.GetAll<IDrawable>().Where(x => x.Visible).OrderBy(x => x.DrawOrder).ToList();
            _updatables = this.GetAll<IUpdateable>().Where(x => x.Enabled).OrderBy(x => x.UpdateOrder).ToList();

            foreach(IGameComponent component in _components)
            {
                component.Initialize();
            }

            foreach(IDrawable drawable in this.GetAll<IDrawable>())
            {
                drawable.DrawOrderChanged += this.HandleDrawOrderChanged;
                drawable.VisibleChanged += this.HandleVisibleChanged;
            }

            foreach (IUpdateable updatable in this.GetAll<IUpdateable>())
            {
                updatable.UpdateOrderChanged += this.HandleUpdateOrderChanged;
                updatable.EnabledChanged += this.HandleEnabledChanged;
            }
        }

        public void Dispose()
        {
            foreach (IDrawable drawable in this.GetAll<IDrawable>())
            {
                drawable.DrawOrderChanged -= this.HandleDrawOrderChanged;
                drawable.VisibleChanged -= this.HandleVisibleChanged;
            }

            foreach (IUpdateable updatable in this.GetAll<IUpdateable>())
            {
                updatable.UpdateOrderChanged -= this.HandleUpdateOrderChanged;
                updatable.EnabledChanged -= this.HandleEnabledChanged;
            }
        }

        public void Draw(GameTime gameTime)
        {
            if(_dirtyDraws)
            {
                _drawables.Clear();
                _drawables.AddRange(this.GetAll<IDrawable>().Where(x => x.Visible).OrderBy(x => x.DrawOrder));
                _dirtyDraws = false;
            }

            foreach(IDrawable drawable in _drawables)
            {
                drawable.Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            if(_dirtyUpdates)
            {
                _updatables.Clear();
                _updatables.AddRange(this.GetAll<IUpdateable>().Where(x => x.Enabled).OrderBy(x => x.UpdateOrder));
                _dirtyUpdates = false;
            }

            foreach (IUpdateable updatable in _updatables)
            {
                updatable.Update(gameTime);
            }
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

        private void HandleUpdateOrderChanged(object? sender, EventArgs e)
        {
            _dirtyUpdates = true;
        }

        private void HandleDrawOrderChanged(object? sender, EventArgs e)
        {
            _dirtyDraws = true;
        }

        private void HandleEnabledChanged(object? sender, EventArgs e)
        {
            _dirtyUpdates = true;
        }

        private void HandleVisibleChanged(object? sender, EventArgs e)
        {
            _dirtyDraws = true;
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
