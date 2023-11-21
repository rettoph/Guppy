using Guppy.Attributes;
using Guppy.Resources;
using Guppy.Resources.ResourceTypes;
using Guppy.Serialization;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Guppy.MonoGame.ResourceTypes
{
    [AutoLoad]
    internal class ColorResourceType : ResourceType<Color>
    {
        private readonly IJsonSerializer _json;

        public ColorResourceType(IJsonSerializer json)
        {
            _json = json;
        }

        protected override bool TryResolve(Resource<Color> resource, string root, ref Utf8JsonReader reader, out Color value)
        {
            return _json.TryDeserialize<Color>(ref reader, out value);
        }
    }
}
