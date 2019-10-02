using Guppy.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Loaders
{
    [IsLoader]
    public class ContentLoader : Loader<String, String, Object>
    {
        private ContentManager _content;
        public ContentLoader(ILogger logger, ContentManager content = null) : base(logger)
        {
            _content = content;
        }

        protected override Object BuildOutput(IGrouping<string, RegisteredValue> registeredValues)
        {
            return _content?.Load<Object>(registeredValues.OrderBy(rv => rv.Priority).First().Value);
        }

        public void TryRegister(String handle, String path, Int32 priority = 100)
        {
            this.Register(handle, path, priority);
        }

        public T TryGet<T>(String handle)
        {
            if(this.values.ContainsKey(handle))
                return (T)this.values[handle];

            return default(T);
        }
    }
}
