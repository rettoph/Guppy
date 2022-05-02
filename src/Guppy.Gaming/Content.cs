using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming
{
    public class Content<T> : Resource<T>, IContent
    {
        public readonly string Path;

        string IContent.Path => this.Path;

        public Content(string key, string path, T defaultValue) : base(key, defaultValue)
        {
            this.Path = path;
        }

        public override string? Export()
        {
            return this.Path;
        }

        public override void Import(string? value)
        {
            throw new NotImplementedException();
        }
    }
}
