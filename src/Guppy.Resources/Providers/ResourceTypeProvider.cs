using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Guppy.Resources.ResourceTypes;

namespace Guppy.Resources.Providers
{
    internal sealed class ResourceTypeProvider : IResourceTypeProvider
    {
        private readonly Dictionary<string, IResourceType> _types;

        public ResourceTypeProvider(IEnumerable<IResourceType> types)
        {
            _types = types.ToDictionary(x => x.Name, x => x);
        }

        public bool TryGet(string name, [MaybeNullWhen(false)] out IResourceType resourceType)
        {
            return _types.TryGetValue(name, out resourceType);
        }

        /*
        private readonly Regex rgbaArrayRegex = new Regex("^(\\d{1,3}),(\\d{1,3}),(\\d{1,3}),(\\d{1,3})$");

        protected override bool TryResolve(Resource<Color> resource, string input, out Color value)
        {
            Match rgbaArray = rgbaArrayRegex.Match(input);
            if (rgbaArray.Success)
            {
                value = new Color(
                    r: byte.Parse(rgbaArray.Groups[1].Value),
                    g: byte.Parse(rgbaArray.Groups[2].Value),
                    b: byte.Parse(rgbaArray.Groups[3].Value),
                    alpha: byte.Parse(rgbaArray.Groups[4].Value)
                );

                return true;
            }

            value = default!;
            return false;
        }
        */
    }
}
