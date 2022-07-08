using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    internal sealed class GuppyProvider : IGuppyProvider
    {
        private IServiceProvider _provider;
        private List<IGuppy> _guppies;

        public GuppyProvider(IServiceProvider provider)
        {
            _guppies = new List<IGuppy>();
            _provider = provider;
        }

        TGuppy IGuppyProvider.Create<TGuppy>()
        {
            var guppy = _provider.CreateScope().ServiceProvider.GetRequiredService<TGuppy>();

            _guppies.Add(guppy);

            return guppy;
        }

        IEnumerable<TGuppy> IGuppyProvider.Get<TGuppy>()
        {
            foreach(IGuppy guppy in _guppies)
            {
                if(guppy is TGuppy casted)
                {
                    yield return casted;
                }
            }
        }

        public IEnumerator<IGuppy> GetEnumerator()
        {
            return _guppies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
