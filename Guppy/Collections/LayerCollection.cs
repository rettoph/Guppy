using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Collections
{
    public class LayerCollection : TrackedDisposableCollection<Layer>
    {
        #region Private Fields
        private List<Layer> _list;
        private IOrderedEnumerable<Layer> _drawables;
        private IOrderedEnumerable<Layer> _updatables;
        #endregion

        #region Public Attributes
        public Int32 Count { get { return _list.Count; } }
        public Layer this[int index] { get { return _list[index]; } set { _list[index] = value; } }
        #endregion

        #region Constructors
        public LayerCollection()
        {
            _list = new List<Layer>();
        }
        #endregion

        #region Frame Methods
        public void Draw(GameTime gameTime)
        {
            // Update all the drawables
            foreach (Layer livingObject in _drawables)
                livingObject.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // Update all the updatables
            foreach (Layer livingObject in _updatables)
                livingObject.Update(gameTime);
        }
        #endregion

        #region Collection Methods
        public override void Add(Layer item)
        {
            base.Add(item);
        }

        public override Boolean Remove(Layer item)
        {
            if(base.Remove(item))
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}
