using Guppy.Common;
using Guppy.Configurations;
using Guppy.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyBuilderExtensions
    {
        private static readonly ConditionalWeakTable<GuppyConfiguration, HashSet<string>> _tags = new();

        private static HashSet<string> GetTags(GuppyConfiguration builder)
        {
            if (_tags.TryGetValue(builder, out var tags))
            {
                return tags;
            }

            tags = new HashSet<string>();
            _tags.Add(builder, tags);

            return tags;
        }

        public static GuppyConfiguration AddTag(this GuppyConfiguration builder, string tag)
        {
            GetTags(builder).Add(tag);

            return builder;
        }

        public static bool HasTag(this GuppyConfiguration engine, string tag)
        {
            return GetTags(engine).Contains(tag);
        }

        public static GuppyConfiguration AddServiceLoader(this GuppyConfiguration engine, IServiceLoader loader, int? order = null)
        {
            return engine.AddLoader(x => loader.ConfigureServices(x.Services));
        }
    }
}
