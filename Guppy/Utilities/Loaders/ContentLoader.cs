using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Content;

namespace Guppy.Utilities.Loaders
{
    /// <summary>
    /// Simple loader used to dynamically load content and
    /// reserve it to a string handle
    /// </summary>
    [IsLoader]
    public class ContentLoader : Loader<String, String, Object>
    {
        #region Private Fields
        private ContentManager _content;
        #endregion

        #region Constructors
        public ContentLoader(ILogger logger, ContentManager content = null) : base(logger)
        {
            _content = content;
        }
        #endregion

        #region Helper Methods
        public void TryRegister(String handle, String path, UInt16 priority = 100)
        {
            this.logger.LogDebug($"Registering new Content({priority}). Handle => '{handle}', Path => '{path}'");

            base.Register(handle, path, priority);
        }

        public TContent Get<TContent>(String handle)
            where TContent : class
        {
            return this.GetValue(handle) as TContent;
        }
        #endregion

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
