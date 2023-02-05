using Guppy.Common;
using Guppy.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class GuppyExtensions
    {
        private static readonly ConditionalWeakTable<GuppyEngine, HashSet<string>> _tags = new();

        private static HashSet<string> GetTags(GuppyEngine engine)
        {
            if (_tags.TryGetValue(engine, out var tags))
            {
                return tags;
            }

            tags = new HashSet<string>();
            _tags.Add(engine, tags);

            return tags;
        }

        public static GuppyEngine AddTag(this GuppyEngine engine, string tag)
        {
            GetTags(engine).Add(tag);

            return engine;
        }

        public static bool HasTag(this GuppyEngine engine, string tag)
        {
            return GetTags(engine).Contains(tag);
        }

        public static GuppyEngine AddServiceLoader(this GuppyEngine engine, IServiceLoader loader, int? order = null)
        {
            return engine.AddLoader(x => loader.ConfigureServices(x.Services));
        }
    }
}
