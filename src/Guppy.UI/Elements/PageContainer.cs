using Guppy.DependencyInjection;
using Guppy.Events.Delegates;
using Guppy.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Elements
{
    public class PageContainer : Container<IPage>
    {
        #region Private Fields
        private IPage _activePage;
        #endregion

        #region Public Properties
        public IPage ActivePage => _activePage;
        #endregion

        #region Event Handlers
        public event OnChangedEventDelegate<PageContainer, IPage> OnActivePageChanged;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.Children.OnCreated += this.HandlePageCreated;
        }

        protected override void Release()
        {
            base.Release();

            this.Children.OnCreated -= this.HandlePageCreated;
        }
        #endregion

        #region Helper Methods
        protected override IEnumerable<IPage> GetActiveChildren()
        {
            if (this.ActivePage != default)
                yield return this.ActivePage;
        }

        /// <summary>
        /// Open a new page...
        /// </summary>
        /// <param name="page"></param>
        public void Open(IPage page)
        {
            if(this.OnActivePageChanged.InvokeIf(_activePage != page, this, ref _activePage, page))
            {
                this.Close();
                this.TryCleanBounds();
            }
        }

        /// <summary>
        /// Close the current page...
        /// </summary>
        public void Close()
        {

        }
        #endregion

        #region Event Handlers
        private void HandlePageCreated(IPage item)
        { // Open the new page if no page has been created yet.
            if(this.ActivePage == default)
                this.Open(item);
        }
        #endregion
    }
}
