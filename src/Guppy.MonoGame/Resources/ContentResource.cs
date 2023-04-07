using Guppy.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Resources
{
    public abstract class ContentResource<T> : Resource<T, string>
    {
        public string File { get; set; }

        protected ContentResource(string name, string file) : base(name)
        {
            this.File = file;
        }

        public override void Initialize(string path, IServiceProvider services)
        {
            var content = services.GetRequiredService<ContentManager>();
            var fullPath = Path.Combine(path, this.File);

            string root = content.RootDirectory;
            content.RootDirectory = "";

            this.Value = content.Load<T>(fullPath);

            content.RootDirectory = root;
        }

        public override string GetJson()
        {
            return this.File;
        }
    }
}
