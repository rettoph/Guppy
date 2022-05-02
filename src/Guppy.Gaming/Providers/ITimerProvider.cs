using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Providers
{
    public interface ITimerProvider : IEnumerable<Timer>
    {
        Timer this[string key] { get; }

        Timer Get(string key);
        bool TryGet(string key, [MaybeNullWhen(false)] out Timer timer);
        Timer Create(string key, double interval, bool enabled);

        internal void Update(GameTime gameTime);
    }
}
