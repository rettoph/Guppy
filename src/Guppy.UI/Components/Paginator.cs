using Guppy.DependencyInjection;
using Guppy.UI.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Components
{
    public class Paginator : Container<Page>
    {
        #region Private Fields
        private Dictionary<IComponent, Action<Object>> _clickHandlers;
        #endregion

        #region Public Attributes
        public IComponent Page { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _clickHandlers = new Dictionary<IComponent, Action<Object>>();
        }
        protected override void Release()
        {
            base.Release();

            _clickHandlers.Clear();
        }
        #endregion

        #region Frame Methods
        protected override void UpdateChildren(GameTime gameTime)
        {
            // base.UpdateChildren(gameTime);

            this.Page?.TryUpdate(gameTime);
        }

        protected override void DrawChildren(GameTime gameTime)
        {
            // base.DrawChildren(gameTime);

            this.Page?.TryDraw(gameTime);
        }
        #endregion

        #region Helper Methods
        public Action<Object> GetSetPageOnClickHandler(Page page)
        {
            if (!_clickHandlers.ContainsKey(page))
                _clickHandlers[page] = o => this.SetPage(page);

            return _clickHandlers[page];
        }

        public void SetPage(Page page)
        {
            if (!this.Children.Contains(page))
                throw new ArgumentException("Unable to set page to Paginator child.");

            this.Page = page;
        }
        #endregion
    }
}
