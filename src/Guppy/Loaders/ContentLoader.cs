using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Loaders
{
    [AutoLoad]
    public sealed class ContentLoader : Loader<String, String, Object>
    {
        #region Private Fields
        private ContentManager _content;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _content);
        }
        #endregion

        /// <summary>
        /// Auto convert the registered String into a usable asset instance.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        protected override Object Configure(IGrouping<String, RegisteredValue> group)
            => _content.Load<Object>(group.OrderBy(rv => rv.Value).First().Value);

        public T Get<T>(String handle)
        {
            return (T)this[handle];
        }
    }
}
