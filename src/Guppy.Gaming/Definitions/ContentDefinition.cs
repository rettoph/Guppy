using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Definitions
{
    public abstract class ContentDefinition
    {
        public abstract string Key { get; }
        public abstract string Path { get; }

        public abstract IContent BuildContent(ContentManager manager);
    }

    public abstract class ContentDefinition<T> : ContentDefinition
    {
        public override IContent BuildContent(ContentManager manager)
        {
            return new Content<T>(
                key: this.Key, 
                path: this.Path,
                defaultValue: manager.Load<T>(this.Path));
        }
    }
}
