using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming
{
    public abstract class Layer : Frameable, ILayer
    {
        private bool _dirtyUpdates;
        private bool _dirtyDraws;
        private int _updateOrder;
        private int _drawOrder;
        private List<ILayerable> _items;
        private List<ILayerable> _updates;
        private List<ILayerable> _draws;

        public readonly string Key;

        string ILayer.Key => this.Key;

        public int UpdateOrder
        {
            get => _updateOrder;
            set => this.OnUpdateOrderChanged!.InvokeIf(value != _updateOrder, this, ref _updateOrder, value);
        }
        public int DrawOrder
        {
            get => _drawOrder;
            set => this.OnDrawOrderChanged!.InvokeIf(value != _drawOrder, this, ref _drawOrder, value);
        }

        public event OnChangedEventDelegate<ILayer, int>? OnUpdateOrderChanged;
        public event OnChangedEventDelegate<ILayer, int>? OnDrawOrderChanged;

        public Layer(string key)
        {
            _items = new List<ILayerable>();
            _updates = new List<ILayerable>();
            _draws = new List<ILayerable>();

            this.Key = key;
        }

        public override void Draw(GameTime gameTime)
        {
            if(_dirtyDraws)
            {
                _draws.Clear();
                _draws.AddRange(_items.Where(x => x.Visible));
            }

            foreach(ILayerable item in _draws)
            {
                item.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (_dirtyUpdates)
            {
                _updates.Clear();
                _updates.AddRange(_items.Where(x => x.Enabled));
            }

            foreach (ILayerable item in _updates)
            {
                item.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected virtual void Add(ILayerable item)
        {
            _items.Add(item);

            item.OnEnabledChanged += this.HandleItemEnabledChanged;
            item.OnVisibleChanged += this.HandleItemVisibleChanged;

            if(item.Enabled)
            {
                _dirtyUpdates = true;
            }

            if (item.Visible)
            {
                _dirtyDraws = true;
            }
        }

        protected virtual void Remove(ILayerable item)
        {
            if(!_items.Remove(item))
            {
                return;
            }

            item.OnEnabledChanged -= this.HandleItemEnabledChanged;
            item.OnVisibleChanged -= this.HandleItemVisibleChanged;

            if (item.Enabled)
            {
                _dirtyUpdates = true;
            }

            if (item.Visible)
            {
                _dirtyDraws = true;
            }
        }

        private void HandleItemEnabledChanged(IFrameable sender, bool args)
        {
            _dirtyUpdates = true;
        }

        private void HandleItemVisibleChanged(IFrameable sender, bool args)
        {
            _dirtyDraws = true;
        }

        void ILayer.Add(ILayerable item)
        {
            this.Add(item);
        }

        void ILayer.Remove(ILayerable item)
        {
            this.Remove(item);
        }

        public IEnumerator<ILayerable> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
