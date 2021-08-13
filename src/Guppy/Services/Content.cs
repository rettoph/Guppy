using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Services
{
    public sealed class Content : LoaderService<String, String, Object>
    {
        #region Private Fields
        private ContentManager _manager;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _manager);
        }

        protected override void Release()
        {
            base.Release();

            _manager = null;
        }
        #endregion

        /// <summary>
        /// Auto convert the registered String into a usable asset instance.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        protected override Object Configure(IGrouping<String, RegisteredValue> group)
            => _manager.Load<Object>(group.OrderBy(rv => rv.Value).First().Value);

        public T Get<T>(String handle)
        {
            return (T)this[handle];
        }

        public static implicit operator ContentManager(Content content)
            => content._manager;
    }
}
