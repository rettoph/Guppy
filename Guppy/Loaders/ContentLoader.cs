using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace Guppy.Loaders
{
    /// <summary>
    /// Simple loader used to dynamically load content and
    /// reserve it to a string handle
    /// </summary>
    public class ContentLoader : Loader<String, String, Object>
    {
        private ContentManager _content;

        public ContentLoader(ILogger logger, ContentManager content = null) : base(logger)
        {
            _content = content;
        }

        public TContent Get<TContent>(String handle)
            where TContent : class
        {
            return this.GetValue(handle) as TContent;
        }

        protected override Dictionary<String, Object> BuildValuesTable()
        {
            // Return an empty dictionary if no content manager is defined
            if (_content == null)
                return new Dictionary<String, Object>();

            // Load the requested textures..
            return this.registeredValuesList
                .GroupBy(rv => rv.Handle)
                .Select(g => g.OrderByDescending(rv => rv.Priority)
                    .FirstOrDefault())
                .ToDictionary(
                    keySelector: rv => rv.Handle,
                    elementSelector: rv => _content.Load<Object>(rv.Value));
        }
    }
}
