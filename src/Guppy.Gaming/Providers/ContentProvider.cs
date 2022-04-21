using Guppy.Gaming.Definitions;
using Guppy.Providers;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Providers
{
    internal sealed class ContentProvider : ResourceProvider<IContent>, IContentProvider
    {
        private ContentManager _manager;
        private Dictionary<string, IContent> _content;

        public ContentProvider(ContentManager manager, IEnumerable<ContentDefinition> definitions)
        {
            _manager = manager;
            _content = definitions.Select(x => x.BuildContent(_manager)).ToDictionary();
        }

        public Content<T> Get<T>(string key)
        {
            if(this.TryGet<T>(key, out Content<T>? content))
            {
                return content;
            }

            throw new ArgumentException();
        }

        public bool TryGet<T>(string key, [MaybeNullWhen(false)] out Content<T> content)
        {
            if(this.TryGet(key, out IContent? lower) && lower is Content<T> casted)
            {
                content = casted;
                return true;
            }

            content = null;
            return false;
        }

        public override bool TryGet(string key, [MaybeNullWhen(false)] out IContent resource)
        {
            return _content.TryGetValue(key, out resource);
        }

        public override void Import(Dictionary<string, string?> values)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<IContent> GetEnumerator()
        {
            return _content.Values.GetEnumerator();
        }
    }
}
